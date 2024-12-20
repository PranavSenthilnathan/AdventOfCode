using System.Numerics;

namespace AdventOfCode;

internal static class Day11
{
    [AnswerMethod(2024, 11, 1)]
    public static string Part1(string input)
    {
        //input = "125 17";
        var inp = input.Split(' ', StringSplitOptions.TrimEntries);
        var ll = new LinkedList<string>(inp);
        
        for (var i = 1; i <= 25; i++)
        {
            var newll = new LinkedList<string>();
            foreach (var item in ll)
            {
                if (item == "0")
                    newll.AddLast("1");
                else if (item.Length % 2 == 0)
                {
                    newll.AddLast(BigInteger.Parse(item[0..(item.Length / 2)]).ToString());
                    newll.AddLast(BigInteger.Parse(item[(item.Length / 2)..]).ToString());
                }
                else
                {
                    newll.AddLast((BigInteger.Parse(item) * 2024).ToString());
                }
            }
            ll = newll;
        }

        return ll.Count.ToString();
    }

    [AnswerMethod(2024, 11, 2)]
    public static string Part2(string input)
    {
        //input = "125 17";
        var inp = input.Split(' ', StringSplitOptions.TrimEntries).Select(BigInteger.Parse);
        var ll = new List<BigInteger>(inp);
        var cache = new Dictionary<(BigInteger n, int steps), BigInteger>();

        var ans = new BigInteger(0);
        var N = 75;
        foreach (var item in ll)
        {
            ans += Compute(item, N);
            BigInteger Compute(BigInteger n, int steps)
            {
                if (cache.TryGetValue((n, steps), out var ret)) return ret;

                if (steps == 0) return 1;

                if (n == 0)
                {
                    return cache[(n, steps)] = Compute(1, steps - 1);
                }
                else if (Digits(n) % 2 == 0)
                {
                    var d = Digits(n);
                    var x = BigInteger.Pow(10, d / 2);

                    return cache[(n, steps)] = (Compute(n / x, steps - 1) + Compute(n % x, steps - 1));
                }
                else
                {
                    return cache[(n, steps)] =  Compute(n * 2024, steps - 1);
                }
            }
        }

        return ans.ToString();

        int Digits(BigInteger b)
        {
            if (b < 10) return 1;

            return (int)(1 + Digits(b / 10));
        }
    }
}