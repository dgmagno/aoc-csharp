using System.Diagnostics;

namespace aoc
{
    internal class Day1 : IPuzzle
    {
        static readonly Dictionary<string, string> writtenDigits = new()
        {
            ["on"] = "one",
            ["tw"] = "two",
            ["th"] = "three",
            ["fo"] = "four",
            ["fi"] = "five",
            ["si"] = "six",
            ["se"] = "seven",
            ["ei"] = "eight",
            ["ni"] = "nine",
            ["one"] = "1",
            ["two"] = "2",
            ["three"] = "3",
            ["four"] = "4",
            ["five"] = "5",
            ["six"] = "6",
            ["seven"] = "7",
            ["eight"] = "8",
            ["nine"] = "9",
        };

        public object Part1(string[] input)
        {
            return input
                .Select(it => it.Where(c => char.IsDigit(c)))
                .Select(it => int.Parse($"{it.First()}{it.Last()}"))
                .Sum();
        }

        public object Part2(string[] input)
        {
            return input
                .Select(FirstAndLastDigits)
                .Sum();
        }

        private static int FirstAndLastDigits(string line)
        {
            var digits = "";

            for (var i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    digits += line[i];
                    continue;
                }

                var key = "";

                do
                {
                    var keySize = Math.Max(2, key.Length);
                    key = string.Concat(line.Skip(i).Take(keySize));
                }
                while (writtenDigits.TryGetValue(key, out key) && key.Length > 1 && i + key.Length <= line.Length);

                if (int.TryParse(key, out var digit))
                {
                    digits += digit;
                }
            }

            return int.Parse($"{digits.First()}{digits.Last()}");
        }

        public void Example()
        {
            var example1 = 
                """
                1abc2
                pqr3stu8vwx
                a1b2c3d4e5f
                treb7uchet
                """
                .Split(Environment.NewLine);

            var example2 =
                """
                two1nine
                eightwothree
                abcone2threexyz
                xtwone3four
                4nineeightseven2
                zoneight234
                7pqrstsixteen
                """
                .Split(Environment.NewLine);

            Debug.Assert(Equals(Part1(example1), 142));
            Debug.Assert(Equals(Part2(example2), 281));
        }
    }
}
