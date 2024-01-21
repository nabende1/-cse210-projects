using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        int number = randomGenerator.Next(1, 100);
        int guess = 0;
        
        do
            {
                Console.WriteLine("What is your guess? ");
                string input = Console.ReadLine();
                guess = int.Parse(input);

                if (guess > number){
                    Console.WriteLine("Lower");
                }
                else if (guess < number){
                    Console.WriteLine("Higher");
                }
                else{
                    Console.WriteLine("You guessed it!");
                }
            } while (number != guess);
    }
}