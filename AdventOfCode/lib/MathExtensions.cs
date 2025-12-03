using System.Numerics;

namespace AdventOfCode.lib;

internal static class MathExtensions
{
    extension<TBase, TExponent>(TBase b)
        where TBase : IMultiplyOperators<TBase, TBase, TBase>, IMultiplicativeIdentity<TBase, TBase>
        where TExponent : IBinaryInteger<TExponent>
    {
        public TBase Pow(TExponent e)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(e);

            TBase ret = TBase.MultiplicativeIdentity;

            TBase basePow = b;

            if ((e & TExponent.One) == TExponent.One)
            {
                ret *= basePow;
            }

            e >>= 1;

            while (e > TExponent.Zero)
            {
                basePow *= basePow;

                if ((e & TExponent.One) == TExponent.One)
                {
                    ret *= basePow;
                }

                e >>= 1;
            }

            return ret;
        }
    }
}
