using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Year2017;

internal static class Day6
{
    [AnswerMethod(2017, 6, 1)]
    public static string Part1(string input)
    {
        var nums = input.Split('\t').Select(int.Parse).ToArray();

        var set = new HashSet<int[]>(new IntArrayComparer());
        set.Add(nums);

        while (set.Add(nums = Diffuse(nums))) ;

        return set.Count.ToString();

        static int[] Diffuse(int[] orig)
        {
            var ret = (int[])orig.Clone();

            var maxIdx = 0;

            for (int i = 1; i < ret.Length; i++)
            {
                if (ret[i] > ret[maxIdx])
                {
                    maxIdx = i;
                }
            }

            var max = ret[maxIdx];
            ret[maxIdx] = 0;

            for (var i = 1; i <= max; i++)
            {
                ret[(maxIdx + i) % ret.Length]++;
            }

            return ret;
        }
    }

    [AnswerMethod(2017, 6, 2)]
    public static string Part2(string input)
    {
        var nums = input.Split('\t').Select(int.Parse).ToArray();

        var set = new Dictionary<int[], int>(new IntArrayComparer());
        int cnt = 0;
        set.Add(nums, cnt);

        while (true)
        {
            cnt++;
            nums = Diffuse(nums);

            if (!set.TryAdd(nums, cnt))
            {
                return (cnt - set[nums]).ToString();
            }
        }

        static int[] Diffuse(int[] orig)
        {
            var ret = (int[])orig.Clone();

            var maxIdx = 0;

            for (int i = 1; i < ret.Length; i++)
            {
                if (ret[i] > ret[maxIdx])
                {
                    maxIdx = i;
                }
            }

            var max = ret[maxIdx];
            ret[maxIdx] = 0;

            for (var i = 1; i <= max; i++)
            {
                ret[(maxIdx + i) % ret.Length]++;
            }

            return ret;
        }
    }

    class IntArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? x, int[]? y)
        {
            if (x is null && y is null) return true;

            if (x is null || y is null) return false;

            if (x.Length != y.Length) return false;

            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i]) return false;
            }

            return true;
        }

        public int GetHashCode([DisallowNull] int[] obj)
        {
            var hc = 0;

            foreach (var i in obj)
            {
                hc = HashCode.Combine(hc, i);
            }

            return hc;
        }
    }
}
