using System.Numerics;

namespace AdventOfCode.Year2025;

internal static class Day2
{
    [AnswerMethod(2025, 2, 1)]
    public static string Part1(string input)
    {
        //input = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,\r\n1698522-1698528,446443-446449,38593856-38593862,565653-565659,\r\n824824821-824824827,2121212118-2121212124";

        BigInteger ans = 0;

        var ranges = input.Split(',').Select(s =>
        {
            var spl = s.Split('-');
            return (BigInteger.Parse(spl[0]), BigInteger.Parse(spl[1]));
        }).ToArray();

        foreach (var (start, end) in ranges)
        {
            for (var i = start; i <= end; i++)
            {
                var test = i.ToString();
                if (test.Length % 2 == 0 &&
                    test.Substring(0, test.Length/2) == test.Substring(test.Length/2))
                {
                    ans += i;
                }
            }
        }

        return ans.ToString();
    }

    [AnswerMethod(2025, 2, 2)]
    public static string Part2(string input)
    {
        //input = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,\r\n1698522-1698528,446443-446449,38593856-38593862,565653-565659,\r\n824824821-824824827,2121212118-2121212124";

        BigInteger ans = 0;

        var ranges = input.Split(',').Select(s =>
        {
            var spl = s.Split('-');
            return (BigInteger.Parse(spl[0]), BigInteger.Parse(spl[1]));
        }).ToArray();

        foreach (var (start, end) in ranges)
        {
            for (var i = start; i <= end; i++)
            {
                var test = i.ToString();

                for (var d = 1; d < test.Length; d++)
                {
                    if (test.Length % d != 0)
                        continue;

                    var match = test.Substring(0, d);

                    for (var ind = 1; ind < test.Length / d; ind++)
                    {
                        if (test.Substring(ind * d, d) != match)
                        {
                            goto next;
                        }
                    }

                    ans += i;
                    goto next2;

                next:
                    ;
                }
            next2:
                ;
            }
        }

        return ans.ToString();
    }
}
