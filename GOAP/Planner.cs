using System;
using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public static class Planner<Model, TAction> where TAction : IAction<Model>
    {
        public static List<TAction> Plan
            (
                Model initialState,
                IEnumerable<TAction> actions,
                Func<Model, bool> satisfies,
                Func<Model, float> heuristic,
                Func<Model, Model> clone
            )
        {
            return LazyPlan(initialState, actions, satisfies, heuristic, clone)
                .Last()
                .Item2?
                .Select(t => t.Item2)
                .ToList();
        }

        public static IEnumerable<Tuple<Tuple<Model, TAction>, IEnumerable<Tuple<Model, TAction>>>> LazyPlan
            (
                Model initialState,
                IEnumerable<TAction> actions,
                Func<Model, bool> satisfies,
                Func<Model, float> heuristic,
                Func<Model, Model> clone
            )
        {
            Tuple<Model, TAction> start = Tuple.Create(initialState, default(TAction));
            Func<Tuple<Model, TAction>, bool> condition = tuple => satisfies(tuple.Item1);
            Func<Tuple<Model, TAction>, float> heur = tuple => heuristic(tuple.Item1);

            Func<Tuple<Model, TAction>, IEnumerable<Tuple<Tuple<Model, TAction>, float>>> explode = currentState =>
            {
                var nodes = new List<Tuple<Tuple<Model, TAction>, float>>();
                foreach (var action in actions)
                {
                    if (action.IsValid(currentState.Item1))
                    {
                        Tuple<Model, float> stateCost = action.Apply(clone(currentState.Item1));
                        nodes.Add(Tuple.Create(Tuple.Create(stateCost.Item1, action), stateCost.Item2));
                    }
                }
                return nodes;
            };

            return AStar.Run<Tuple<Model, TAction>>(start, condition, heur, explode);
        }

    }
}
