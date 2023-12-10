using System.Diagnostics;

namespace aoc
{
    class History : List<long>
    {
        private readonly History? next;

        public History(IReadOnlyList<long> values) : base(values)
        {
            if (values.Any(it => it != 0))
            {
                next = NextSequence(values);
            }
        }

        public long Extrapolate()
        {
            return next is null ? 0 : this.Last() + next.Extrapolate();
        }

        public long ExtrapolateBackwards()
        {
            return next is null ? 0 : this.First() - next.ExtrapolateBackwards();
        }

        public History NextSequence(IReadOnlyList<long> values)
        {
            var differences = new List<long>();

            for (int i = 0; i < Count - 1; i++)
            {
                differences.Add(values[i + 1] - values[i]);
            }

            return new History(differences);
        }
    }

    public class Day9 : IPuzzle
    {
        public object Part1(string[] input)
        {
            var histories = Parse(input);

            return histories.Sum(it => it.Extrapolate());
        }

        public object Part2(string[] input)
        {
            var histories = Parse(input);

            return histories.Sum(it => it.ExtrapolateBackwards());
        }

        private static History[] Parse(string[] input)
        {
            return input
                .Select(it => new History(it.Split(" ").Select(it => long.Parse(it)).ToArray()))
                .ToArray();
        }

        public void Example()
        {
            var example =
                """
                0 3 6 9 12 15
                1 3 6 10 15 21
                10 13 16 21 30 45
                """
                .Split(Environment.NewLine);

            Console.WriteLine(Part2(example));

            Debug.Assert(Equals(Part1(example), 114L));
            Debug.Assert(Equals(Part2(example), 2L));
        }
    }
}
