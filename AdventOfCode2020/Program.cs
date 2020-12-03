using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Advent Of Code 2020");

            Day1First();
            Day1Second();

            Day2First();
            Day2Second();

            Console.ReadKey();
        }


        static void Day1First()
        {
            string[] numbers = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day1\day1-1.txt").ToArray();
            var number1 = 0;
            var number2 = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int z = 0; z < numbers.Length; z++)
                {
                        if (int.Parse(numbers[i]) + int.Parse(numbers[z]) == 2020)
                        {
                            number1 = int.Parse(numbers[i]);
                            number2 = int.Parse(numbers[z]);
                            break;
                        }
                    if (number1 > 0 && number2 > 0) break;
                }
            }
            var total = number1 * number2;
            Console.WriteLine($"Numbers are : {number1} {number2}");
            Console.WriteLine($"Total is : {total}");
        }
        static void Day1Second()
        {
            string[] numbers = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day1\day1-1.txt").ToArray();
            var number1 = 0;
            var number2 = 0;
            var number3 = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int z = 0; z < numbers.Length; z++)
                {
                    for (int y = 0; y < numbers.Length; y++)
                    {
                        if (int.Parse(numbers[i]) + int.Parse(numbers[y]) + int.Parse(numbers[z]) == 2020)
                        {
                            number1 = int.Parse(numbers[i]);
                            number2 = int.Parse(numbers[y]);
                            number3 = int.Parse(numbers[z]);
                            break;
                        }
                    }
                    if (number1 > 0 && number2 > 0 && number3 > 0) break;
                }
            }
            var total = number1 * number2 * number3;
            Console.WriteLine($"Numbers are : {number1} {number2} {number3}");
            Console.WriteLine($"Total is : {total}");
        }
        static void Day2First()
        {
            string[] pwds = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day2\Day2-1.txt").ToArray();
            var validPwds = new List<String>();
            foreach (var pwdLine in pwds)
            {
                var min = int.Parse(pwdLine.Split('-')[0]);
                var max = int.Parse(pwdLine.Split('-')[1].Split(' ')[0]);
                var letter = pwdLine.Split(' ')[1].Split(':')[0];
                var pwdLetters = pwdLine.Split(' ')[2];
                var grps = pwdLetters.ToCharArray().GroupBy(i => i);
                if(grps.Count(grp => grp.Key.ToString() == letter && grp.Count() >= min && grp.Count() <= max)>0) validPwds.Add(pwdLetters);
            }
            Console.WriteLine("Valids pwds count: "+validPwds.Count());
        }
        static void Day2Second()
        {
            string[] pwds = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day2\Day2-1.txt").ToArray();
            var validPwds = new List<String>();
            foreach (var pwdLine in pwds)
            {
                var min = int.Parse(pwdLine.Split('-')[0]);
                var max = int.Parse(pwdLine.Split('-')[1].Split(' ')[0]);
                var letter = pwdLine.Split(' ')[1].Split(':')[0];
                var pwd = pwdLine.Split(' ')[2];
                var letters = pwd.ToCharArray();
                if ((letters[min-1].ToString() == letter && letters[max-1].ToString() != letter) ||
                    (letters[min - 1].ToString() != letter && letters[max - 1].ToString() == letter)) validPwds.Add(pwd);
            }
            Console.WriteLine("Valids pwds count: " + validPwds.Count());
        }
    }
}
