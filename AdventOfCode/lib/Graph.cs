using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.lib
{
    internal static class Graph
    {
        public static Graph<T> Create<T>(IEnumerable<T> nodes, IEnumerable<(T, T)> edges)
            where T : notnull
        {
            return new Graph<T>(nodes, edges);
        }
    }

    struct Graph<T>
        where T : notnull
    {
        public readonly Dictionary<T, HashSet<T>> AdjacencyList;

        public Graph(IEnumerable<T> nodes, IEnumerable<(T, T)> edges)
        {
            AdjacencyList = new Dictionary<T, HashSet<T>>();
            foreach (var edge in edges)
            {
                ref var dests = ref CollectionsMarshal.GetValueRefOrAddDefault(AdjacencyList, edge.Item1, out var exists);
                if (!exists) dests = new();

                dests!.Add(edge.Item2);
            }

            foreach (var node in nodes)
            {
                ref var dests = ref CollectionsMarshal.GetValueRefOrAddDefault(AdjacencyList, node, out var exists);
                if (!exists) dests = new();
            }
        }

        public void DepthFirstSearch(T root, Action<T> preOrderVisit, Action<T> postOrderVisit)
        {
            var visited = new HashSet<T>();
            var adj = AdjacencyList;
            DFS(root);
            
            void DFS(T node)
            {
                visited.Add(node);

                preOrderVisit(node);

                foreach (var neighbor in adj[node])
                {
                    if (!visited.Contains(neighbor))
                        DFS(neighbor);
                }

                postOrderVisit(node);
            }
        }

        public List<T> TopologicalSort()
        {
            var ret = new List<T>(AdjacencyList.Count);

            var visited = new HashSet<T>();
            var adj = AdjacencyList;

            foreach (var (node, _) in AdjacencyList)
                if (!visited.Contains(node))
                    DFS(node);

            void DFS(T node)
            {
                visited.Add(node);

                foreach (var neighbor in adj[node])
                {
                    if (!visited.Contains(neighbor))
                        DFS(neighbor);
                }

                ret.Add(node);
            }

            ret.Reverse();
            return ret;
        }
    }
}
