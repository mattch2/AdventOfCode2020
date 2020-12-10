using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Advent Of Code 2020");

            //Day1First();
            //Day1Second();

            //Day2First();
            //Day2Second();

            //Day3();
            //Day4();
            //Day5();
            //Day6();
            //Day7();
            //Day8();
            Day9();
            Console.ReadKey();
        }

        private static void Day9()
        {
            string[] lines = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day9\data.txt").ToArray();
            int preambule = 25;
            var Finalnumber = 0;
            for (int i = preambule; i < lines.Length; i++)
            {
                var found = false;
                var number = int.Parse(lines[i]);
                for (int y = i - preambule; y < i; y++)
                {
                    var first = int.Parse(lines[y]);
                    for (int z = i - preambule; z < i; z++)
                    {
                        var second = int.Parse(lines[z]);
                        if (first + second == number && first!=second) found = true;
                    }
                }
                if (!found) {
                    Console.WriteLine($"No matching sum for {number}");
                    Finalnumber = number;
                    break;
                }
            }
            var currentList = new List<BigInteger>();
            BigInteger start = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var number = BigInteger.Parse(lines[i]);
                if (start + number <= Finalnumber)
                {
                    currentList.Add(number);
                    start += number;
                }
                else {
                    i -= currentList.Count - 1;
                    currentList = new List<BigInteger>();
                    start = 0;
                }

                if(start == Finalnumber && currentList.Count>=2) break;
            }
            currentList.Sort();
            Console.WriteLine($"Sum is : {currentList.First() + currentList.Last()}");
        }


        private static void Day8()
        {
            string[] lines = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day8\data.txt").ToArray();
            var instructions = new List<Tuple<string, int, int>>();
            var isFinished = false;
            for (int i = 0; i < lines.Length; i++)
            {
                var instruction = lines[i].Split(' ')[0].Trim();
                var parameter = int.Parse(lines[i].Split(' ')[1].Trim());
                instructions.Add(Tuple.Create(instruction, parameter, i));
            }
            Console.WriteLine($"Counter is:{GetCount(instructions, out isFinished)}");

            for (int i = 0; i < instructions.Count; i++)
            {
                var tempList = new List<Tuple<string, int, int>>(instructions);
                if (instructions[i].Item1 == "jmp") {
                    tempList[i] = Tuple.Create("nop", instructions[i].Item2, instructions[i].Item3);
                }
                else if (instructions[i].Item1 == "nop")
                {
                    tempList[i] = Tuple.Create("jmp", instructions[i].Item2, instructions[i].Item3);
                }
                var temp = GetCount(tempList, out isFinished);
                if(isFinished) Console.WriteLine($"Final Counter is:{temp}");
            }
        }



        private static int GetCount(List<Tuple<string, int, int>> instructions, out bool isFinished) {
            int count = 0;
            int i = 0;
            var visited = new List<Tuple<string, int, int>>();

            while (i< instructions.Count)
            {
                var instruction = instructions[i];
                if (visited.Any(a => a.Item3 == i)) {
                    isFinished = false;
                    return count;
                } 
              
                if (instruction.Item1 == "nop") i++;
                if (instruction.Item1 == "acc")
                {
                    count += instruction.Item2;
                    i++;
                }
                if (instruction.Item1 == "jmp") i += instruction.Item2;
                visited.Add(instruction);
            }
            isFinished = true;
            return count;
        }

        private static void Day7()
        {
            string[] lines = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day7\data.txt").ToArray();
            var allRules = new List<Tuple<string, int, string>>();
            var extractedRultes = new List<Tuple<string, int, string>>();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("bags", "").Replace("bag", "").Replace(".", "").Replace("  ", " ");
                var outerBag = lines[i].Split(new string[] { "contain" }, options: StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                var innerBages = lines[i].Split(new string[] { "contain" }, options: StringSplitOptions.RemoveEmptyEntries)[1].Split(',');
                for (int y = 0; y < innerBages.Length; y++)
                {
                    if (innerBages[y].Trim() == "no other") break;
                    var temp = innerBages[y].Split(new string[] { " " }, options: StringSplitOptions.RemoveEmptyEntries);
                    allRules.Add(Tuple.Create(outerBag, int.Parse(temp[0].Trim()), temp[1].Trim() + " " + temp[2].Trim()));
                }

            }
            var bag = "shiny gold";
            //countMatching(bag,allRules, extractedRultes);
            int count = 0;
            CountRequiredBags(bag, allRules, extractedRultes, ref count, 1);



            Console.WriteLine($"Number Of Bags : {extractedRultes.Count}");
            Console.WriteLine($"Number Of Required Bags : {count}");

        }
        private static void countMatching(String bag, List<Tuple<string, int, string>> allRules, List<Tuple<string, int, string>> extractedRultes)
        {
            for (int i = 0; i < allRules.Count; i++)
            {
                if (allRules[i].Item3 == bag) {
                    if(!extractedRultes.Any(r=>r.Item1== allRules[i].Item1)) extractedRultes.Add(allRules[i]);
                    countMatching(allRules[i].Item1, allRules, extractedRultes);
                } 
            }
        }

        private static void CountRequiredBags(String bag, List<Tuple<string, int, string>> allRules, List<Tuple<string, int, string>> extractedRultes,
            ref int count, int multiplicator)
        {
            for (int i = 0; i < allRules.Count; i++)
            {
                if (allRules[i].Item1 == bag)
                {
                    extractedRultes.Add(allRules[i]);
                    count += multiplicator* allRules[i].Item2;
                    CountRequiredBags(allRules[i].Item3, allRules, extractedRultes, ref count, multiplicator*allRules[i].Item2);
                }
            }
        }

        private static void Day6() {
            string[] lines = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day6\data.txt").ToArray();

            var groups = new List<List<String>>();
            var answers = new List<String>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "") answers.Add(lines[i]);
                else {
                    groups.Add(answers);
                    answers = new List<String>();
                }
            }
            groups.Add(answers);
            var count = 0;
            for (int i = 0; i < groups.Count; i++)
            {
                int member = groups[i].Count;
                string all = String.Empty;
                for (int y = 0; y < groups[i].Count; y++)
                {
                    all+= string.Join("", groups[i][y].ToCharArray().Distinct());
                }
                count+=all.ToCharArray().GroupBy(z => z).Count(g=>g.Count()==member);
            }
        }

        private static void Day5()
        {

            string[] lines = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day5\data.txt").ToArray();
            int total = 0;
            var seat = new List<int>();
            for (int i = 0; i < lines.Length; i++)
            {
                var interval = new List<int>() { 0, 127 };
                var column = new List<int>() { 0, 7 };
                var letters = lines[i].ToCharArray();
                for (int y = 0; y < 7; y++)
                {
                    divideInterval(letters[y],interval);
                }

                for (int y = 7; y < 10; y++)
                {
                    divideInterval(letters[y], column);
                }

                int currentscore = interval[0] * 8 + column[0];
                seat.Add(currentscore);
                if (currentscore > total) total = currentscore;
                Console.WriteLine($"Result : {interval[0]} {column[0]} {currentscore}");
            }
            Console.WriteLine($"Max Seat : {total}");
            
            seat.Sort();
            var begin = seat[0];
            for (int i = 0; i < seat.Count; i++)
            {
                if ((begin + 1) != seat[i + 1]) {
                    Console.WriteLine($"My seat is : {begin + 1}");
                    break;
                }
                begin++;
            }
        }

        private static void divideInterval(Char letter, List<int> interval) {
            var sub = (interval[1] - interval[0]) / 2+1;
            if (letter == 'F') {
                interval[1] -= sub;
            }
            if (letter == 'L')
            {
                interval[1] -= sub;
            }
            if (letter == 'B')
            {
                interval[0] += sub;
            }
            if (letter == 'R')
            {
                interval[0] += sub;
            }

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
        private static void Day3()
        {
            string tree = "#";
            string[] rows = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day3\data.txt").ToArray();
            int columnIndex1 = 0;
            int columnIndex2 = 0;
            int columnIndex3 = 0;
            int columnIndex4 = 0;
            BigInteger treeCount1 = 0;
            BigInteger treeCount2 = 0;
            BigInteger treeCount3 = 0;
            BigInteger treeCount4 = 0;
            BigInteger treeCount5 = 0;
            for (int i = 1; i < rows.Length; i++)
            {
                var positions = rows[i].ToCharArray();
                for (int y = 0; y < 1; y++)
                {
                    columnIndex1 = columnIndex1 + 1 >= positions.Length ? 0 : columnIndex1 + 1;
                }
                for (int y = 0; y < 3; y++)
                {
                    columnIndex2 = columnIndex2 + 1 >= positions.Length ? 0 : columnIndex2 + 1;
                }
                for (int y = 0; y < 5; y++)
                {
                    columnIndex3 = columnIndex3 + 1 >= positions.Length ? 0 : columnIndex3 + 1;
                }
                for (int y = 0; y < 7; y++)
                {
                    columnIndex4 = columnIndex4 + 1 >= positions.Length ? 0 : columnIndex4 + 1;
                }
                if (positions[columnIndex1].ToString() == tree) treeCount1 += 1;
                if (positions[columnIndex2].ToString() == tree) treeCount2 += 1;
                if (positions[columnIndex3].ToString() == tree) treeCount3 += 1;
                if (positions[columnIndex4].ToString() == tree) treeCount4 += 1;
            }
            columnIndex1 = 0;
            int count = 0;
            for (int i = 0; i < rows.Length; i+=2)
            {
                count++;
                var positions = rows[i].ToCharArray();
                if (positions[columnIndex1].ToString() == tree) treeCount5 += 1;
                if (!(columnIndex1 + 1 >= positions.Length))
                {
                    columnIndex1++;
                }
                else 
                {
                    columnIndex1 = 0;
                }
            }
            BigInteger total = treeCount1 * treeCount2 * treeCount3 * treeCount4 * treeCount5;
            Console.WriteLine($"Number of trees = {treeCount1} {treeCount2} {treeCount3} {treeCount4} {treeCount5}, Total : {total}");
        }
        private static void Day4() 
        {
            string[] passports = File.ReadLines(@"C:\Users\Christophe\source\repos\mattch2\AdventOfCode2020\AdventOfCode2020\Data\Day4\data.txt").ToArray();

            String byr = "byr";
            String iyr = "iyr";
            String eyr = "eyr";
            String hgt = "hgt";
            String hcl = "hcl";
            String ecl = "ecl";
            String pid = "pid";
            String cid = "cid";
            int validPwds = 0;
            var currentPwd = String.Empty;
            for (int i = 0; i < passports.Length; i++)
            {
                if (passports[i] != "")
                {
                    currentPwd = currentPwd + " " + passports[i];
                }
                else {
                    var Score = 0;
                    var fields = currentPwd.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (currentPwd.Contains(byr))
                    {
                        var value = fields.First(f => f.Contains(byr)).Split(':')[1];
                        int parsed = 0;
                        if (int.TryParse(value, out parsed) && parsed >= 1920 && parsed <= 2002) Score++;
                    }
                    if (currentPwd.Contains(iyr))
                    {
                        var value = fields.First(f => f.Contains(iyr)).Split(':')[1];
                        int parsed = 0;
                        if (int.TryParse(value, out parsed) && parsed >= 2010 && parsed <= 2020) Score++;
                    }
                    if (currentPwd.Contains(eyr))
                    {
                        var value = fields.First(f => f.Contains(eyr)).Split(':')[1];
                        int parsed = 0;
                        if (int.TryParse(value, out parsed) && parsed >= 2020 && parsed <= 2030) Score++;
                    }
                    if (currentPwd.Contains(hgt))
                    {
                        var value = fields.First(f => f.Contains(hgt)).Split(':')[1];

                        if (value.Contains("in"))
                        {
                            value=value.Replace("in", "");
                            int parsed = 0;
                            if (int.TryParse(value, out parsed) && parsed >= 59 && parsed <= 76) Score++;
                        }
                        if (value.Contains("cm"))
                        {
                            value=value.Replace("cm", "");
                            int parsed = 0;
                            if (int.TryParse(value, out parsed) && parsed >= 150 && parsed <= 193) Score++;
                        }
                    }
                    if (currentPwd.Contains(hcl))
                    {
                        var value = fields.First(f => f.Contains(hcl)).Split(':')[1];
                        if (Regex.IsMatch(value, @"^[#][0-f]{6}$")) Score++;
                    }
                    if (currentPwd.Contains(ecl))
                    {
                        var colors = new List<String>(){ "amb","blu","brn","gry","grn","hzl","oth" };
                        var value = fields.First(f => f.Contains(ecl)).Split(':')[1];
                        if (colors.Contains(value)) Score++;
                    }
                    if (currentPwd.Contains(pid))
                    {
                        var value = fields.First(f => f.Contains(pid)).Split(':')[1];
                        if (Regex.IsMatch(value, @"^[0-9]{9}$")) Score++;
                                            }
                    if (Score == 7) validPwds++;
                    currentPwd = String.Empty;
                    Score = 0;
                }
            }
            Console.WriteLine($"Valid pwds: {validPwds}");
        }

    }
}
