using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using AdventOfCode.lib;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode
{
    internal static class Day16
    {
        [AnswerMethod(2024, 16, 1)]
        public static string Part1(string[] input)
        {
            var grid = input.CreateGrid();

            var start = grid.Where(kvp => kvp.Value == 'S').Single().Key;
            var end = grid.Where(kvp => kvp.Value == 'E').Single().Key;

            var frontier = new PriorityQueue<Vertex, long>();

            frontier.Enqueue(new Vertex(start, Direction.East), 0);
            var visited = new HashSet<Vertex>();
            Vertex? endVertex = default;
            var endCost = 0L;
            var prevList = new Dictionary<Vertex, Vertex>();

            while (frontier.TryDequeue(out var curr, out var cost))
            {
                visited.Add(curr!);
                if (curr.Position == end)
                {
                    endVertex = curr;
                    endCost = cost;
                    break;
                }

                var rightOfDir = (curr.Direction.Item2, -curr.Direction.Item1);
                var next = new Vertex(curr.Position, rightOfDir);
                if (!visited.Contains(next))
                {
                    if (frontier.Remove(next, out var existing, out var pri))
                    {
                        if (cost + 1000 < pri)
                        {
                            prevList[next] = curr;
                        }

                        frontier.Enqueue(next, Math.Min(cost + 1000, pri));
                    }
                    else
                    {
                        prevList[next] = curr;
                        frontier.Enqueue(next, cost + 1000);
                    }
                }

                var leftOfDir = (-curr.Direction.Item2, curr.Direction.Item1);
                next = new Vertex(curr.Position, leftOfDir);
                if (!visited.Contains(next))
                {
                    if (frontier.Remove(next, out var existing, out var pri))
                    {
                        if (cost + 1000 < pri)
                        {
                            prevList[next] = curr;
                        }

                        frontier.Enqueue(next, Math.Min(cost + 1000, pri));
                    }
                    else
                    {
                        prevList[next] = curr;
                        frontier.Enqueue(next, cost + 1000);
                    }
                }

                var forward = new Vertex(curr.Position.Plus(curr.Direction), curr.Direction);
                if (grid[forward.Position] != '#' && !visited.Contains(forward))
                {
                    if (frontier.Remove(forward, out var existing, out var pri))
                    {
                        if (cost + 1 < pri)
                        {
                            prevList[forward] = curr;
                        }

                        frontier.Enqueue(forward, Math.Min(cost + 1, pri));
                    }
                    else
                    {
                        prevList[forward] = curr;
                        frontier.Enqueue(forward, cost + 1);
                    }
                }
            }

            while (prevList.ContainsKey(endVertex!))
            {
                endVertex = prevList[endVertex!];
            }

            return endCost.ToString();
        }

        record class Vertex((int, int) Position, (int, int) Direction);

        [AnswerMethod(2024, 16, 2)]
        public static string Part2(string[] input)
        {
            var grid = input.CreateGrid();

            var start = grid.Where(kvp => kvp.Value == 'S').Single().Key;
            var end = grid.Where(kvp => kvp.Value == 'E').Single().Key;

            var frontier = new PriorityQueue<Vertex, long>();

            frontier.Enqueue(new Vertex(start, Direction.East), 0);
            var visited = new HashSet<Vertex>();
            Vertex? endVertex = default;
            var endCost = 0L;
            var prevList = new Dictionary<Vertex, HashSet<Vertex>>();

            while (frontier.Count > 0)
            {
                frontier.TryDequeue(out var curr, out var cost);
                visited.Add(curr!);
                if (curr!.Position == end)
                {
                    endVertex = curr;
                    endCost = cost;
                    break;
                }

                var rightOfDir = (curr.Direction.Item2, -curr.Direction.Item1);
                var next = new Vertex(curr.Position, rightOfDir);
                if (!visited.Contains(next))
                {
                    if (frontier.Remove(next, out var existing, out var pri))
                    {
                        if (cost + 1000 < pri)
                        {
                            prevList[next] = [curr];
                        }
                        else if (cost + 1000 == pri)
                        {
                            prevList[next].Add(curr);
                        }

                        frontier.Enqueue(next, Math.Min(cost + 1000, pri));
                    }
                    else
                    {
                        prevList[next] = new HashSet<Vertex>();
                        prevList[next].Add(curr);
                        frontier.Enqueue(next, cost + 1000);
                    }
                }

                var leftOfDir = (-curr.Direction.Item2, curr.Direction.Item1);
                next = new Vertex(curr.Position, leftOfDir);
                if (!visited.Contains(next))
                {
                    if (frontier.Remove(next, out var existing, out var pri))
                    {
                        if (cost + 1000 < pri)
                        {
                            prevList[next] = new HashSet<Vertex>();
                            prevList[next].Add(curr);
                        }
                        else if (cost + 1000 == pri)
                        {
                            prevList[next].Add(curr);
                        }

                        frontier.Enqueue(next, Math.Min(cost + 1000, pri));
                    }
                    else
                    {
                        prevList[next] = [curr];
                        frontier.Enqueue(next, cost + 1000);
                    }
                }

                var forward = new Vertex(curr.Position.Plus(curr.Direction), curr.Direction);
                if (grid[forward.Position] != '#' && !visited.Contains(forward))
                {
                    if (frontier.Remove(forward, out var existing, out var pri))
                    {
                        if (cost + 1 < pri)
                        {
                            prevList[forward] = [curr];
                        }
                        else if (cost + 1 == pri)
                        {
                            prevList[forward].Add(curr);
                        }

                        frontier.Enqueue(forward, Math.Min(cost + 1, pri));
                    }
                    else
                    {
                        prevList[forward] = [curr];
                        frontier.Enqueue(forward, cost + 1);
                    }
                }
            }

            var paths = new HashSet<Vertex>();
            void DFS(Vertex v)
            {
                if (v == new Vertex(start, Direction.East)) return;
                //Console.WriteLine(v);
                paths.Add(v);
                foreach (var n in prevList[v])
                {
                    if (!paths.Contains(n))
                    {
                        DFS(n);
                    }
                }
            }

            DFS(endVertex!);

            return paths.DistinctBy(x => x.Position).Count().ToString();
        }

        [AnswerMethod(2024, 16, 2)]
        public static string Part2_clean(string[] input)
        {
            var grid = input.CreateGrid();

            var start = grid.Where(kvp => kvp.Value == 'S').Single().Key;
            var end = grid.Where(kvp => kvp.Value == 'E').Single().Key;

            var frontier = new PriorityQueue<Vertex2, long>();

            frontier.Enqueue(new (new (start, Direction.East), null), 0);
            Vertex2? endVertex = default;
            var prevList = new Dictionary<Vertex, (long, HashSet<Vertex>)>();

            while (frontier.TryDequeue(out var curr, out var cost))
            {
                ref var prevListEntry = ref prevList.GetValueRefOrAddDefault(curr!.v, out var exists);
                if (exists)
                {
                    if (cost == prevListEntry.Item1 && curr.prev != null)
                    {
                        prevListEntry.Item2.Add(curr.prev);
                    }

                    continue;
                }
                else
                {
                    prevListEntry = (cost, curr.prev != null ? [curr.prev] : []);
                }

                if (curr.v.Position == end)
                {
                    endVertex = curr;
                    break;
                }

                UpdateNeighbor(new (new (curr.v.Position, curr.v.Direction.TurnRight()), curr.v), cost + 1000);
                UpdateNeighbor(new(new(curr.v.Position, curr.v.Direction.TurnLeft()), curr.v), cost + 1000);
                UpdateNeighbor(new(new(curr.v.Position.Plus(curr.v.Direction), curr.v.Direction), curr.v), cost + 1);

                void UpdateNeighbor(Vertex2 n, long cost)
                {
                    if (grid[n.v.Position] == '#') return;
                    frontier.Enqueue(n, cost);
                }
            }

            var paths = new HashSet<Vertex>();
            void DFS(Vertex v)
            {
                if (v == new Vertex(start, Direction.East)) return;

                paths.Add(v);
                foreach (var n in prevList[v].Item2)
                {
                    if (!paths.Contains(n))
                    {
                        DFS(n);
                    }
                }
            }

            DFS(endVertex!.v);

            return paths.DistinctBy(x => x.Position).Count().ToString();
        }

        record class Vertex2(Vertex v, Vertex? prev = null);
    }
}