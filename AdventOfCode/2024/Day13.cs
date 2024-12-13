using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using AdventOfCode.lib;

namespace AdventOfCode
{
    internal static class Day13
    {
        [AnswerMethod(2024, 13, 1)]
        public static string Part1(string[] input)
        {
            var ans = 0L;

            for (int i = 0; i < input.Length; i++)
            {
                var s = input[i];
                var A = s.Split(['+', ',']).ToArray();
                var (xa, ya) = (int.Parse(A[1]), int.Parse(A[3]));

                i++;
                s = input[i];
                var B = s.Split(['+', ',']).ToArray();
                var (xb, yb) = (int.Parse(B[1]), int.Parse(B[3]));

                i++;
                s = input[i];
                var G = s.Split(['=', ',']).ToArray();
                var (xg, yg) = (int.Parse(G[1]), int.Parse(G[3]));
                i++;

                var a = 0;
                var minCost = long.MaxValue;
                while (true)
                {
                    var totalX = a * xa;
                    if (totalX > xg)
                    {
                        break;
                    }

                    if ((xg - totalX) % xb != 0) goto Increment;

                    var b = (xg - totalX) / xb;
                    if ((a * ya + b * yb) == yg)
                    {
                        minCost = Math.Min(minCost, a * 3 + b);
                    }
                Increment:
                    a++;
                }

                if (minCost != long.MaxValue)
                {
                    ans += minCost;
                }
            }

            return ans.ToString();
        }

        [AnswerMethod(2024, 13, 2)]
        public static string Part2(string[] input)
        {
            //input =
            //    """
            //    Button A: X+94, Y+34
            //    Button B: X+22, Y+67
            //    Prize: X=8400, Y=5400

            //    Button A: X+26, Y+66
            //    Button B: X+67, Y+21
            //    Prize: X=12748, Y=12176

            //    Button A: X+17, Y+86
            //    Button B: X+84, Y+37
            //    Prize: X=7870, Y=6450

            //    Button A: X+69, Y+23
            //    Button B: X+27, Y+71
            //    Prize: X=18641, Y=10279
            //    """.Split('\n', StringSplitOptions.TrimEntries);
            BigInteger ans = 0L;

            for (int i = 0; i < input.Length; i++)
            {
                var s = input[i];
                var A = s.Split(['+', ',']).ToArray();
                var (xa, ya) = (BigInteger.Parse(A[1]), BigInteger.Parse(A[3]));

                i++;
                s = input[i];
                var B = s.Split(['+', ',']).ToArray();
                var (xb, yb) = (BigInteger.Parse(B[1]), BigInteger.Parse(B[3]));

                i++;
                s = input[i];
                var G = s.Split(['=', ',']).ToArray();
                var (xg, yg) = (BigInteger.Parse(G[1]), BigInteger.Parse(G[3]));
                xg += new BigInteger(10000000000000);
                yg += new BigInteger(10000000000000);
                i++;

                if ((xa * yg - ya * xg) % (xa * yb - ya * xb) == 0)
                {
                    var b = (xa * yg - ya * xg) / (xa * yb - ya * xb);
                    if ((xg - b * xb) % xa == 0)
                    {
                        var a = (xg - b * xb) / xa;
                        Console.WriteLine(a * xa + b * xb == xg);
                        Console.WriteLine(a * ya + b * yb == yg);
                        ans += b + 3 * a;
                    }
                }
            }

            return ans.ToString();
        }
    }
}