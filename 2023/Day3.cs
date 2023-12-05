using System.Diagnostics;

namespace aoc
{
    internal class Day3 : IPuzzle
    {
        private static string[] SafeInput(string[] input)
        {
            var emptyLine = new string('.', input[0].Length + 2);

            return [emptyLine, .. input.Select(it => $".{it}."), emptyLine];
        }

        private static (int, int) NumberAndSize(string[] input, int i, int j)
        {
            int k;

            for (k = 0; j + k < input[i].Length - 1 && char.IsDigit(input[i][j + k]); k++) ;

            var number = int.Parse(string.Concat(input[i].Skip(j).Take(k)));

            return (number, k);
        }

        private static (int I, int J)[] Edges(int i, int j, int size) => new[]
        {
            (i - 1, j - 1),
            (i - 0, j - 1),
            (i + 1, j - 1),
            (i - 1, j + size),
            (i + 0, j + size),
            (i + 1, j + size),
        };

        public object Part1(string[] unsafeInput)
        {
            var input = SafeInput(unsafeInput);

            static bool IsPartNumber(string[] input, int i, int j, int size)
            {
                var isPartNumber = Edges(i, j, size).Any(it => input[it.I][it.J] != '.');

                for (int s = 0; !isPartNumber && s < size; s++)
                {
                    isPartNumber = isPartNumber || input[i - 1][j + s] != '.' || input[i + 1][j + s] != '.';
                }

                return isPartNumber;
            }

            var sum = 0;

            for (int i = 1; i < input.Length - 1; i++)
            {
                for (int j = 1; j < input[i].Length - 1; j++)
                {
                    var value = input[i][j];

                    if (char.IsDigit(value))
                    {
                        var (number, size) = NumberAndSize(input, i, j);

                        var isPartNumber = IsPartNumber(input, i, j, size);

                        if (isPartNumber)
                        {
                            sum += number;
                        }

                        j += size;
                    }
                }
            }

            return sum;
        }

        public object Part2(string[] unsafeInput)
        {
            var input = SafeInput(unsafeInput);

            static IEnumerable<(int, int)> GearPositions(string[] input, int i, int j, int size)
            {
                foreach (var edge in Edges(i, j, size))
                {
                    if (input[edge.I][edge.J] == '*')
                    {
                        yield return edge;
                    }
                }

                for (int s = 0; s < size; s++)
                {
                    if (input[i - 1][j + s] == '*')
                    {
                        yield return (i - 1, j + s);
                    }

                    if (input[i + 1][j + s] == '*')
                    {
                        yield return (i + 1, j + s);
                    }
                }
            }

            var possibleGears = new Dictionary<(int, int), List<int>>();

            for (int i = 1; i < input.Length - 1; i++)
            {
                for (int j = 1; j < input[i].Length - 1; j++)
                {
                    var value = input[i][j];

                    if (char.IsDigit(value))
                    {
                        var (number, size) = NumberAndSize(input, i, j);

                        foreach (var position in GearPositions(input, i, j, size))
                        {
                            possibleGears.TryGetValue(position, out var numbers);
                            possibleGears[position] = numbers ?? [];
                            possibleGears[position].Add(number);
                        }

                        j += size;
                    }
                }
            }

            return possibleGears
                .Where(it => it.Value.Count == 2)
                .Select(it => it.Value.First() * it.Value.Last())
                .Sum();
        }

        public void Example()
        {
            var input = 
                """"
                467..114..
                ...*......
                ..35..633.
                ......#...
                617*......
                .....+.58.
                ..592.....
                ......755.
                ...$.*....
                .664.598..
                """"
                .Split(Environment.NewLine);

            input = SafeInput(input);

            Debug.Assert(Equals(Part1(input), 4361));
            Debug.Assert(Equals(Part2(input), 467835));
        }
    }
}
