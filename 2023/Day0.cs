using System.Diagnostics;

namespace aoc
{
    internal class Day0 : IPuzzle
    {
       public object Part1(string[] input)
        {
            return 0;
        }

        public object Part2(string[] input)
        {
            return 0;
        }
        
        public void Example()
        {
            var example1 = 
                """

                """
                .Split(Environment.NewLine);

            Debug.Assert(Equals(Part1(example1), 0));
            Debug.Assert(Equals(Part2(example1), 0));
        }
    }
}
