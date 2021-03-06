﻿using System;

namespace GOAP
{
    /// <summary>
    /// Action interface to being applied by the planner.
    /// </summary>
    /// <typeparam name="Model">World model type.</typeparam>
    public interface IAction<Model>
    {
        /// <summary>
        /// Can the action be applied on the given state?
        /// </summary>
        /// <param name="state">State to validate.</param>
        /// <returns>True if is a valid state. Duh.</returns>
        bool IsValid(Model state);

        /// <summary>
        /// Apply this action on the given state.
        /// </summary>
        /// <param name="worldState">State to transform.</param>
        /// <returns>The transition that contains the altered state and the cost of change.</returns>
        Transition<Model> Apply(Model worldState);
    }
}
