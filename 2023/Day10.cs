using System.Diagnostics;

namespace aoc
{
    readonly record struct Position(int Y, int X);

    class PipesMatrix(string[] matrix)
    {
        private const char StartPipe = 'S';

        private readonly string[] matrix = matrix;

        private static readonly Dictionary<Position, HashSet<string>> allConnections = new()
        {
            [new(-1,  0)] = ["||", "|7", "|F", "L|", "L7", "LF", "J|", "J7", "JF", "S|", "S7", "SF"],
            [new( 1,  0)] = ["||", "|L", "|J", "7|", "7L", "7J", "F|", "FL", "FJ", "S|", "SL", "SJ"],
            [new( 0, -1)] = ["--", "-F", "-L", "7-", "7F", "7L", "J-", "JF", "JL", "S-", "SF", "SL"],
            [new( 0,  1)] = ["--", "-7", "-J", "F-", "F7", "FJ", "L-", "L7", "LJ", "S-", "S7", "SJ"],
        };

        private IEnumerable<Position> FindNextPositions(Position position, Position? previousPosition)
        {
            foreach (var (direction, connections) in allConnections)
            {
                Position possiblePosition = new(position.Y + direction.Y, position.X + direction.X);

                var move = $"{matrix[position.Y][position.X]}{matrix[possiblePosition.Y][possiblePosition.X]}";
                
                if (connections.Contains(move) && possiblePosition != previousPosition)
                {
                    yield return possiblePosition;
                }
            }
        }

        public int FindFarthestDistance()
        {
            var startPosition = FindStartPosition();

            var distance = 1;

            var nextPositions = FindNextPositions(startPosition, null).ToArray();

            Position[] previousPositions = [startPosition, startPosition];

            while (nextPositions[0] != nextPositions[1])
            {
                Position[] temp = [..nextPositions];

                nextPositions[0] = FindNextPositions(nextPositions[0], previousPositions[0]).Single();
                nextPositions[1] = FindNextPositions(nextPositions[1], previousPositions[1]).Single();

                previousPositions = temp;

                distance++;
            }

            return distance;
        }

        private Position FindStartPosition()
        {
            for (int i = 1; i < matrix.Length - 1; i++)
            {
                for (int j = 1; j < matrix[i].Length - 1; j++)
                {
                    if (matrix[i][j] == StartPipe)
                    {
                        return new Position(i, j);
                    }
                }
            }

            throw new Exception("Start not found");
        }
    }

    internal class Day10 : IPuzzle
    {
        private static string[] SafeInput(string[] input)
        {
            var emptyLine = new string('.', input[0].Length + 2);

            return [emptyLine, .. input.Select(it => $".{it}."), emptyLine];
        }

        public object Part1(string[] unsafeInput)
        {
            var input = SafeInput(unsafeInput);

            var matrix = new PipesMatrix(input);

            return matrix.FindFarthestDistance();
        }

        public object Part2(string[] unsafeInput)
        {
            var input = SafeInput(unsafeInput);

            return 0;
        }

        public void Example()
        {
            var example1 =
                """
                .....
                .S-7.
                .|.|.
                .L-J.
                .....
                """
                .Split(Environment.NewLine);

            var example2 =
                """
                ..F7.
                .FJ|.
                SJ.L7
                |F--J
                LJ...
                """
                .Split(Environment.NewLine);

            Debug.Assert(Equals(Part1(example1), 4));
            Debug.Assert(Equals(Part1(example2), 8));
        }
    }
}
