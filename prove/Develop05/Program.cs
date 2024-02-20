using System;
using System.Collections.Generic;
using System.IO;

public class GoalManager
{
    private List<Goal> _goals;
    public int Score { get; set; }

    public GoalManager()
    {
        _goals = new List<Goal>();
        Score = 0;
    }

    public List<Goal> GetGoals() => _goals;

    public void Start()
    {
        Console.WriteLine($"Welcome to the Eternal Quest Program. Your current score: {Score}");
        while (true)
        {
            DisplayMainMenu();
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ListGoalDetails();
                    break;
                case "2":
                    CreateGoal();
                    break;
                case "3":
                    RecordEvent();
                    break;
                case "4":
                    SaveGoals();
                    break;
                case "5":
                    LoadGoals();
                    break;
                case "6":
                    DisplayPlayerInfo();
                    break;
                case "7":
                    Console.WriteLine("Exiting the program...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void DisplayMainMenu()
    {
        Console.WriteLine("\nMain Menu:");
        Console.WriteLine("1. View Goals");
        Console.WriteLine("2. Add New Goal");
        Console.WriteLine("3. Record Event");
        Console.WriteLine("4. Save Goals");
        Console.WriteLine("5. Load Goals");
        Console.WriteLine("6. Display Score");
        Console.WriteLine("7. Exit");
        Console.Write("Enter your choice: ");
    }

    public void DisplayPlayerInfo()
    {
        Console.WriteLine($"Your current score: {Score}");
    }

    public void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available to record event.");
            return;
        }

        Console.WriteLine("Select the goal to record event:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
        Console.Write("Enter your choice: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > _goals.Count)
        {
            Console.WriteLine("Invalid choice.");
            return;
        }

        Goal selectedGoal = _goals[index - 1];
        selectedGoal.RecordEvent(this); // Pass the GoalManager instance
        Console.WriteLine($"Your current score: {Score}");

        if (selectedGoal.IsComplete())
        {
            Console.WriteLine($"Congratulations! You completed the goal '{selectedGoal.GetName()}'");
        }
        else
        {
            Console.WriteLine($"Event recorded for goal '{selectedGoal.GetName()}'");
        }
    }

    public void ListGoalDetails()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available.");
            return;
        }

        Console.WriteLine("Goals List:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
        Console.WriteLine($"Your current score: {Score}");
    }

    public void CreateGoal()
    {
        Console.WriteLine("Select goal type:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Checklist Goal");
        Console.WriteLine("3. Eternal Goal");
        Console.WriteLine("4. Negative Goal");
        Console.WriteLine("5. Return to Main Menu");
        Console.Write("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CreateSimpleGoal();
                break;
            case "2":
                CreateChecklistGoal();
                break;
            case "3":
                CreateEternalGoal();
                break;
            case "4":
                CreateNegativeGoal();
                break;
            case "5":
                Console.WriteLine("Returning to Main Menu...");
                break;
            default:
                Console.WriteLine("Invalid choice. Creating a Simple Goal by default.");
                CreateSimpleGoal();
                break;
        }
    }

    public void CreateSimpleGoal()
    {
        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter goal points: ");
        if (!int.TryParse(Console.ReadLine(), out int points))
        {
            Console.WriteLine("Invalid points value. Creating a goal with 0 points.");
            points = 0;
        }

        _goals.Add(new SimpleGoal(name, description, points, false));
        Console.WriteLine("Simple Goal added successfully!");
    }

    public void CreateChecklistGoal()
    {
        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter goal points: ");
        if (!int.TryParse(Console.ReadLine(), out int points))
        {
            Console.WriteLine("Invalid points value. Creating a goal with 0 points.");
            points = 0;
        }
        Console.Write("Enter target completion count: ");
        if (!int.TryParse(Console.ReadLine(), out int target))
        {
            Console.WriteLine("Invalid target value. Creating a goal with target count 0.");
            target = 0;
        }
        Console.Write("Enter bonus points: ");
        if (!int.TryParse(Console.ReadLine(), out int bonus))
        {
            Console.WriteLine("Invalid bonus value. Creating a goal with bonus points 0.");
            bonus = 0;
        }

        _goals.Add(new ChecklistGoal(name, description, points, target, false, 0, bonus));
        Console.WriteLine("Checklist Goal added successfully!");
    }

    public void CreateEternalGoal()
    {
        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter goal points: ");
        if (!int.TryParse(Console.ReadLine(), out int points))
        {
            Console.WriteLine("Invalid points value. Creating a goal with 0 points.");
            points = 0;
        }

        _goals.Add(new EternalGoal(name, description, points, false));
        Console.WriteLine("Eternal Goal added successfully!");
    }

    public void CreateNegativeGoal()
    {
        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter penalty points: ");
        if (!int.TryParse(Console.ReadLine(), out int penalty))
        {
            Console.WriteLine("Invalid penalty value. Creating a goal with penalty 0.");
            penalty = 0;
        }

        _goals.Add(new NegativeGoal(name, description, 0, penalty));
        Console.WriteLine("Negative Goal added successfully!");
    }

    public void SaveGoals()
    {
        Console.Write("Enter file name to save goals (with .txt extension): ");
        string fileName = Console.ReadLine();
        try
        {
            GoalFileHandler.SaveGoals(fileName, _goals, Score);
            Console.WriteLine("Goals saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving goals: {ex.Message}");
        }
    }

    public void LoadGoals()
    {
        Console.Write("Enter file name to load goals from (with .txt extension): ");
        string fileName = Console.ReadLine();
        try
        {
            var loadedData = GoalFileHandler.LoadGoals(fileName);
            _goals = loadedData.Goals;
            Score = loadedData.Score;
            Console.WriteLine("Goals loaded successfully!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading goals: {ex.Message}");
        }
    }
}

public static class GoalFileHandler
{
    public static void SaveGoals(string fileName, List<Goal> goals, int score)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine($"Score: {score}");

            foreach (var goal in goals)
            {
                writer.WriteLine(goal.GetSaveString());
            }
        }
    }

    public static (List<Goal> Goals, int Score) LoadGoals(string fileName)
    {
        List<Goal> loadedGoals = new List<Goal>();
        int loadedScore = 0;

        using (StreamReader reader = new StreamReader(fileName))
        {
            string scoreLine = reader.ReadLine();
            if (scoreLine != null && scoreLine.StartsWith("Score: "))
            {
                loadedScore = int.Parse(scoreLine.Substring(7));
            }

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Goal goal = Goal.ParseSaveString(line);
                if (goal != null)
                {
                    loadedGoals.Add(goal);
                }
                else
                {
                    Console.WriteLine("Failed to parse goal. Skipping line.");
                }
            }
        }

        return (loadedGoals, loadedScore);
    }
}

public abstract class Goal
{
    protected string _name;
    protected string _description;
    protected int _points;
    protected bool _isComplete;

    public Goal(string name, string description, int points, bool isComplete)
    {
        _name = name;
        _description = description;
        _points = points;
        _isComplete = isComplete;
    }

    public string GetName() => _name;

    public string GetDescription() => _description;

    public int GetPoints() => _points;

    public bool IsComplete() => _isComplete;

    public abstract void RecordEvent(GoalManager goalManager);

    public virtual string GetDetailsString() => $"{_name} {(_isComplete ? "[X]" : "[ ]")} ({_description})";

    public abstract string GetSaveString();

    public static Goal ParseSaveString(string saveString)
    {
        string[] parts = saveString.Split(',');
        if (parts.Length >= 5)
        {
            string typeName = parts[0];
            string name = parts[1];
            string description = parts[2];
            int points;
            if (int.TryParse(parts[3], out points))
            {
                bool isComplete = parts[4] == "1";
                switch (typeName)
                {
                    case nameof(SimpleGoal):
                        return new SimpleGoal(name, description, points, isComplete);
                    case nameof(ChecklistGoal):
                        if (parts.Length >= 8)
                        {
                            int timesCompleted, target, bonus;
                            if (int.TryParse(parts[5], out timesCompleted) &&
                                int.TryParse(parts[6], out target) &&
                                int.TryParse(parts[7], out bonus))
                            {
                                return new ChecklistGoal(name, description, points, target, isComplete, timesCompleted, bonus);
                            }
                        }
                        break;
                    case nameof(EternalGoal):
                        return new EternalGoal(name, description, points, isComplete);
                    case nameof(NegativeGoal):
                        if (parts.Length >= 6)
                        {
                            int penalty;
                            if (int.TryParse(parts[5], out penalty))
                            {
                                return new NegativeGoal(name, description, points, penalty);
                            }
                        }
                        break;
                }
            }
        }
        return null;
    }
}

public class SimpleGoal : Goal
{
    public SimpleGoal(string name, string description, int points, bool isComplete)
        : base(name, description, points, isComplete)
    {
    }

    public override void RecordEvent(GoalManager goalManager)
    {
        _isComplete = true;
        goalManager.Score += _points; // Add points to the score
    }

    public override string GetSaveString()
    {
        return $"{nameof(SimpleGoal)},{_name},{_description},{_points},{(_isComplete ? 1 : 0)}";
    }
}

public class ChecklistGoal : Goal
{
    private int _timesCompleted;
    public int Target { get; private set; }
    public int Bonus { get; private set; }

    public ChecklistGoal(string name, string description, int points, int target, bool isComplete, int timesCompleted, int bonus)
        : base(name, description, points, isComplete)
    {
        _timesCompleted = timesCompleted;
        Target = target;
        Bonus = bonus;
    }

    public override void RecordEvent(GoalManager goalManager)
    {
        _timesCompleted++;
        goalManager.Score += _points; // Earn points each time an event is recorded
        if (_timesCompleted >= Target)
        {
            _isComplete = true;
            goalManager.Score += Bonus; // Earn bonus points upon completing the goal
        }
    }

    public override string GetDetailsString()
    {
        if (Target > 0)
        {
            return $"{_name} {(_isComplete ? "[X]" : "[ ]")} ({_description}) currently completed: {_timesCompleted}/{Target}";
        }
        else
        {
            return $"{_name} {(_isComplete ? "[X]" : "[ ]")} ({_description})";
        }
    }

    public override string GetSaveString()
    {
        return $"{nameof(ChecklistGoal)},{_name},{_description},{_points},{(_isComplete ? 1 : 0)},{_timesCompleted},{Target},{Bonus}";
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points, bool isComplete)
        : base(name, description, points, isComplete)
    {
    }

    public override void RecordEvent(GoalManager goalManager)
    {
        goalManager.Score += _points; // Add points to the score
    }

    public override string GetSaveString()
    {
        return $"{nameof(EternalGoal)},{_name},{_description},{_points},{(_isComplete ? 1 : 0)}";
    }
}

public class NegativeGoal : Goal
{
    private int _penalty;

    public NegativeGoal(string name, string description, int points, int penalty)
        : base(name, description, points, false) // Negative goals are always incomplete
    {
        _penalty = penalty;
    }

    public override void RecordEvent(GoalManager goalManager)
    {
        // Apply penalty to the score
        goalManager.Score -= _penalty;
    }

    public override string GetDetailsString()
    {
        return $"{_name} - {_description} - Penalty: {_penalty} points";
    }

    public override string GetSaveString()
    {
        return $"{nameof(NegativeGoal)},{_name},{_description},{_points},{(_isComplete ? 1 : 0)},{_penalty}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        GoalManager manager = new GoalManager();
        manager.Start();
    }
}
