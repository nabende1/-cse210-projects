using System;

class Program
{
    static void Main(string[] args)
    {
        Job job1 = new Job();

        job1._jobTitle = "Software Engineer";
        job1._company = "Proud Tech";
        job1._startYear = 2029;
        job1._endYear = 2024;
        // job1.Display();

        Job job2 = new Job();

        job2._jobTitle = "Software Engineer";
        job2._company = "Microsoft";
        job2._startYear = 2020;
        job2._endYear = 2023;
        // job2.Display();

        Resume myResume = new Resume();
        myResume._name = "Brian Nabende";
        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        myResume.Display();
    }
}