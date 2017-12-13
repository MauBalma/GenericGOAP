using System;
using System.Collections.Generic;
using System.Linq;

public static class ThetaStar
{
    public static IEnumerable<Tuple<Node, IEnumerable<Node>>> Run<Node>
        (
            Node start,
            Func<Node, bool> satisfies,
            Func<Node, float> heuristic,
            Func<Node, Node, float> cost,
            Func<Node, IEnumerable<Tuple<Node, float>>> expand,
            Func<Node, Node, bool> inSight
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

                foreach (var t in transitions)
                {
                    var node = t.Item1;

                    if (closed.Contains(node)) continue;

                    var gn = gs.ContainsKey(node) ? gs[node] : float.MaxValue;

                    Node parent;
                    if (previus.TryGetValue(candidate, out parent) && inSight(parent, node))
                    {
                        var newDistParent = gs[parent] + cost(parent, node);
                        if (newDistParent < gn)
                        {
                            gs[node] = newDistParent;
                            previus[node] = parent;
                            open.Insert(node, newDistParent + heuristic(node));
                        }
                        continue;
                    }

                    var newDist = gs[candidate] + cost(candidate, node);
                    if (newDist < gn)
                    {
                        gs[node] = newDist;
                        previus[node] = candidate;
                        open.Insert(node, newDist + heuristic(node));
                        continue;
                    }

                }
                yield return Tuple.Create<Node, IEnumerable<Node>>(candidate, null);
            }
        }
    }
}
