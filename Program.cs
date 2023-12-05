using aoc;

//TODO: improve this

var day = args.Take(1).FirstOrDefault() ?? DateTime.Now.Day.ToString();

var input = File.ReadAllLines($"data/2023/{day}.txt");

var puzzle = (IPuzzle)Activator.CreateInstance(Type.GetType($"aoc.Day{day}")!)!;

puzzle.Example();

Console.WriteLine(puzzle.Part1(input));
Console.WriteLine(puzzle.Part2(input));
