using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Activity
{
    protected string _name;
    protected string _description;
    protected int duration;

    public Activity(int duration)
    {
        this.duration = duration;
    }

    public virtual void DisplayStartingMessage()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Starting {_name}...");
        Console.WriteLine(_description);
        Console.ResetColor();
    }

    public virtual void DisplayEndingMessage(TimeSpan elapsedTime)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Congratulations! You have completed the {_name}.");
        Console.WriteLine($"Total duration: {Math.Round(elapsedTime.TotalSeconds)} seconds");
        Console.ResetColor();
    }

    public void ShowSpinner(int seconds)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Loading...");
        for (int i = 0; i < seconds; i++)
        {
            Console.Write("/");
            Thread.Sleep(1000);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write("-");
            Thread.Sleep(1000);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write("\\");
            Thread.Sleep(1000);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    public async Task ShowCountDownAsync(int seconds, CancellationToken token)
    {
        for (int i = seconds; i > 0; i--)
        {
            if (token.IsCancellationRequested)
                return;

            Console.WriteLine($"Time left: {i} seconds");
            await Task.Delay(1000, token);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }

    public virtual Task Run(CancellationTokenSource tokenSource)
    {
        throw new NotImplementedException();
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity(int duration) : base(duration)
    {
        _name = "Breathing Activity";
        _description = "This activity will help you relax by guiding you through deep breathing exercises.";
    }

    public override async Task Run(CancellationTokenSource tokenSource)
    {
        DisplayStartingMessage();
        Console.WriteLine("Get ready to start breathing...");

        DateTime startTime = DateTime.Now;
        TimeSpan elapsedTime;

        // Run breathing cycles until the duration elapses
        while (duration > 0)
        {
            Console.WriteLine("Breathe in...");
            await SimulateBreathAsync(5, tokenSource.Token); // Breathe in for 5 seconds
            if (tokenSource.Token.IsCancellationRequested)
                return; // Exit if cancellation requested

            int remainingDuration = duration - 5; // Adjust remaining duration after breathing in

            int breathOutDuration = Math.Min(remainingDuration, 5); // Ensure breathing out at least 5 seconds
            Console.WriteLine("Breathe out...");
            await SimulateBreathAsync(breathOutDuration, tokenSource.Token); // Breathe out
            if (tokenSource.Token.IsCancellationRequested)
                return; // Exit if cancellation requested

            duration -= (5 + breathOutDuration); // Reduce the duration by the time taken for one breathing cycle (5 + breathOutDuration)
        }

        elapsedTime = DateTime.Now - startTime;
        Console.WriteLine("Time's up!");
        DisplayEndingMessage(elapsedTime);
    }

    // Simulate the breathing process with an animation
    private async Task SimulateBreathAsync(int breathDuration, CancellationToken token)
    {
        for (int i = 0; i < breathDuration; i++)
        {
            if (token.IsCancellationRequested)
                return;

            Console.WriteLine($"Breathe... ({i + 1}/{breathDuration})");
            await Task.Delay(1000);
        }
    }
}

class ListingActivity : Activity
{
    public ListingActivity(int duration) : base(duration)
    {
        _name = "Listing Activity";
        _description = "Reflect on the good things in your life by listing as many items as you can.";
    }

    public override async Task Run(CancellationTokenSource tokenSource)
    {
        DisplayStartingMessage();
        Console.WriteLine(_description);
        Console.WriteLine($"You have {duration} seconds to list as many items as you can. Press Enter after each item, or type 'done' when finished.");
        Console.WriteLine("Start listing:");

        DateTime startTime = DateTime.Now;
        TimeSpan elapsedTime;

        List<string> responses = new List<string>();

        Task countDownTask = ShowCountDownAsync(duration, tokenSource.Token);

        while (true)
        {
            string response = Console.ReadLine();

            if (response.ToLower() == "done")
                break;

            responses.Add(response);
        }

        tokenSource.Cancel();

        elapsedTime = DateTime.Now - startTime;
        Console.WriteLine("Time's up!");
        Console.WriteLine("Here are your responses:");
        foreach (var response in responses)
        {
            Console.WriteLine(response);
        }

        DisplayEndingMessage(elapsedTime);
    }
}

class ReflectingActivity : Activity
{
    private List<string> questions;

    public ReflectingActivity(int duration) : base(duration)
    {
        _name = "Reflecting Activity";
        _description = "Reflect on times in your life when you have shown strength and resilience.";
        questions = new List<string> {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };
    }

    public override async Task Run(CancellationTokenSource tokenSource)
    {
        DisplayStartingMessage();
        Console.WriteLine(_description);
        Thread.Sleep(2000);
        Console.WriteLine("Press Enter to see the next question. Type 'done' to end the activity.");
        Console.WriteLine("Reflect on the following questions:");

        DateTime startTime = DateTime.Now;
        TimeSpan elapsedTime;

        // Start a separate thread to display the spinner animation
        Task spinnerTask = Task.Run(() => ShowSpinnerWhileWaiting(tokenSource.Token));

        foreach (var question in questions)
        {
            Console.WriteLine(question);
            await WaitAndContinue(tokenSource.Token); // Wait for Enter key press to continue
            if (tokenSource.Token.IsCancellationRequested)
                return; // Exit if cancellation requested
        }

        tokenSource.Cancel();
        spinnerTask.Wait(); // Wait for the spinner task to finish

        elapsedTime = DateTime.Now - startTime;
        DisplayEndingMessage(elapsedTime);
    }

    // Display spinner animation while waiting for user input
    private void ShowSpinnerWhileWaiting(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Console.Write("\r-");
            Thread.Sleep(200);
            Console.Write("\r\\");
            Thread.Sleep(200);
            Console.Write("\r|");
            Thread.Sleep(200);
            Console.Write("\r/");
            Thread.Sleep(200);
        }
    }

    // Wait for Enter key press to continue
    private async Task WaitAndContinue(CancellationToken token)
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return;
                }
            }

            await Task.Delay(100);
            if (token.IsCancellationRequested)
                return; // Exit if cancellation requested
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to the Mindfulness Program!");

        while (true)
        {
            Console.WriteLine("\nChoose an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflecting Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Exit");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid input! Please enter a number between 1 and 4.");
            }

            if (choice == 4)
            {
                Console.WriteLine("Exiting the program...");
                break;
            }

            Console.WriteLine("Enter the duration (in seconds) for the activity:");
            int duration;
            while (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
            {
                Console.WriteLine("Invalid input! Please enter a positive integer for the duration.");
            }

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            Activity activity = null;
            switch (choice)
            {
                case 1:
                    activity = new BreathingActivity(duration);
                    break;
                case 2:
                    activity = new ReflectingActivity(duration);
                    break;
                case 3:
                    activity = new ListingActivity(duration);
                    break;
            }

            if (activity != null)
            {
                await activity.Run(tokenSource);
            }
        }
    }
}
