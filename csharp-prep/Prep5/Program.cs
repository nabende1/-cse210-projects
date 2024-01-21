using System;

class Program
{
    static void Main(string[] args)
    {
        static void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the program!");
        }


        static string PromptUserName()
        {
            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();
            return userName;
        }
        static int PromptUserNumber()
        {
            Console.Write("Please enter your favorite number: ");
            string fav = Console.ReadLine();
            int    favInt = int.Parse(fav);
            return favInt;
        }
        static int SquareNumber(int number)
        {
            int Square = number * number;
            return Square;
        }
        static void DisplayResult(string name, int number)
        {
            Console.WriteLine($"{name}, the square of your number is {number}");
        }
        DisplayWelcome();
        string name = PromptUserName();
        int square = SquareNumber(PromptUserNumber());
        DisplayResult(name,square);
    }
}