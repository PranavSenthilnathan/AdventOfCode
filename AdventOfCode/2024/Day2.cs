namespace AdventOfCode;

internal static class Day2
{
    [AnswerMethod(2024, 2, 1)]
    public static string Part1(IEnumerable<string> input)
    {
        //            input =
        //"""
        //7 6 4 2 1
        //1 2 7 8 9
        //9 7 6 2 1
        //1 3 2 4 5
        //8 6 4 4 1
        //1 3 6 7 9
        //""".Split('\n');
        var tot = 0;
        foreach (var line in input)
        {
            var spl = line.Split(" ").Select(int.Parse).ToArray();
            var prev = spl[0];
            var inc = spl[1] > spl[0];
            if (spl[0] != spl[1])
            {
                var res = true;
                foreach (var x in spl.Skip(1))
                {
                    if (x > prev && inc || x < prev && !inc)
                    {
                        var diff = Math.Abs(x - prev);
                        if (diff >= 1 && diff <= 3)
                        {
                            prev = x;
                        }
                        else
                        {
                            res = false;
                            break;
                        }
                    }
                    else
                    {
                        res = false;
                        break;
                    }
                }

                if (res == true)
                    tot++;
            }
        }

        return tot.ToString();
    }

    [AnswerMethod(2024, 2, 2)]
    public static string Part2(IEnumerable<string> input)
    {
        //input =
        //"""
        //7 6 4 2 1
        //1 2 7 8 9
        //9 7 6 2 1
        //1 3 2 4 5
        //8 6 4 4 1
        //1 3 6 7 9
        //""".Split('\n');
        var tot = 0;
        foreach (var line in input)
        {
            var spl = line.Split(" ").Select(int.Parse).ToArray();
            if (Safe(spl))
            {
                tot++;
            }
            else
            {
                for (var i = 0; i < spl.Length; i++)
                {
                    if (Safe(spl.Take(i).Concat(spl.Skip(i + 1))))
                    {
                        tot++;
                        break;
                    }
                }
            }

            static bool Safe(IEnumerable<int> seq)
            {
                var spl = seq.ToArray();
                var prev = spl[0];
                var inc = spl[1] > spl[0];
                if (spl[0] != spl[1])
                {
                    var res = true;
                    foreach (var x in spl.Skip(1))
                    {
                        if (x > prev && inc || x < prev && !inc)
                        {
                            var diff = Math.Abs(x - prev);
                            if (diff >= 1 && diff <= 3)
                            {
                                prev = x;
                            }
                            else
                            {
                                res = false;
                                break;
                            }
                        }
                        else
                        {
                            res = false;
                            break;
                        }
                    }

                    if (res == true)
                        return true;
                }
                return false;
            }
        }

        return tot.ToString();
    }
}
