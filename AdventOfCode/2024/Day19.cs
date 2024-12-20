namespace AdventOfCode;

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
        var cache = new long?[input.Skip(2).Max(s => s.Length) + 1];
        foreach (var str in input.Skip(2))
        {
            ans += Make(str, segments);
            Array.Clear(cache, 0, str.Length + 1);

            long Make(ReadOnlySpan<char> partial, string[] segments)
            {
                ref var cached = ref cache[partial.Length];
                if (cached.HasValue)
                    return cached.Value;
                var ret = 0L;
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

                return (cached = ret).Value;
            }
        }

        return ans.ToString();
    }
}