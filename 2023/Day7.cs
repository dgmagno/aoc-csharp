using System.Diagnostics;

namespace aoc
{
    class Game : IComparer<Hand>
    {
        private readonly Dictionary<char, int> strengths = new()
        {
            ['A'] = 12,
            ['K'] = 11,
            ['Q'] = 10,
            ['J'] = 9,
            ['T'] = 8,
            ['9'] = 7,
            ['8'] = 6,
            ['7'] = 5,
            ['6'] = 4,
            ['5'] = 3,
            ['4'] = 2,
            ['3'] = 1,
            ['2'] = 0,
        };

        private readonly char? joker;
        private readonly Hand[] hands;

        public Game(Hand[] hands, char? joker = null)
        {
            if (joker is not null)
            {
                strengths[joker.Value] = -1;
            }

            this.joker = joker;

            this.hands = [.. hands.Order(this)];
        }

        public int GetStrength(Hand hand, int cardPosition) => strengths[hand.Value[cardPosition]];

        public CamelType GetCamelType(Hand hand)
        {
            var jokers = hand.Value.Count(it => it == joker);

            var groupCounts = hand.Value
                .Where(it => it != joker)
                .GroupBy(it => it)
                .Select(it => it.Count())
                .OrderDescending()
                .ToList();

            var firstCount = groupCounts.FirstOrDefault();
            var length = groupCounts.Count;

            return length switch
            {
                3 => (CamelType)(firstCount + jokers),
                4 or 5 => (CamelType)(firstCount + jokers - 1),
                _ => (CamelType)(firstCount + jokers + 1)
            };
        }

        public int TotalWinnings()
        {
            var result = 0;

            for (int i = 0; i < hands.Length; i++)
            {
                result += hands[i].Bid * (i + 1);
            }

            return result;
        }

        public int Compare(Hand? one, Hand? other)
        {
            one = one ?? throw new Exception();
            other = other ?? throw new Exception();

            if (one == other)
            {
                return 0;
            }

            var result = Comparer<CamelType>.Default.Compare(GetCamelType(one), GetCamelType(other));

            if (result != 0)
            {
                return result;
            }

            var i = 0;
            int oneStrength;
            int otherStrength;

            do
            {
                oneStrength = GetStrength(one, i);
                otherStrength = GetStrength(other, i);
                i++;
            }
            while (oneStrength == otherStrength);

            return oneStrength.CompareTo(otherStrength);
        }
    }

    record Hand(string Value, int Bid);

    enum CamelType
    {
        Five = 6,
        Four = 5,
        FulHouse = 4,
        Three = 3,
        Two = 2,
        One = 1,
        Zero = 0,
    }

    public class Day7 : IPuzzle
    {
        public object Part1(string[] input)
        {
            var hands = Parse(input);

            var game = new Game(hands);

            return game.TotalWinnings();
        }

        public object Part2(string[] input)
        {
            var hands = Parse(input);

            var game = new Game(hands, 'J');

            return game.TotalWinnings();
        }

        private static Hand[] Parse(string[] input)
        {
            return input
                .Select(it =>
                {
                    var splitted = it.Split(" ");

                    return new Hand(splitted[0], int.Parse(splitted[1]));
                })
                .ToArray();
        }


        public void Example()
        {
            var example =
                """
                32T3K 765
                T55J5 684
                KK677 28
                KTJJT 220
                QQQJA 483
                """
                .Split(Environment.NewLine);

            Console.WriteLine(Part2(example));

            Debug.Assert(Equals(Part1(example), 6440));
            Debug.Assert(Equals(Part2(example), 5905));
        }
    }
}
