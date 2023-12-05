using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc
{
    internal partial class Day4 : IPuzzle
    {
        record Card(int Id, int TotalWinning);

        public object Part1(string[] input)
        {
             var cards = input.Select(Parse).ToList();

            return cards
                .Select(it => it.TotalWinning - 1)
                .Where(it => it >= 0)
                .Sum(it => Math.Pow(2, it));
        }

        public object Part2(string[] input)
        {
            var cards = input.Select(Parse).ToList();

            var totals = cards.ToDictionary(it => it.Id, it => 1);

            foreach (var card in cards)
            {
                var multiplier = totals[card.Id];

                for (int i = 1; i <= card.TotalWinning; i++)
                {
                    totals[card.Id + i] += multiplier;
                }
            }

            return totals.Values.Sum();
        }

        public void Example()
        {
            var input = 
                """"
                Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
                Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
                Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
                Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
                Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
                Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
                """"
                .Split(Environment.NewLine);


            Debug.Assert(Equals(Part1(input), 13D));
            Debug.Assert(Equals(Part2(input), 30));
        }

        private static Card Parse(string line)
        {
            var regex = DigitsRegex();

            var splitted = line.Split(new[] { ":", "|" }, StringSplitOptions.RemoveEmptyEntries);

            var id = int.Parse(regex.Match(splitted[0]).Groups[1].Value);

            var winning = regex.Matches(splitted[1]).Select(it => int.Parse(it.Value)).ToHashSet();
            var numbers = regex.Matches(splitted[2]).Select(it => int.Parse(it.Value)).ToHashSet();

            return new Card(id, winning.Intersect(numbers).Count());
        }

        [GeneratedRegex("(\\d+)+", RegexOptions.Compiled)]
        private static partial Regex DigitsRegex();
    }
}
