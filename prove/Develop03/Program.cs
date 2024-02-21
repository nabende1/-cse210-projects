using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private string _originalText;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _originalText = text;
        _words = text.Split(' ').Select(w => new Word(w)).ToList();
    }

    public void HideRandomWords(int numberToHide)
    {
        Random rand = new Random();
        for (int i = 0; i < numberToHide; i++)
        {
            List<Word> visibleWords = _words.Where(w => !w.IsHidden()).ToList();
            if (visibleWords.Count == 0)
                break;

            int index = rand.Next(0, visibleWords.Count);
            visibleWords[index].Hide();
        }
    }

    public string GetDisplayText()
    {
        string displayText = _reference.GetDisplayText() + " ";
        for (int i = 0; i < _words.Count; i++)
        {
            displayText += _words[i].GetDisplayText() + (i == _words.Count - 1 ? "" : " ");
        }
        return displayText;
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(w => w.IsHidden());
    }

    public string OriginalText => _originalText;
}

public class Reference
{
    private string _book;
    private int _chapter;
    private int _verse;
    private int? _endVerse; // Nullable int for verses range

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _verse = verse;
        _endVerse = null;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _verse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        if (_endVerse == null)
            return $"{_book} {_chapter}:{_verse}";
        else
            return $"{_book} {_chapter}:{_verse}-{_endVerse}";
    }
}

public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        return _isHidden ? new string('_', _text.Length) : _text;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Scripture Hider!");
        Console.WriteLine("Press Enter to hide a word, or press Escape to exit.");

        // Create a library of scriptures
        List<Scripture> scriptures = new List<Scripture>();
        scriptures.Add(new Scripture(new Reference("John", 3, 16), "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."));
        scriptures.Add(new Scripture(new Reference("Matthew", 5, 16), "In the same way, let your light shine before others, that they may see your good deeds and glorify your Father in heaven."));
        scriptures.Add(new Scripture(new Reference("Proverbs", 3, 5, 6), "Trust in the LORD with all your heart and lean not on your own understanding; in all your ways submit to him, and he will make your paths straight."));

        Random rand = new Random();

        while (true)
        {
            // Select a random scripture from the library
            Scripture selectedScripture = scriptures[rand.Next(0, scriptures.Count)];

            // Display initial scripture text
            Console.WriteLine(selectedScripture.GetDisplayText());
            string originalText = selectedScripture.OriginalText;

            // Start the loop for hiding words
            while (!selectedScripture.IsCompletelyHidden())
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    // Clear the screen and reset cursor position
                    Console.Clear();
                    Console.WriteLine("Press Enter to hide a word, or press Escape to exit.");
                    Console.WriteLine(); // Add a blank line for aesthetics

                    // Update scripture and display
                    selectedScripture.HideRandomWords(1);
                    Console.WriteLine(selectedScripture.GetDisplayText());

                    // Add a delay for better readability
                    Thread.Sleep(500);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nExiting the program. Goodbye!");
                    return; // Exit the program
                }
            }
        }
    }
}
