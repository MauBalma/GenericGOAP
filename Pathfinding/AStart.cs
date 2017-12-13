using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{
    public static IEnumerable<Tuple<Node, IEnumerable<Node>>> Run<Node>
        (
            Node start,
            Func<Node, bool> satisfies,
            Func<Node, float> heuristic,
            Func<Node, IEnumerable<Tuple<Node, float>>> expand
        )
    {
        var open = new MinHeap<Node, float>();
        var closed = new HashSet<Node>();
        var previus = new Dictionary<Node, Node>();
        var gs = new Dictionary<Node, float>();

        open.Insert(start, 0);
        gs[start] = 0f;

        bool done = false;
        while (!done && !open.IsEmpty)
        {
            var candidate = open.Pop();

            if (satisfies(candidate))
            {
                done = true;
                yield return Tuple.Create<Node, IEnumerable<Node>>(
                    candidate,
                    EnumerableUtils.Generate(candidate, n => previus[n])
                        .TakeWhile(n => n != null && previus.ContainsKey(n))
                        .Reverse()
                    );
            }
            else
            {
                closed.Add(candidate);
                var transitions = expand(candidate);
                var gc = gs[candidate];
                foreach (var t in transitions)
                {
                    var node = t.Item1;
                    if (closed.Contains(node)) continue;

                    var gn = gc + t.Item2;
                    if (gs.ContainsKey(node) && gn >= gs[node]) continue;

                    previus[node] = candidate;
                    gs[node] = gn;
                    open.Insert(node, gn + heuristic(node));
                }
                yield return Tuple.Create<Node, IEnumerable<Node>>(candidate, null);
            }
        }
    }

}