using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter your grade percentage: ");
        string Input = Console.ReadLine();
        int percentage = int.Parse(Input);
        String letter ="";

        if (percentage >= 90){
            letter = "A";
        }
        else if (percentage >= 80){
            letter = "B";
        }
        else if (percentage >= 70){
            letter = "C";
        }
        else if (percentage >= 60){
            letter = "D";
        }
        else{
            letter = "F";
        }
        Console.WriteLine($"Your grade is {letter}");
        if (percentage >=70){
            Console.WriteLine("Congratulations, You have passed this course!");
        }
        else{
            Console.WriteLine("Try again next semester!");
        }
    }
}