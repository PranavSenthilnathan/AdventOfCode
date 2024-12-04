using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class ValueTupleExtensions
    {
        public static (T1, T2) Plus<T1, T2>(this (T1, T2) a, (T1, T2) b)
            where T1 : IAdditionOperators<T1, T1, T1>
            where T2 : IAdditionOperators<T2, T2, T2>
            => (a.Item1 + b.Item1, a.Item2 + b.Item2);

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
    }
}
