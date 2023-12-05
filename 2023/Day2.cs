using System.Diagnostics;

namespace aoc
{
    // quick and dirty solution. I may improve this later
    internal class Day2 : IPuzzle
    {
        public object Part1(string[] input)
        {
            var sum = 0;

            foreach (var line in input)
            {
                var split = line.Split(":");

                var id = int.Parse(split[0].Replace("Game ", ""));

                var possible = true;

                foreach (var ballSet in split[1].Split(";"))
                {
                    var balls = ballSet.Split(",");

                    var r = 0;
                    var g = 0;
                    var b = 0;

                    foreach (var ball in balls)
                    {
                        if (ball.Contains("blue"))
                        {
                            b += int.Parse(ball.Replace(" blue", ""));
                        }
                        if (ball.Contains("red"))
                        {
                            r += int.Parse(ball.Replace(" red", ""));
                        }
                        if (ball.Contains("green"))
                        {
                            g += int.Parse(ball.Replace(" green", ""));
                        }
                    }

                    if (!(r <= 12 && g <= 13 && b <= 14))
                    {
                        possible = false;
                        break;
                    }
                }

                if (possible)
                {
                    sum += id;
                }
            }

            return sum;
        }

        public object Part2(string[] input)
        {
            var sum = 0;

            foreach (var line in input)
            {
                var split = line.Split(":");

                var id = int.Parse(split[0].Replace("Game ", ""));

                var r = 0;
                var g = 0;
                var b = 0;

                foreach (var ballSet in split[1].Split(";"))
                {
                    var balls = ballSet.Split(",");

                    foreach (var ball in balls)
                    {
                        if (ball.Contains("blue"))
                        {
                            b = Math.Max(b, int.Parse(ball.Replace(" blue", "")));
                        }
                        if (ball.Contains("red"))
                        {
                            r = Math.Max(r, int.Parse(ball.Replace(" red", "")));
                        }
                        if (ball.Contains("green"))
                        {
                            g = Math.Max(g, int.Parse(ball.Replace(" green", "")));
                        }
                    }
                }

                sum += r * b * g;
            }

            return sum;
        }

        public void Example()
        {
            var input = 
                """"
                Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
                Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
                Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
                Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
                Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
                """"
                .Split(Environment.NewLine);

            Debug.Assert(Equals(Part1(input), 8));
            Debug.Assert(Equals(Part2(input), 2286));
        }
    }
}
