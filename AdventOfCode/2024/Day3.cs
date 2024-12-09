using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal static class Day3
    {
        [AnswerMethod(2024, 3, 1)]
        public static string Part1(IEnumerable<string> input)
        {
            //input =
            //    ["""
            //    xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
            //    """];

            var tot = 0;
            foreach (var inp in input)
            {
                for (int i = 0; i < inp.Length; i++)
                {
                    if (inp.AsSpan(i).StartsWith("mul("))
                    {
                        var num = 0;
                        i += 4;
                        if (char.IsDigit(inp[i]))
                        {
                            num = inp[i] - '0';
                            i++;
                            if (char.IsDigit(inp[i]))
                            {
                                num = num * 10 + (inp[i] - '0');
                                i++;
                                if (char.IsDigit(inp[i]))
                                {
                                    num = num * 10 + (inp[i] - '0');
                                    i++;
                                }
                            }

                            var num2 = 0;
                            if (inp[i] == ',')
                            {
                                i++;
                                if (char.IsDigit(inp[i]))
                                {
                                    num2 = inp[i] - '0';
                                    i++;
                                    if (char.IsDigit(inp[i]))
                                    {
                                        num2 = num2 * 10 + (inp[i] - '0');
                                        i++;
                                        if (char.IsDigit(inp[i]))
                                        {
                                            num2 = num2 * 10 + (inp[i] - '0');
                                            i++;
                                        }
                                    }

                                    if (inp[i] == ')')
                                    {
                                        tot += num * num2;
                                    }
                                }
                            }
                        }
                        i--;
                    }
                }
            }

            return tot.ToString();
        }

        [AnswerMethod(2024, 3, 2)]
        public static string Part2(IEnumerable<string> input)
        {
            //input =
            //    ["""
            //    xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
            //    """];

            var tot = 0;
            var enabled = true;
            foreach (var inp in input)
            {
                for (int i = 0; i < inp.Length; i++)
                {
                    if (inp.AsSpan(i).StartsWith("don't()"))
                    {
                        enabled = false;
                    }
                    else if (inp.AsSpan(i).StartsWith("do()"))
                    {
                        enabled = true;
                    }

                    if (!enabled) continue;

                    if (inp.AsSpan(i).StartsWith("mul("))
                    {
                        var num = 0;
                        i += 4;
                        if (char.IsDigit(inp[i]))
                        {
                            num = inp[i] - '0';
                            i++;
                            if (char.IsDigit(inp[i]))
                            {
                                num = num * 10 + (inp[i] - '0');
                                i++;
                                if (char.IsDigit(inp[i]))
                                {
                                    num = num * 10 + (inp[i] - '0');
                                    i++;
                                }
                            }

                            var num2 = 0;
                            if (inp[i] == ',')
                            {
                                i++;
                                if (char.IsDigit(inp[i]))
                                {
                                    num2 = inp[i] - '0';
                                    i++;
                                    if (char.IsDigit(inp[i]))
                                    {
                                        num2 = num2 * 10 + (inp[i] - '0');
                                        i++;
                                        if (char.IsDigit(inp[i]))
                                        {
                                            num2 = num2 * 10 + (inp[i] - '0');
                                            i++;
                                        }
                                    }

                                    if (inp[i] == ')')
                                    {
                                        tot += num * num2;
                                    }
                                }
                            }
                        }
                        i--;
                    }
                }
            }

            return tot.ToString();
        }

        [AnswerMethod(2024, 3, 1)]
        public static string Part1_clean(string input)
        {
            var mulRegex = new Regex(@"mul\(([\d]{1,3}),([\d]{1,3})\)");
            return mulRegex.Matches(input).Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).Sum().ToString();
        }

        [AnswerMethod(2024, 3, 2)]
        public static string Part2_clean(string input)
        {
            var mulRegex = new Regex(@"mul\(([\d]{1,3}),([\d]{1,3})\)");
            var doRegex = new Regex(@"do\(\).*?don't\(\)", RegexOptions.Singleline);
            return doRegex
                .Matches("do()" + input + "don't()")
                .SelectMany(dm => mulRegex
                    .Matches(input.Substring(dm.Index, dm.Length))
                    .Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)))
                .Sum()
                .ToString();
        }
    }
}
