using System.Diagnostics;

namespace aoc
{
    public class Day5 : IPuzzle
    {
        record Map(long Source, long Destination, long Count)
        {
            public bool InSourceRange(long sourceValue) => Source <= sourceValue && sourceValue < Source + Count;
            public bool InDestinationRange(long destinationValue) => Destination <= destinationValue && destinationValue < Destination + Count;

            public long GetDestination(long sourceValue) => sourceValue - Source + Destination;
            public long GetSource(long destinationValue) => destinationValue - Destination + Source;
        }

        record SeedRange(long Lower, long Upper)
        {
            public bool InRange(long value) => Lower <= value && value < Lower + Upper;
        }

        public object Part1(string[] input)
        {
            var seeds = ParseSeeds(input);
            var groupedMaps = ParseMaps(input);

            return seeds.Min(seed => FindLocation(groupedMaps, seed));
        }

        private static long[] ParseSeeds(string[] input)
        {
            return input[0].Replace("seeds: ", "").Split(" ").Select(long.Parse).ToArray();
        }

        private static Map[][] ParseMaps(string[] input)
        {
            static Map ToMap(string line)
            {
                var numbers = line.Split(" ").Select(long.Parse).ToArray();

                return new Map(
                    Source: numbers[1],
                    Destination: numbers[0],
                    Count: numbers[2]);
            }

            var lists = new List<Map[]>();

            var maps = new List<Map>();

            foreach (var line in input.Skip(3).Where(it => it != ""))
            {
                if (char.IsDigit(line[0]))
                {
                    maps.Add(ToMap(line));
                }
                else
                {
                    lists.Add([.. maps]);
                    maps = [];
                }
            }

            lists.Add([.. maps]);
            
            return [.. lists];
        }

        private static long FindLocation(IEnumerable<IEnumerable<Map>> groupedMaps, long seed)
        {
            var current = seed;

            foreach (var maps in groupedMaps)
            {
                var map = maps.Where(it => it.InSourceRange(current)).SingleOrDefault();

                current = map is null ? current : map.GetDestination(current);
            }

            return current;
        }

        public object Part2(string[] input)
        {
            var seeds = ParseSeeds(input);
            var groupedMaps = ParseMaps(input);
            var seedRanges = CreateSeedRanges(seeds);

            var seed = FindSeed(groupedMaps, seedRanges);

            return FindLocation(groupedMaps, seed);
        }

        static List<SeedRange> CreateSeedRanges(long[] seeds)
        {
            var seedRanges = new List<SeedRange>();

            for (int i = 0; i < seeds.Length - 1; i += 2)
            {
                seedRanges.Add(new SeedRange(seeds[i], seeds[i + 1]));
            }

            return seedRanges;
        }

        private static long FindSeed(IEnumerable<IEnumerable<Map>> groupedMaps, IEnumerable<SeedRange> seedRanges)
        {
            for (var i = 0L; ; i++)
            {
                var current = i;

                foreach (var maps in groupedMaps.Reverse())
                {
                    var map = maps.FirstOrDefault(it => it.InDestinationRange(current));

                    current = map is null ? current : map.GetSource(current);
                }

                if (seedRanges.Any(it => it.InRange(current)))
                {
                    return current;
                }
            }
        }

        public void Example()
        {
            var example =
                """
                seeds: 79 14 55 13

                seed-to-soil map:
                50 98 2
                52 50 48

                soil-to-fertilizer map:
                0 15 37
                37 52 2
                39 0 15

                fertilizer-to-water map:
                49 53 8
                0 11 42
                42 0 7
                57 7 4

                water-to-light map:
                88 18 7
                18 25 70

                light-to-temperature map:
                45 77 23
                81 45 19
                68 64 13

                temperature-to-humidity map:
                0 69 1
                1 0 69

                humidity-to-location map:
                60 56 37
                56 93 4
                """
                .Split(Environment.NewLine);

            Debug.Assert(Equals(Part1(example), 35L));
            Debug.Assert(Equals(Part2(example), 46L));
        }
    }
}
