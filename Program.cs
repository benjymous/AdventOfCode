using System;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            string day01Input = System.IO.File.ReadAllText(@"Data\MMXVIII\day01.txt");
            Console.WriteLine("2018 Day01 Pt1 - " + MMXVIII.Day01.Callibrate01(day01Input));
            Console.WriteLine("2018 Day01 Pt2 - " + MMXVIII.Day01.Callibrate02(day01Input));
        }
    }
}
