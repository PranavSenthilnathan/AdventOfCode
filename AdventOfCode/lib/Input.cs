using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.lib
{
    internal static class Input
    {
        public static Dictionary<(int i, int j), char> CreateGrid(this string[] input)
        {
            var ret = new Dictionary<(int i, int j), char>();
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    ret.Add((i, j), input[i][j]);
                }
            }

            return ret;
        }

        public static ref TValue? GetValueRefOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, out bool exists) where TKey : notnull
            => ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out exists);
    }
}
