using System.Runtime.InteropServices;
using AdventOfCode.lib;

namespace AdventOfCode
{
    internal static class Day5
    {
        [AnswerMethod(2024, 5, 1)]
        public static string Part1(string[] input)
        {
            var edge = new Dictionary<int, HashSet<int>>();
            var mode = 0;
            var tot = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) mode = 1;
                else if (mode == 0)
                {
                    var inp = line.Split('|').Select(int.Parse).ToArray();
                    ref var set = ref CollectionsMarshal.GetValueRefOrAddDefault(edge, inp[0], out var exists);
                    if (!exists)
                    {
                        set = new HashSet<int>();
                    }

                    set!.Add(inp[1]);
                }
                else
                {
                    var inp = line.Split(',').Select(int.Parse).ToArray();
                    for (global::System.Int32 i = 0; i < inp.Length; i++)
                    {
                        for (global::System.Int32 j = i + 1; j < inp.Length; j++)
                        {
                            if (!edge.GetValueOrDefault(inp[i], new HashSet<int>()).Contains(inp[j]))
                                goto loopend;
                        }
                    }
                    tot += inp[inp.Length/2];
                loopend:
                    ;
                        
                }
            }

            return tot.ToString();
        }

        [AnswerMethod(2024, 5, 2)]
        public static string Part2(string[] input)
        {
//            input = 
//"""
//47|53
//97|13
//97|61
//97|47
//75|29
//61|13
//75|53
//29|13
//97|29
//53|29
//61|53
//97|53
//61|29
//47|13
//75|47
//97|75
//47|61
//75|61
//47|29
//75|13
//53|13

//75,47,61,53,29
//97,61,53,29,13
//75,29,13
//75,97,47,61,53
//61,13,29
//97,13,75,29,47
//""".Split('\n');
            var edges = new Dictionary<int, HashSet<int>>();
            var mode = 0;
            var tot = 0;
            foreach (var line2 in input)
            {
                var line = line2.TrimEnd();
                if (string.IsNullOrEmpty(line))
                {
                    mode = 1;
                }
                else if (mode == 0)
                {
                    var inp = line.Split('|').Select(int.Parse).ToArray();
                    ref var set = ref CollectionsMarshal.GetValueRefOrAddDefault(edges, inp[0], out var exists);
                    if (!exists)
                    {
                        set = new HashSet<int>();
                    }

                    set!.Add(inp[1]);
                }
                else
                {
                    var inp = line.Split(',').Select(int.Parse).ToArray();
                    var inp2 = (int[])inp.Clone();
                    Array.Sort(inp, (x, y) => edges.TryGetValue(x, out var edgeList) && edgeList.Contains(y) ? -1 : 1);
                    if (!inp.SequenceEqual(inp2))
                    {
                        Console.WriteLine(string.Join(',', inp));
                        tot += inp[inp.Length / 2];
                    }
                }
            }

            return tot.ToString();
        }

        [AnswerMethod(2024, 5, 2)]
        public static string Part2_linq(string[] input)
        {
            var edges = input
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .Select(line => line.Split('|').Select(int.Parse))
                .ToLookup(x => (x.ElementAt(0), x.ElementAt(1)));
            return input
                .SkipWhile(line => !string.IsNullOrEmpty(line))
                .Skip(1)
                .Select(line => line.Split(',').Select(int.Parse).ToArray())
                .Select(line => (line, sorted: line.OrderBy(x => x, Comparer<int>.Create((x, y) => edges.Contains((x, y)) ? -1 : 1))))
                .Where(tup => !tup.line.SequenceEqual(tup.sorted))
                .Select(tup => tup.sorted.ElementAt(tup.line.Length / 2))
                .Sum()
                .ToString();
        }

        [AnswerMethod(2024, 5, 2)]
        public static string Part2_topsort(string[] input)
        {
//            input =
//"""
//47|53
//97|13
//97|61
//97|47
//75|29
//61|13
//75|53
//29|13
//97|29
//53|29
//61|53
//97|53
//61|29
//47|13
//75|47
//97|75
//47|61
//75|61
//47|29
//75|13
//53|13

//75,47,61,53,29
//97,61,53,29,13
//75,29,13
//75,97,47,61,53
//61,13,29
//97,13,75,29,47
//""".Split('\n').Select(s => s.Trim()).ToArray();
            var edgeList = input
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .Select(line => line.Split('|').Select(int.Parse).ToList())
                .ToList();
            var nodes = edgeList.SelectMany(x => x).Distinct().ToList();
            var graph = Graph.Create(nodes, edgeList.Select(edge => (edge[0], edge[1])));

            graph.DepthFirstSearch(graph.AdjacencyList.Keys.First()!, _=> { }, _ => { });
            var topsort = graph.TopologicalSort();
            var sort2 = nodes.ToArray();
            Array.Sort(sort2, (x, y) => graph.AdjacencyList.TryGetValue(x, out var edgeList) && edgeList.Contains(y) ? -1 : 1);
            var sort3 = nodes.OrderBy(x => -x).ToArray();
            Array.Sort(sort3, (x, y) => graph.AdjacencyList.TryGetValue(x, out var edgeList) && edgeList.Contains(y) ? -1 : 1);

            var ret = 0;
            foreach (var seq in input
                .SkipWhile(line => !string.IsNullOrEmpty(line))
                .Skip(1)
                .Select(line => line.Split(',').Select(int.Parse).ToArray()))
            {
                var sorted = topsort.Where(x => seq.Contains(x)).ToList();
                if (!sorted.SequenceEqual(seq))
                {
                    Console.WriteLine(string.Join(',', sorted));
                    ret += sorted[sorted.Count / 2];
                }
            }

            return ret.ToString();
        }

        [AnswerMethod(2024, 5, 2)]
        public static string Part2_topsorttest(string[] input)
        {
            var g = Graph.Create([4,2,3,5,1], [
                (1,2),
                (1,3),
                (1,4),
                (1,5),
                (2,3),
                (2,4),
                (2,5),
                (3,4),
                (3,5),
                (5,4),
            ]);

            var r = g.TopologicalSort();

            return "";
        }
    }
}