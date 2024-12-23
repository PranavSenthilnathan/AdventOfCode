using System.Numerics;
using AdventOfCode.lib;

namespace AdventOfCode;

internal static partial class Day23
{
    [AnswerMethod(2024, 23, 1)]
    public static string Part1(string[] input)
    {
        var edges = new HashSet<(string, string)>();
        var vertices = new HashSet<string>();
        foreach (var line in input)
        {
            var spl = line.Split('-');
            var (s, d) = (spl[0], spl[1]);
            edges.Add((s, d));
            edges.Add((d, s));
            vertices.Add(s);
            vertices.Add(d);
        }

        var adj = Graph.Create(vertices, edges).AdjacencyList;

        var ans = 0L;
        foreach (var v1 in vertices)
        {
            foreach (var v2 in vertices)
            {
                foreach (var v3 in vertices)
                {
                    if (v1 == v2 || v2 == v3 || v1 == v3)
                        continue;
                    if (adj[v1].Contains(v2) && adj[v2].Contains(v3) && adj[v3].Contains(v1))
                    {
                        if (v1[0] == 't' ||
                            v2[0] == 't' ||
                            v3[0] == 't')
                            ans++;
                    }
                }
            }
        }

        return (ans / 6).ToString();
    }

    [AnswerMethod(2024, 23, 2)]
    public static string Part2(string[] input)
    {
        var edges = new HashSet<(string, string)>();
        var vertices = new HashSet<string>();
        foreach (var line in input)
        {
            var spl = line.Split('-');
            var (s, d) = (spl[0], spl[1]);
            edges.Add((s, d));
            edges.Add((d, s));
            vertices.Add(s);
            vertices.Add(d);
        }

        var graph = Graph.Create(vertices, edges);
        var adj = graph.AdjacencyList;

        var pooled = new HashSet<string>();
        var maxComplete = 0;
        var maxCompleteVertices = new List<string>();
        foreach (var (v, es) in adj)
        {
            var esLst = es.ToArray();
            var mask = 1 << es.Count;

            for (var i = 0; i < mask; i++)
            {
                if (BitOperations.PopCount((uint)i) + 1 <= maxComplete)
                    continue;

                pooled.Clear();
                for (var j = 0; (1 << j) <= i; j++)
                {
                    if (((1 << j) & i) != 0)
                    {
                        pooled.Add(esLst[j]);
                    }
                }

                foreach (var v2 in pooled)
                {
                    var numContained = adj[v2].Intersect(pooled).Count();
                    if (numContained != pooled.Count - 1)
                    {
                        goto next;
                    }
                }

                if (maxComplete < pooled.Count + 1)
                {
                    maxComplete = pooled.Count + 1;
                    maxCompleteVertices.Clear();
                    maxCompleteVertices.AddRange(pooled);
                    maxCompleteVertices.Add(v);
                }
            next:;
            }
        }

        maxCompleteVertices.Sort();
        return string.Join(',', maxCompleteVertices);
    }

    [AnswerMethod(2024, 23, 2)]
    public static string Part2_parallel(string[] input)
    {
        var edges = new HashSet<(string, string)>();
        var vertices = new HashSet<string>();
        foreach (var line in input)
        {
            var spl = line.Split('-');
            var (s, d) = (spl[0], spl[1]);
            edges.Add((s, d));
            edges.Add((d, s));
            vertices.Add(s);
            vertices.Add(d);
        }

        var graph = Graph.Create(vertices, edges);
        var adj = graph.AdjacencyList;

        var maxCompleteVertices = new List<string>();
        Parallel.ForEach(adj, (kvp, state) =>
        {
            var pooled = new HashSet<string>(13);
            var (v, es) = kvp;
            var esLst = es.ToArray();
            var mask = 1 << es.Count;

            for (var i = 0; i < mask; i++)
            {
                if (BitOperations.PopCount((uint)i) + 1 <= maxCompleteVertices.Count)
                    continue;

                pooled.Clear();
                for (var j = 0; (1 << j) <= i; j++)
                {
                    if (((1 << j) & i) != 0)
                    {
                        pooled.Add(esLst[j]);
                    }
                }

                foreach (var v2 in pooled)
                {
                    var numContained = adj[v2].Intersect(pooled).Count();
                    if (numContained != pooled.Count - 1)
                    {
                        goto next;
                    }
                }

                if (maxCompleteVertices.Count < pooled.Count + 1)
                {
                    var prev = maxCompleteVertices;
                    var newMaxComplete = new List<string>(pooled) { v };
                    while ((prev = maxCompleteVertices).Count < newMaxComplete.Count &&
                            prev != Interlocked.CompareExchange(ref maxCompleteVertices, newMaxComplete, prev));
                }
            next:;
            }
        });

        maxCompleteVertices.Sort();
        return string.Join(',', maxCompleteVertices);
    }
}