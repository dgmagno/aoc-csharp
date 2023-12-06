using System.Diagnostics;

namespace aoc
{
    public class Day6 : IPuzzle
    {
        private static readonly string[] emptySpaceSeparator = [" "];

        record Race(long Time, long Distance);

        public object Part1(string[] input)
        {
            var races = ParseRaces(input);

            return RacesResult(races);
        }

        private static int RacesResult(Race[] races)
        {
            var result = 1;

            foreach (var race in races)
            {
                var button = 0;

                var wins = 0;

                for (var i = 0; i <= race.Time; i++)
                {
                    if ((race.Time - i) * button * 1L > race.Distance)
                    {
                        wins++;
                    }

                    button++;
                }

                result *= wins;
            }

            return result;
        }

        public object Part2(string[] input)
        {
            var monsterRaces = ParseMonsterRaces(input);

            return RacesResult(monsterRaces);
        }

        private static Race[] ParseRaces(string[] input)
        {
            var distances = input[1]
                .Split(emptySpaceSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(long.Parse)
                .ToArray();

            return input[0]
                .Split(emptySpaceSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(long.Parse)
                .Select((it, i) => new Race(it, distances[i]))
                .ToArray();
        }

        private static Race[] ParseMonsterRaces(string[] input)
        {
            var time = int.Parse(string.Concat(input[0].Where(char.IsDigit)));
            var distance = long.Parse(string.Concat(input[1].Where(char.IsDigit)));

            return [new Race(time, distance)];
        }

        public void Example()
        {
            var example =
                """
                Time:      7  15   30
                Distance:  9  40  200
                """
                .Split(Environment.NewLine);

            Debug.Assert(Equals(Part1(example), 288));
            Debug.Assert(Equals(Part2(example), 71503));
        }
    }
}
