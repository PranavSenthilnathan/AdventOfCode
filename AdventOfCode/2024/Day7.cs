using System.Buffers;
using AdventOfCode.lib;

namespace AdventOfCode
{
    internal static class Day7
    {
        [AnswerMethod(2024, 7, 1)]
        public static string Part1(string[] input)
        {
            //input = """
            //                    190: 10 19
            //    3267: 81 40 27
            //    83: 17 5
            //    156: 15 6
            //    7290: 6 8 6 15
            //    161011: 16 10 13
            //    192: 17 8 14
            //    21037: 9 7 18 13
            //    292: 11 6 16 20
            //    """.Split('\n', StringSplitOptions.TrimEntries);
            var ans = 0L;
            foreach (var line in input)
            {
                var spl = line.Split(":");
                var tot = long.Parse(spl[0]);
                var nums = spl[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                
                nums = nums.Reverse().ToArray();

                if (Solve(nums).Contains(tot))
                {
                    ans += tot;
                }

                HashSet<long> Solve(Span<long> span)
                {
                    if (span.Length == 1) return new HashSet<long>() { span[0] };

                    var pos = Solve(span.Slice(1));
                    var ret = new HashSet<long>();
                    foreach (var n in pos)
                    {
                        ret.Add(n * span[0]);
                        ret.Add(n + span[0]);
                    }
                    return ret;
                }
            }
            return ans.ToString();
        }

        [AnswerMethod(2024, 7, 2)]
        public static string Part2(string[] input)
        {
            //input = """
            //                    190: 10 19
            //    3267: 81 40 27
            //    83: 17 5
            //    156: 15 6
            //    7290: 6 8 6 15
            //    161011: 16 10 13
            //    192: 17 8 14
            //    21037: 9 7 18 13
            //    292: 11 6 16 20
            //    """.Split('\n', StringSplitOptions.TrimEntries);
            var pool = new HashSetPool<long>(20);
            var numRanges = new Range[1000];
            var nums = new long[1000];
            var ans = 0L;
            foreach (var line in input)
            {
                var spl = line.Split(":");
                var tot = long.Parse(spl[0]);
                var N = MemoryExtensions.Split(spl[1], numRanges, " ", StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < N; i++)
                {
                    nums[i] = long.Parse(spl[1][numRanges[i]]);
                }

                var set = Solve(nums.AsSpan(0, N));
                if (set.Contains(tot))
                {
                    ans += tot;
                }
                pool.Return(set);

                HashSet<long> Solve(Span<long> span)
                {
                    var ret = pool.Rent();

                    if (span.Length == 1)
                    {
                        ret.Add(span[0]);
                        return ret;
                    }

                    var pos = Solve(span[0..^1]);
                    Span<char> buffer = stackalloc char[50];
                    foreach (var n in pos)
                    {
                        ret.Add(n * span[^1]);
                        ret.Add(n + span[^1]);
                        n.TryFormat(buffer, out var written);
                        span[^1].TryFormat(buffer[written..], out var written2);
                        ret.Add(long.Parse(buffer[0..(written + written2)]));
                    }
                    pool.Return(pos);
                    return ret;
                }
            }

            return ans.ToString();
        }

        [AnswerMethod(2024, 7, 1)]
        public static string Part1_linq(string[] input)
        {
            return input
                .Select(line => line.Split(':'))
                .Select(line => (goal: long.Parse(line[0]), operands: line[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()))
                .Select(eq => new Func<long, long, long>[] { (x, y) => x + y, (x, y) => x * y }
                    .ToTheNthDimension(eq.operands.Length - 1)
                    .Select(operators =>
                        eq.operands.Skip(1)
                        .Zip(operators)
                        .Aggregate(eq.operands[0], (acc, op) => op.Second(acc, op.First)))
                    .FirstOrDefault(res => res == eq.goal))
                .Sum()
                .ToString();
        }

        [AnswerMethod(2024, 7, 2)]
        public static string Part2_linq2(string[] input)
        {
            return input
                .Select(line => line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray())
                .Select(line => (goal: line[0], fst: line[1], rst: line[2..^0]))
                .Select(eq => eq.rst
                    .Aggregate(new HashSet<long> { eq.fst }, (acc, operand) => new HashSet<long>(
                        acc.SelectMany(achievableResults => 
                            new Func<long, long, long>[] { (x, y) => x + y, (x, y) => x * y, (x, y) => long.Parse(x.ToString() + y.ToString()) }
                            .Select(operation => operation(achievableResults, operand)))))
                    .FirstOrDefault(res => res == eq.goal))
                .Sum()
                .ToString();
        }
    }
}