using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string input ="";
        List<int> numbers = new List<int>();
        Console.WriteLine("Enter a list of numbers, type 0 when finished. ");
        do{
            Console.WriteLine("Enter a number: ");
            input = Console.ReadLine();
            int number = int.Parse(input);
            if (number != 0)
            {
                numbers.Add(number);
            }
        }while(input != "0");
        int sum = 0;
        foreach (int number in numbers)
        {
            sum += number;
        }

        Console.WriteLine($"The sum is: {sum}");

        float average = ((float)sum) / numbers.Count;
        Console.WriteLine($"The average is: {average}");

        int max = numbers[0];

        foreach (int number in numbers)
        {
            if (number > max)
            {
                max = number;
            }
        }

        Console.WriteLine($"The max is: {max}");
    }
}