using System.Numerics;

namespace AdventOfCode
{
    internal static class ValueTupleExtensions
    {
        public static (T1, T2) Plus<T1, T2>(this (T1, T2) a, (T1, T2) b)
            where T1 : IAdditionOperators<T1, T1, T1>
            where T2 : IAdditionOperators<T2, T2, T2>
            => (a.Item1 + b.Item1, a.Item2 + b.Item2);

        public static (T1, T2) Minus<T1, T2>(this (T1, T2) a, (T1, T2) b)
            where T1 : ISubtractionOperators<T1, T1, T1>
            where T2 : ISubtractionOperators<T2, T2, T2>
            => (a.Item1 - b.Item1, a.Item2 - b.Item2);

        /// <summary>
        /// Clockwise from North
        /// </summary>
        public static IEnumerable<(T1, T2)> GetCardinalNeighbors<T1, T2>(this (T1, T2) pt)
            where T1 : IIncrementOperators<T1>, IDecrementOperators<T1>
            where T2 : IIncrementOperators<T2>, IDecrementOperators<T2>
        {
            yield return (++pt.Item1, pt.Item2);    // North
            yield return (--pt.Item1, ++pt.Item2);  // East
            yield return (--pt.Item1, --pt.Item2);  // South
            yield return (++pt.Item1, --pt.Item2);  // West
        }

        /// <summary>
        /// Clockwise from North
        /// </summary>
        public static IEnumerable<(T1, T2)> GetAllNeighbors<T1, T2>(this (T1, T2) pt)
            where T1 : IIncrementOperators<T1>, IDecrementOperators<T1>
            where T2 : IIncrementOperators<T2>, IDecrementOperators<T2>
        {
            yield return (++pt.Item1, pt.Item2);    // North
            yield return (pt.Item1, ++pt.Item2);    // Northeast
            yield return (--pt.Item1, pt.Item2);    // East
            yield return (--pt.Item1, pt.Item2);    // Southeast
            yield return (pt.Item1, --pt.Item2);    // South
            yield return (pt.Item1, --pt.Item2);    // Southwest
            yield return (++pt.Item1, pt.Item2);    // West
            yield return (++pt.Item1, pt.Item2);    // Northwest
        }

        // <summary>
        /// Clockwise from Northeast
        /// </summary>
        public static IEnumerable<(T1, T2)> GetIntercardinalNeighbors<T1, T2>(this (T1, T2) pt)
            where T1 : IIncrementOperators<T1>, IDecrementOperators<T1>
            where T2 : IIncrementOperators<T2>, IDecrementOperators<T2>
        {
            yield return (++pt.Item1, ++pt.Item2);  // Northeast
            --pt.Item1;
            yield return (--pt.Item1, pt.Item2);    // Southeast
            --pt.Item2;
            yield return (pt.Item1, --pt.Item2);    // Southwest
            ++pt.Item1;
            yield return (++pt.Item1, pt.Item2);    // Northwest
        }

        /// <summary>
        /// Clockwise from North
        /// </summary>
        public static IEnumerable<(T1, T2)> GetCardinalDirections<T1, T2>()
            where T1 : IIncrementOperators<T1>, IDecrementOperators<T1>, IAdditiveIdentity<T1, T1>
            where T2 : IIncrementOperators<T2>, IDecrementOperators<T2>, IAdditiveIdentity<T2, T2>
            => (T1.AdditiveIdentity, T2.AdditiveIdentity).GetCardinalNeighbors();

        /// <summary>
        /// Clockwise from North
        /// </summary>
        public static IEnumerable<(T1, T2)> GetAllDirections<T1, T2>()
            where T1 : IIncrementOperators<T1>, IDecrementOperators<T1>, IAdditiveIdentity<T1, T1>
            where T2 : IIncrementOperators<T2>, IDecrementOperators<T2>, IAdditiveIdentity<T2, T2>
            => (T1.AdditiveIdentity, T2.AdditiveIdentity).GetAllNeighbors();

        /// <summary>
        /// Clockwise from Northeast
        /// </summary>
        public static IEnumerable<(T1, T2)> GetIntercardinalDirections<T1, T2>()
            where T1 : IIncrementOperators<T1>, IDecrementOperators<T1>, IAdditiveIdentity<T1, T1>
            where T2 : IIncrementOperators<T2>, IDecrementOperators<T2>, IAdditiveIdentity<T2, T2>
            => (T1.AdditiveIdentity, T2.AdditiveIdentity).GetIntercardinalNeighbors();

        public static bool LessThan<T1, T2>(this (T1, T2) pt1, (T1, T2) pt2)
            where T1 : IComparisonOperators<T1, T1, bool>
            where T2 : IComparisonOperators<T2, T2, bool>
        {
            return pt1.Item1 < pt2.Item1 && pt1.Item2 < pt2.Item2;
        }

        public static bool LessThanOrEqualTo<T1, T2>(this (T1, T2) pt1, (T1, T2) pt2)
            where T1 : IComparisonOperators<T1, T1, bool>
            where T2 : IComparisonOperators<T2, T2, bool>
        {
            return pt1.Item1 <= pt2.Item1 && pt1.Item2 <= pt2.Item2;
        }

        public static bool GreaterThan<T1, T2>(this (T1, T2) pt1, (T1, T2) pt2)
            where T1 : IComparisonOperators<T1, T1, bool>
            where T2 : IComparisonOperators<T2, T2, bool>
        {
            return pt1.Item1 > pt2.Item1 && pt1.Item2 > pt2.Item2;
        }

        public static bool GreaterThanOrEqualTo<T1, T2>(this (T1, T2) pt1, (T1, T2) pt2)
            where T1 : IComparisonOperators<T1, T1, bool>
            where T2 : IComparisonOperators<T2, T2, bool>
        {
            return pt1.Item1 >= pt2.Item1 && pt1.Item2 >= pt2.Item2;
        }
    }

    static class Direction
    {
        public static (int, int) North = (-1, 0);
        public static (int, int) South = (1, 0);
        public static (int, int) West = (0, -1);
        public static (int, int) East = (0, 1);

        public static (int, int) Left = West;
        public static (int, int) Right = East;
        public static (int, int) Down = South;
        public static (int, int) Up = North;

        public static (int, int) TurnRight(this (int, int) dir)
            => (dir.Item2, -dir.Item1);

        public static (int, int) TurnLeft(this (int, int) dir)
            => (-dir.Item2, dir.Item1);
    }
}
