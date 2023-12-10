using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc
{
    public partial class Day8 : IPuzzle
    {
        private static readonly Dictionary<char, long> instructionValues = new()
        {
            ['L'] = 0,
            ['R'] = 1,
        };

        public object Part1(string[] input)
        {
            var instructions = input[0];

            var network = ParseNetwork(input);

            return Navigate(network, instructions, start: "AAA", stopFunction: element => element == "ZZZ");
        }

        public object Part2(string[] input)
        {
            var instructions = input[0];

            var network = ParseNetwork(input);

            return GhostNavigate(network, instructions);
        }

        private long Navigate(
            Dictionary<string, string[]> network, string instructions, string start, Func<string, bool> stopFunction)
        {
            var element = start;

            var i = 0;

            while (!stopFunction(element))
            {
                var instruction = instructions[i % instructions.Length];

                element = network[element][instructionValues[instruction]];

                i++;
            }

            return i;
        }

        private long GhostNavigate(Dictionary<string, string[]> network, string instructions)
        {
            var elements = network.Keys.Where(it => it.EndsWith('A')).ToArray();

            return elements
                .Select(element => Navigate(network, instructions, start: element, stopFunction: element => element.EndsWith('Z')))
                .LeastCommonMultiple();
        }

        private Dictionary<string, string[]> ParseNetwork(string[] input)
        {
            var lookup = new Dictionary<string, string[]>();

            foreach (var line in input.Skip(2))
            {
                var match = InputRegex().Matches(line);

                lookup[match[0].Groups[1].Value] = [match[0].Groups[2].Value, match[0].Groups[3].Value];

            }

            return lookup;
        }

        public void Example()
        {
            var example1 =
                """
                RL

                AAA = (BBB, CCC)
                BBB = (DDD, EEE)
                CCC = (ZZZ, GGG)
                DDD = (DDD, DDD)
                EEE = (EEE, EEE)
                GGG = (GGG, GGG)
                ZZZ = (ZZZ, ZZZ)
                """
                .Split(Environment.NewLine);

            var example2 =
                """
                LLR

                AAA = (BBB, BBB)
                BBB = (AAA, ZZZ)
                ZZZ = (ZZZ, ZZZ)
                """
                .Split(Environment.NewLine);

            var example3 =
                """"
                LR

                11A = (11B, XXX)
                11B = (XXX, 11Z)
                11Z = (11B, XXX)
                22A = (22B, XXX)
                22B = (22C, 22C)
                22C = (22Z, 22Z)
                22Z = (22B, 22B)
                XXX = (XXX, XXX)
                """"
                .Split(Environment.NewLine);

            Debug.Assert(Equals(Part1(example1), 2L));
            Debug.Assert(Equals(Part1(example2), 6L));
            Debug.Assert(Equals(Part2(example3), 6L));
        }

        [GeneratedRegex("(\\w{3}) = \\((\\w{3}), (\\w{3})\\)", RegexOptions.Compiled)]
        private static partial Regex InputRegex();
    }
}
