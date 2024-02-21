using System;
using System.Collections.Generic;
using System.IO;

class Entry
{
    public string Date { get; }
    public string PromptText { get; set; }
    public string EntryText { get; set; }

    public Entry(string promptText, string entryText)
    {
        Date = DateTime.Now.ToString("yyyy-MM-dd");
        PromptText = promptText;
        EntryText = entryText;
    }

    public virtual void Display()
    {
        Console.WriteLine($"Date: {Date}");
        Console.WriteLine($"Prompt: {PromptText}");
        Console.WriteLine($"Entry: {EntryText}\n");
    }
}

class JournalEntry : Entry
{
    public JournalEntry(string promptText, string entryText) 
        : base(promptText, entryText)
    {
    }

    public override void Display()
    {
        Console.WriteLine($"Date: {Date}");
        Console.WriteLine($"Prompt: {PromptText}");
        Console.WriteLine($"Entry: {EntryText}\n");
    }
}

class Journal
{
    private List<Entry> _entries = new List<Entry>();

    public void AddEntry(string promptText, string entryText)
    {
        _entries.Add(new Entry(promptText, entryText));
    }

    public void DisplayAll()
    {
        foreach (var entry in _entries)
        {
            entry.Display();
        }
    }

    public void SaveToFile(string file)
    {
        using (StreamWriter writer = new StreamWriter(file))
        {
            foreach (var entry in _entries)
            {
                writer.WriteLine($"{entry.Date}|{entry.PromptText}|{entry.EntryText}");
            }
        }
    }

    public void LoadFromFile(string file)
    {
        _entries.Clear(); // Clear existing entries before loading from file
        string line;
        using (StreamReader reader = new StreamReader(file))
        {
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 3)
                {
                    _entries.Add(new Entry(parts[1], parts[2]));
                }
            }
        }
    }
}

class PromptGenerator
{
    private List<string> _prompts = new List<string>();

    public PromptGenerator()
    {
        // Add default prompts
        _prompts.Add("Who was the most interesting person I interacted with today?");
        _prompts.Add("What was the best part of my day?");
        _prompts.Add("How did I see the hand of the Lord in my life today?");
        _prompts.Add("What was the strongest emotion I felt today?");
        _prompts.Add("If I had one thing I could do over today, what would it be?");
    }

    public string GetRandomPrompt()
    {
        Random rnd = new Random();
        int index = rnd.Next(_prompts.Count);
        return _prompts[index];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        PromptGenerator promptGenerator = new PromptGenerator();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    string prompt = promptGenerator.GetRandomPrompt();
                    Console.WriteLine($"Prompt: {prompt}");
                    Console.Write("Enter your response: ");
                    string response = Console.ReadLine();
                    journal.AddEntry(prompt, response);
                    break;
                case "2":
                    Console.WriteLine("Journal Entries:");
                    journal.DisplayAll();
                    break;
                case "3":
                    Console.Write("Enter filename to save: ");
                    string saveFilename = Console.ReadLine();
                    journal.SaveToFile(saveFilename);
                    Console.WriteLine($"Journal saved to {saveFilename}");
                    break;
                case "4":
                    Console.Write("Enter filename to load: ");
                    string loadFilename = Console.ReadLine();
                    journal.LoadFromFile(loadFilename);
                    Console.WriteLine($"Journal loaded from {loadFilename}");
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }
        }
    }
}