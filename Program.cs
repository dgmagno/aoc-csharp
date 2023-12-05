using aoc;
using System.Diagnostics;

//TODO: improve this

var day = args.Take(1).FirstOrDefault() ?? DateTime.Now.Day.ToString();

var input = File.ReadAllLines($"data/2023/{day}.txt");

var puzzle = (IPuzzle)Activator.CreateInstance(Type.GetType($"aoc.Day{day}")!)!;

puzzle.Example();

var timer = Stopwatch.StartNew();

var part1 = puzzle.Part1(input);
Console.WriteLine($"[{timer.Elapsed}] Part 1: {part1}");

timer.Restart();

var part2 = puzzle.Part2(input);
Console.Write($"[{timer.Elapsed}] Part 2: {part2}");
