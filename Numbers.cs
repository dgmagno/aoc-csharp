using System.Numerics;

namespace aoc
{
    internal static class Numbers
    {
        // LeastCommonMultiple. Adapted from: https://www.geeksforgeeks.org/lcm-of-given-array-elements/
        public static T LeastCommonMultiple<T>(this IEnumerable<T> numbers) where T : INumber<T>
        {
            return numbers.Aggregate((x, y) => x * y / GreatestCommonDivisor(x, y));
        }

        // GreatestCommonDivisor. Adapted from: https://www.geeksforgeeks.org/lcm-of-given-array-elements/
        public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
        {
            if (b == T.Zero)
            {
                return a;
            }

            return GreatestCommonDivisor(b, a % b);
        }
    }
}
