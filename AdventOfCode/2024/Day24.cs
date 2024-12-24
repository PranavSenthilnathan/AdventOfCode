using AdventOfCode.lib;

namespace AdventOfCode;

internal static partial class Day24
{
    [AnswerMethod(2024, 24, 1)]
    public static string Part1(string[] input)
    {
        //input = ex1.Split('\n', StringSplitOptions.TrimEntries);
        var init = input.TakeWhile(x => !string.IsNullOrEmpty(x));
        var v = new Dictionary<string, int?>();
        var e = new HashSet<(string, string)>();
        var rev = new Dictionary<string, (string, string, string)>();

        foreach (var x in init)
        {
            var spl = x.Split(':');
            v[spl[0]] = int.Parse(spl[1]);
        }

        init = input.SkipWhile(x => !string.IsNullOrEmpty(x)).Skip(1);

        foreach (var x in init)
        {
            var spl = x.Split(' ');
            var (v1, v2) = (spl[0], spl[2]);
            var op = spl[1];
            var v3 = spl[4];

            v.TryAdd(v3, null);
            v.TryAdd(v2, null);
            v.TryAdd(v1, null);
            
            e.Add((v1, v3));
            e.Add((v2, v3));
            rev[v3] = (v1, v2, op);
        }

        var g = Graph.Create(v.Keys, e);
        var sorted = g.TopologicalSort();

        for (int i = 0; i < sorted.Count; i++)
        {
            var vv = sorted[i];
            if (!rev.ContainsKey(vv)) continue;
            v[vv] = rev[vv].Item3 switch
            {
                "AND" => v[rev[vv].Item1] & v[rev[vv].Item2],
                "OR" => v[rev[vv].Item1] | v[rev[vv].Item2],
                "XOR" => v[rev[vv].Item1] ^ v[rev[vv].Item2],
                _ => throw new Exception()
            };
        }

        var ans = v.Where(x => x.Key.StartsWith("z")).OrderBy(x => x.Key).Reverse().Aggregate(0L, (acc, kvp) => (acc << 1) + kvp.Value!.Value);
        var xx = v.Where(x => x.Key.StartsWith("x")).OrderBy(x => x.Key).Reverse().Aggregate(0L, (acc, kvp) => (acc << 1) + kvp.Value!.Value);
        var yy = v.Where(x => x.Key.StartsWith("y")).OrderBy(x => x.Key).Reverse().Aggregate(0L, (acc, kvp) => (acc << 1) + kvp.Value!.Value);
        var ordered = v.OrderBy(x => x.Key).ToList();

        return ans.ToString();
    }

    [AnswerMethod(2024, 24, 2)]
    public static string Part2(string[] input)
    {
        //input = ex1.Split('\n', StringSplitOptions.TrimEntries);
        var init = input.TakeWhile(x => !string.IsNullOrEmpty(x));
        var v = new Dictionary<string, int?>();
        var e = new HashSet<(string, string)>();
        var rev = new Dictionary<string, (string, string, string)>();

        foreach (var inp in init)
        {
            var spl = inp.Split(':');
            v[spl[0]] = int.Parse(spl[1]);
        }

        init = input.SkipWhile(x => !string.IsNullOrEmpty(x)).Skip(1);

        var subs1 = new Dictionary<(string, string, string), (string, string, string)>()
        {
            { ("qhq", "tfc", "z17"), ("qwg", "wvj", "cmv") },
            { ("kkf", "pbw", "z23"), ("kkf", "pbw", "rmj") },
            { ("x38", "y38", "mwp"), ("x38", "y38", "btb") },
            { ("x30", "y30", "z30"), ("knj", "rvp", "rdg") },
        };

        subs1 = subs1.Concat(subs1.Select(x => new KeyValuePair<(string, string, string), (string, string, string)>(x.Value, x.Key))).ToDictionary(x => x.Key, x => x.Value);
        var ans = new Dictionary<(string, string, string), (string, string, string)>(subs1);
        foreach (var inp in init)
        {
            var spl = inp.Split(' ');
            var s2 = new[] { spl[0], spl[2] }.Order().ToArray();
            var (v1, v2) = (s2[0], s2[1]);
            var op = spl[1];
            var v3 = spl[4];

            foreach (var sub in subs1)
            {
                if (sub.Key == (v1, v2, v3))
                {
                    v3 = sub.Value.Item3;
                    subs1.Remove(sub.Key);
                    break;
                }
            }

            v.TryAdd(v3, null);
            v.TryAdd(v2, null);
            v.TryAdd(v1, null);

            e.Add((v1, v3));
            e.Add((v2, v3));
            rev[v3] = (v1, v2, op);
        }

        var g = Graph.Create(v.Keys, e);
        var sorted = g.TopologicalSort();

        for (int i = 0; i < sorted.Count; i++)
        {
            var vv = sorted[i];
            if (!rev.ContainsKey(vv)) continue;
            v[vv] = rev[vv].Item3 switch
            {
                "AND" => v[rev[vv].Item1] & v[rev[vv].Item2],
                "OR" => v[rev[vv].Item1] | v[rev[vv].Item2],
                "XOR" => v[rev[vv].Item1] ^ v[rev[vv].Item2],
                _ => throw new Exception()
            };
        }

        var ans2 = v.Where(x => x.Key.StartsWith("z")).OrderBy(x => x.Key).Reverse().Aggregate(0L, (acc, kvp) => (acc << 1) + kvp.Value!.Value);
        var xx = v.Where(x => x.Key.StartsWith("x")).OrderBy(x => x.Key).Reverse().Aggregate(0L, (acc, kvp) => (acc << 1) + kvp.Value!.Value);
        var yy = v.Where(x => x.Key.StartsWith("y")).OrderBy(x => x.Key).Reverse().Aggregate(0L, (acc, kvp) => (acc << 1) + kvp.Value!.Value);
        var ordered = v.OrderBy(x => x.Key).ToList();

        var subs = new Dictionary<string, string>();
        foreach (var x in rev)
        {
            subs[x.Key] = x.Key + "_" + x.Value.Item3;
        }

        //foreach (var x in rev)
        //{
        //    var i1 = subs.TryGetValue(x.Value.Item1, out var key) ? key : x.Value.Item1;
        //    var i2 = subs.TryGetValue(x.Value.Item2, out key) ? key : x.Value.Item2;
        //    var res = subs.TryGetValue(x.Key, out key) ? key : x.Value.Item2;
        //    Console.WriteLine($"{i1} -> {res}");
        //    Console.WriteLine($"{i2} -> {res}");
        //}

        var badZ = subs.Where(x => x.Value.StartsWith("z") && !x.Value.EndsWith("XOR"));

        return string.Join(",", ans.Select(x => x.Key.Item3).Order());
    }

    private static string ex1 = """
        x00: 1
        x01: 0
        x02: 1
        x03: 1
        x04: 0
        y00: 1
        y01: 1
        y02: 1
        y03: 1
        y04: 1

        ntg XOR fgs -> mjb
        y02 OR x01 -> tnw
        kwq OR kpj -> z05
        x00 OR x03 -> fst
        tgd XOR rvg -> z01
        vdt OR tnw -> bfw
        bfw AND frj -> z10
        ffh OR nrd -> bqk
        y00 AND y03 -> djm
        y03 OR y00 -> psh
        bqk OR frj -> z08
        tnw OR fst -> frj
        gnj AND tgd -> z11
        bfw XOR mjb -> z00
        x03 OR x00 -> vdt
        gnj AND wpb -> z02
        x04 AND y00 -> kjc
        djm OR pbm -> qhw
        nrd AND vdt -> hwm
        kjc AND fst -> rvg
        y04 OR y02 -> fgs
        y01 AND x02 -> pbm
        ntg OR kjc -> kwq
        psh XOR fgs -> tgd
        qhw XOR tgd -> z09
        pbm OR djm -> kpj
        x03 XOR y03 -> ffh
        x00 XOR y04 -> ntg
        bfw OR bqk -> z06
        nrd XOR fgs -> wpb
        frj XOR qhw -> z04
        bqk OR frj -> z07
        y03 OR x01 -> nrd
        hwm AND bqk -> z03
        tgd XOR rvg -> z12
        tnw OR pbm -> gnj
        """;
}