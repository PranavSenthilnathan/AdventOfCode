using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using AdventOfCode.lib;
using BenchmarkDotNet.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode
{
    internal static partial class Day19
    {
        [AnswerMethod(2024, 19, 1)]
        public static string Part1(string[] input)
        {
            var segments = input[0].Split(",", StringSplitOptions.TrimEntries);
            var ans = 0;
            foreach (var str in input.Skip(2))
            {
                if (TryMake(str, segments))
                    ans++;

                static bool TryMake(ReadOnlySpan<char> partial, string[] segments)
                {
                    foreach (var segment in segments)
                    {
                        if (partial.StartsWith(segment))
                        {
                            if (partial.Length == segment.Length)
                                return true;
                            if (TryMake(partial[segment.Length..], segments))
                                return true;
                        }
                    }

                    return false;
                }
            }

            return ans.ToString();
        }

        [AnswerMethod(2024, 19, 2)]
        public static string Part2(string[] input)
        {
            var segments = input[0].Split(",", StringSplitOptions.TrimEntries);
            var ans = 0L;
            var cache = new Dictionary<int, long>();
            foreach (var str in input.Skip(2))
            {
                cache.Clear();
                ans += Make(str, segments);

                long Make(ReadOnlySpan<char> partial, string[] segments)
                {
                    if (cache.TryGetValue(partial.Length, out var ret))
                        return ret;
                    ret = 0;
                    foreach (var segment in segments)
                    {
                        if (partial.StartsWith(segment))
                        {
                            if (partial.Length == segment.Length)
                                ret++;
                            else
                                ret += Make(partial[segment.Length..], segments);
                        }
                    }

                    return cache[partial.Length] = ret;
                }
            }

            return ans.ToString();
        }
    }
}