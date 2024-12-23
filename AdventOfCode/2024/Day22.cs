using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using AdventOfCode.lib;

namespace AdventOfCode;

internal static partial class Day22
{
    [AnswerMethod(2024, 22, 1)]
    public static string Part1(string[] input)
    {
        //input = """
        //    1
        //    10
        //    100
        //    2024
        //    """.Split('\n', StringSplitOptions.TrimEntries);
        var numbers = input.Select(long.Parse).ToArray();
        var ans = 0L;
        foreach (var number in numbers)
        {
            var secret = number;
            for (global::System.Int32 i = 0; i < 2000; i++)
            {

                Mix(ref secret, 64 * secret);
                Prune(ref secret);
                Mix(ref secret, secret / 32);
                Prune(ref secret);
                Mix(ref secret, 2048 * secret);
                Prune(ref secret);
            }

            ans += secret;

            void Mix(ref long secret, long x)
            {
                secret ^= x;
            }

            void Prune(ref long secret)
            {
                secret %= 16777216;
            }
        }

        return ans.ToString();
    }

    //[AnswerMethod(2024, 22, 2)]
    public static string Part2(string[] input)
    {
        //input = """
        //    1
        //    2
        //    3
        //    2024
        //    """.Split('\n', StringSplitOptions.TrimEntries);
        //input = ["123"];
        var numbers = input.Select(long.Parse).ToArray();
        var allChanges = new List<List<long>>();
        var allPrices = new List<List<long>>();
        foreach (var number in numbers)
        {
            var secrets = ComputeSecrets(number).Select(x => x % 10).ToList();
            allPrices.Add(secrets);
            var changes = new List<long>();
            var curr = number % 10;
            for (var i = 0; i < secrets.Count; i++)
            {
                changes.Add(secrets[i] - curr);
                curr = secrets[i];
            }
            allChanges.Add(changes);
        }

        var all4sequences = new HashSet<(long, long, long, long)>();

        foreach (var changes in allChanges)
        {
            for (var i = 0; i + 3 < changes.Count; i++)
            {
                all4sequences.Add((
                    changes[i],
                    changes[i + 1],
                    changes[i + 2],
                    changes[i + 3]));
            }
        }

        var ans = 0L;
        foreach (var sequence in all4sequences)
        {
            var price = 0L;
            foreach (var (changeIdx, changes) in allChanges.Index())
            {
                for (var i = 0; i + 3 < changes.Count; i++)
                {
                    if (
                        changes[i] == sequence.Item1 &&
                        changes[i + 1] == sequence.Item2 &&
                        changes[i + 2] == sequence.Item3 &&
                        changes[i + 3] == sequence.Item4
                        )
                    {
                        price += allPrices[changeIdx][i + 3];
                        break;
                    }
                }
            }

            //if (price > ans)
            //{
            //    Console.WriteLine(sequence);
            //}
            ans = Math.Max(ans, price);
        }

        return ans.ToString();

        static IEnumerable<long> ComputeSecrets(long secret)
        {
            for (global::System.Int32 i = 0; i < 2000; i++)
            {
                Mix(ref secret, 64 * secret);
                Prune(ref secret);
                Mix(ref secret, secret / 32);
                Prune(ref secret);
                Mix(ref secret, 2048 * secret);
                Prune(ref secret);

                yield return secret;
            }

            void Mix(ref long secret, long x)
            {
                secret ^= x;
            }

            void Prune(ref long secret)
            {
                secret %= 16777216;
            }
        }
    }

    //[AnswerMethod(2024, 22, 2)]
    public static string Part2_faster(string[] input)
    {
        //input = """
        //    1
        //    2
        //    3
        //    2024
        //    """.Split('\n', StringSplitOptions.TrimEntries);
        //input = ["123"];
        var numbers = input.Select(long.Parse).ToArray();
        var allChanges = new List<List<long>>();
        var allPrices = new List<List<long>>();
        foreach (var number in numbers)
        {
            var secrets = ComputeSecrets(number).Select(x => x % 10).ToList();
            allPrices.Add(secrets);
            var changes = new List<long>();
            var curr = number % 10;
            for (var i = 0; i < secrets.Count; i++)
            {
                changes.Add(secrets[i] - curr);
                curr = secrets[i];
            }
            allChanges.Add(changes);
        }

        var all4sequences = new Dictionary<(long, long, long, long), (int, long)>();

        foreach (var (idx, changes) in allChanges.Index())
        {
            for (var i = 0; i + 3 < changes.Count; i++)
            {
                var seq = (changes[i], changes[i + 1], changes[i + 2], changes[i + 3]);
                ref var value = ref all4sequences.GetValueRefOrAddDefault(seq, out _);
                if (value.Item1 == idx + 1) continue;
                value = (idx + 1, value.Item2 + allPrices[idx][i + 3]);
            }
        }

        return all4sequences.Max(kvp => kvp.Value.Item2).ToString();

        static IEnumerable<long> ComputeSecrets(long secret)
        {
            for (global::System.Int32 i = 0; i < 2000; i++)
            {
                Mix(ref secret, 64 * secret);
                Prune(ref secret);
                Mix(ref secret, secret / 32);
                Prune(ref secret);
                Mix(ref secret, 2048 * secret);
                Prune(ref secret);

                yield return secret;
            }

            void Mix(ref long secret, long x)
            {
                secret ^= x;
            }

            void Prune(ref long secret)
            {
                secret %= 16777216;
            }
        }
    }

    [AnswerMethod(2024, 22, 2)]
    public static string Part2_ints(string input)
    {
        var regex = SplitByNewLinesRegex();
        const int size = 1 << (5 + 5 + 5 + 5);
        int[] prices = new int[size];
        var seen = new BitArray(size);
        var mask = 0x1f;
        foreach (var range in regex.EnumerateSplits(input))
        {
            var prev = int.Parse(input.AsSpan(range), System.Globalization.NumberStyles.None);
            var diff = 0;
            for (var i = 0; i < 2000; i++)
            {
                var secret = prev;
                secret ^= (64 * secret) % 16777216;
                secret ^= (secret / 32);
                secret ^= (int)(((uint)secret << 11) % 16777216);

                var price = secret % 10;
                diff = (diff << 5) | ((price - prev % 10) & mask);
                diff &= 0xfffff;

                if (i >= 3 && !seen[diff])
                {
                    seen[diff] = true;
                    prices[diff] = prices[diff] + price;
                }

                prev = secret;
            }

            seen.SetAll(false);
        }

        return prices.Max().ToString();
    }

    [AnswerMethod(2024, 22, 2)]
    public static string Part2_ints_simd(string input)
    {
        var regex = SplitByNewLinesRegex();
        const int size = 1 << (5 + 5 + 5 + 5);
        uint[] prices = new uint[size];

        var seen = new BitArray[16];
        for (var i = 0; i < 16; i++)
            seen[i] = new BitArray(size);

        var mask = Vector512.Create<uint>(0x1f);
        var diffmask = Vector512.Create<uint>(0xfffff);
        var prune = Vector512.Create<uint>(16777216 - 1);

        int j = 0;
        Span<uint> buffer = stackalloc uint[16];
        foreach (var range in regex.EnumerateSplits(input))
        {
            buffer[j] = uint.Parse(input.AsSpan(range), System.Globalization.NumberStyles.None);
            if (++j != 16) continue;

            j = 0;

            Vector512<uint> secret = Vector512.Create<uint>(buffer);
            Vector512<uint> prevMod10 = Modulo10(secret);
            Vector512<uint> diff = Vector512<uint>.Zero;

            for (var i = 0; i < 2000; i++)
            {
                secret ^= (secret << 6) & prune;
                secret ^= secret >> 5;
                secret ^= ((secret << 11) & prune);

                var price = Modulo10(secret);
                diff = (diff << 5) | ((price.AsInt32() - prevMod10.AsInt32()).AsUInt32() & mask);
                diff &= diffmask;
                prevMod10 = price;
                
                if (i >= 3)
                {
                    for (var k = 0; k < 16; k++)
                    {
                        var idx = (int)diff[k];
                        if (!seen[k][idx])
                        {
                            seen[k][idx] = true;
                            prices[idx] += price[k];
                        }
                    }
                }
            }

            foreach (var arr in seen)
            {
                arr.SetAll(false);
            }
        }

        for (j -= 1; j >= 0; j--)
        {
            var prev = buffer[j];
            var diff = 0u;
            for (var i = 0; i < 2000; i++)
            {
                var secret = prev;
                secret ^= (64 * secret) % 16777216;
                secret ^= (secret / 32);
                secret ^= (secret << 11) % 16777216;

                var price = secret % 10;
                diff = (diff << 5) | unchecked(((uint)(((int)price - (int)prev % 10)) & 0x1f));
                diff &= 0xfffff;

                if (i >= 3 && !seen[j][(int)diff])
                {
                    seen[j][(int)diff] = true;
                    prices[diff] = prices[diff] + price;
                }

                prev = secret;
            }
        }

        return prices.Max().ToString();   
    }

    // https://stackoverflow.com/a/77631254
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Vector512<uint> Modulo10(in Vector512<uint> dividend)
    {
        return (Modulo5(dividend >> 1) << 1) | (dividend & Vector512.Create<uint>(1));

        static Vector512<uint> Modulo5(in Vector512<uint> inp)
        {
            var dividend = (inp >>> 16) + (inp & Vector512.Create<uint>(0xffff));
            dividend = (dividend >>> 8) + (dividend & Vector512.Create<uint>(0xff));
            dividend = (dividend >>> 4) + (dividend & Vector512.Create<uint>(0xf));
            dividend = (dividend >>> 4) + (dividend & Vector512.Create<uint>(0xf));

            return (
                (Avx512F.ShiftRightLogicalVariable(Vector512.Create<uint>(0x10840), dividend) & Vector512.Create<uint>(4))
              | (Avx512F.ShiftRightLogicalVariable(Vector512.Create<uint>(0x46318), dividend) & Vector512.Create<uint>(2))
              | (Avx512F.ShiftRightLogicalVariable(Vector512.Create<uint>(0x1294a), dividend) & Vector512.Create<uint>(1))
            );
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Vector512<T> Modulo<T>(in Vector512<T> dividend, in Vector512<T> divisor)
    {
        // SAD
        return dividend - dividend / divisor * divisor;
    }

    [GeneratedRegex("\n", RegexOptions.Singleline)]
    private static partial Regex SplitByNewLinesRegex();
}