using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using AdventOfCode.lib;
using BenchmarkDotNet.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode
{
    internal static class Day17
    {
        [AnswerMethod(2024, 17, 1)]
        public static string Part1(string[] input)
        {
            //input =
            //    """
            //    Register A: 729
            //    Register B: 0
            //    Register C: 0

            //    Program: 0,1,5,4,3,0
            //    """.Split("\n", StringSplitOptions.TrimEntries);
            var a = long.Parse(input[0].Split(":", StringSplitOptions.TrimEntries)[1]);
            var b = long.Parse(input[1].Split(":", StringSplitOptions.TrimEntries)[1]);
            var c = long.Parse(input[2].Split(":", StringSplitOptions.TrimEntries)[1]);

            var p = input[4].Split(":", StringSplitOptions.TrimEntries)[1].Split(",", StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
            var output = new List<long>();
            var ip = 0L;
            while (ip < p.Length)
            {
                //Console.WriteLine(ip);
                var literal = p[ip + 1];
                var combo = p[ip + 1];
                combo = combo switch
                {
                    4 => a,
                    5 => b,
                    6 => c,
                    _ => combo
                };

                switch (p[ip])
                {
                    case 0:
                        a = a >> (int)combo;
                        break;
                    case 1:
                        b = b ^ literal;
                        break;
                    case 2:
                        b = combo % 8;
                        break;
                    case 3:
                        if (a != 0)
                        {
                            ip = literal;
                            goto End;
                        }
                        break;
                    case 4:
                        b = b ^ c;
                        break;
                    case 5:
                        output.Add(combo % 8);
                        break;
                    case 6:
                        b = a >> (int)combo;
                        break;
                    case 7:
                        c = a >> (int)combo;
                        break;
                }
                ip += 2;
            End:
                ;
            }

            return string.Join(",", output).ToString();
        }

        [AnswerMethod(2024, 17, 2)]
        public static string Part2(string[] input)
        {
            //input =
            //    """
            //    Register A: 729
            //    Register B: 0
            //    Register C: 0

            //    Program: 0,1,5,4,3,0
            //    """.Split("\n", StringSplitOptions.TrimEntries);
            var a = long.Parse(input[0].Split(":", StringSplitOptions.TrimEntries)[1]);
            var b = long.Parse(input[1].Split(":", StringSplitOptions.TrimEntries)[1]);
            var c = long.Parse(input[2].Split(":", StringSplitOptions.TrimEntries)[1]);

            var p = input[4].Split(":", StringSplitOptions.TrimEntries)[1].Split(",", StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
            var output = new List<long>();

            for (var i = 0; i < p.Length; i++)
            {
                if (i % 2 == 1) continue;
                Console.Write(i + ": ");
                var literal = p[i + 1];
                var combo = p[i + 1].ToString();
                if (p[i + 1] > 3)
                {
                    combo = ((char)('a' + (char)p[i + 1] - 4)).ToString();
                }

                if (p[i] == 0)
                {
                    Console.WriteLine($"a = a >> {combo}");
                }
                if (p[i] == 1)
                {
                    Console.WriteLine($"b = b ^ {literal}");
                }
                if (p[i] == 2)
                {
                    Console.WriteLine($"b = {combo} % 8");
                }
                if (p[i] == 3)
                {
                    Console.WriteLine($"jump if A != 0 to {literal}");
                }
                if (p[i] == 4)
                {
                    Console.WriteLine($"b = b ^ c");
                }
                if (p[i] == 5)
                {
                    Console.WriteLine($"output {combo} % 8");
                }
                if (p[i] == 6)
                {
                    Console.WriteLine($"b = a >> {combo}");
                }
                if (p[i] == 7)
                {
                    Console.WriteLine($"c = a >> {combo}");
                }
            }

            var pStr = input[4].Split(":", StringSplitOptions.TrimEntries)[1];
            var candidates = new List<long> { 0 };
            while (true)
            {
                var newCandidates = new List<long>();
                foreach (var candidate in candidates)
                {
                    for (var i = 0L; i < 8; i++)
                    {
                        var test = (candidate << 3) | i;
                        var testLst = Run(test, 0, 0);
                        var testStr = string.Join(",", testLst);
                        if (pStr == testStr)
                        {
                            return test.ToString();
                        }
                        if (pStr.EndsWith(testStr))
                        {
                            newCandidates.Add(test);
                        }
                    }
                }
                candidates = newCandidates;
            }

            List<long> Run(long a, long b, long c)
            {
                var ip = 0L;
                var ret = new List<long>();
                while (ip < p.Length)
                {
                    var literal = p[ip + 1];
                    var combo = p[ip + 1];
                    combo = combo switch
                    {
                        4 => a,
                        5 => b,
                        6 => c,
                        _ => combo
                    };

                    switch (p[ip])
                    {
                        case 0:
                            a = a >> (int)combo;
                            break;
                        case 1:
                            b = b ^ literal;
                            break;
                        case 2:
                            b = combo % 8;
                            break;
                        case 3:
                            if (a != 0)
                            {
                                ip = literal;
                                goto End;
                            }
                            break;
                        case 4:
                            b = b ^ c;
                            break;
                        case 5:
                            ret.Add(combo % 8);
                            break;
                        case 6:
                            b = a >> (int)combo;
                            break;
                        case 7:
                            c = a >> (int)combo;
                            break;
                    }
                    ip += 2;
                End:
                    ;
                }

                return ret;
            }

            throw new Exception();
        }

        [AnswerMethod(2024, 17, 2)]
        public static string Part2_faster(string[] input)
        {
            //var p = input[4].Split(":", StringSplitOptions.TrimEntries)[1].Split(",", StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
            byte[] p = [2, 4, 1, 7, 7, 5, 1, 7, 0, 3, 4, 1, 5, 5, 3, 0];
            byte?[] cached = new byte?[1 << 10];
            
            var ret = Solve(p, 0, cached, p)!.ToString();
            return ret!;

            static long? Solve(ReadOnlySpan<byte> goal, long curr, byte?[] cached, byte[] p)
            {
                if (goal.Length == 0)
                    return curr;

                var last = goal[^1];

                var testVec = Vector256.Create<uint>([0, 1, 2, 3, 4, 5, 6, 7]);
                testVec += Vector256.Create((uint)((curr << 3) & 0x3ff));
                var solveRes = RunOneIterVec(testVec, p);
                var testRes = Avx2.MoveMask(Avx2.CompareEqual(Vector256.Create<uint>(last), solveRes).AsByte());
                if (testRes == 0)
                {
                    return null;
                }

                for (var i = 0; i < Vector256<uint>.Count; i++)
                {
                    if ((testRes & (1 << (4 * i))) != 0)
                    {
                        var ret = Solve(goal[0..^1], (curr << 3) | i, cached, p);
                        if (ret != null) return ret;
                    }
                }
                //var pre = (curr << 3) & 0x3ff;
                //for (var i = 0L; i < 8; i++)
                //{
                //    var test = pre | i;
                //    var oneIter = cached[test];
                //    if (oneIter == null)
                //        oneIter = cached[test] = RunOneIter(test, p);
                //    if (oneIter == last)
                //    {
                //        var ret = Solve(goal[0..^1], (curr << 3) | i, cached, p);
                //        if (ret != null) return ret;
                //    }
                //}

                return null;
            }

            static Vector256<uint> RunOneIterVec(Vector256<uint> a, byte[] p)
            {
                Vector256<uint> b = Vector256.Create(0u);
                Vector256<uint> c = Vector256.Create(0u);
                var ip = 0L;
                while (ip < p.Length)
                {
                    var literal = (uint)p[ip + 1];
                    var literalVec = Vector256.Create((uint)p[ip + 1]);
                    var combo = Vector256.Create((uint)p[ip + 1]);
                    combo = p[ip + 1] switch
                    {
                        4 => a,
                        5 => b,
                        6 => c,
                        _ => combo
                    };

                    switch (p[ip])
                    {
                        case 0:
                            a = Avx2.ShiftRightLogicalVariable(a, combo);// >> combo;
                            break;
                        case 1:
                            b = b ^ literalVec;
                            break;
                        case 2:
                            b = combo & Vector256.Create(0b111u);
                            break;
                        case 3:
                            var comparison = Avx2.MoveMask(Avx2.CompareEqual(a, Vector256.Create(0u)).AsByte());
                            if (comparison == 0)
                            {
                                ip = literal;
                                goto End;
                            }
                            else if (comparison == -1)
                            {
                                break;
                            }
                            throw new Exception();
                        case 4:
                            b = b ^ c;
                            break;
                        case 5:
                            return combo & Vector256.Create(0b111u);
                        case 6:
                            b = Avx2.ShiftRightLogicalVariable(a, combo);
                            break;
                        case 7:
                            c = Avx2.ShiftRightLogicalVariable(a, combo);
                            break;
                    }
                    ip += 2;
                End:
                    ;
                }

                throw new Exception();
            }

            static byte RunOneIter(long a, byte[] p)
            {
                long b = 0;
                long c = 0;
                var ip = 0L;
                while (ip < p.Length)
                {
                    var literal = p[ip + 1];
                    long combo = p[ip + 1];
                    combo = combo switch
                    {
                        4 => a,
                        5 => b,
                        6 => c,
                        _ => combo
                    };

                    switch (p[ip])
                    {
                        case 0:
                            a = a >> (int)combo;
                            break;
                        case 1:
                            b = b ^ literal;
                            break;
                        case 2:
                            b = combo % 8;
                            break;
                        case 3:
                            if (a != 0)
                            {
                                ip = literal;
                                goto End;
                            }
                            break;
                        case 4:
                            b = b ^ c;
                            break;
                        case 5:
                            return (byte)(combo % 8);
                        case 6:
                            b = a >> (int)combo;
                            break;
                        case 7:
                            c = a >> (int)combo;
                            break;
                    }
                    ip += 2;
                End:
                    ;
                }

                throw new Exception();
            }
        }
    }
}