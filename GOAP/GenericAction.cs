using System;

namespace GOAP
{
    /// <summary>
    /// Generic Action where it's behaviour can be injected through delegates and "User Data" attached as well.
    /// </summary>
    /// <typeparam name="UserData">User data type.</typeparam>
    /// <typeparam name="Model">World model type.</typeparam>
    public class GenericAction<UserData, Model> : IAction<Model>
    {
        /// <summary>
        /// Useful attached information.
        /// </summary>
        public UserData userData;

        private Func<Model, bool> _isValid;
        private Func<Model, Tuple<Model, float>> _apply;

        public GenericAction(UserData data, Func<Model, bool> isValid, Func<Model, Tuple<Model, float>> apply)
        {
            this.userData = data;
            _isValid = isValid;
            _apply = apply;
        }

        /// <summary>
        /// Apply this action on the given state.
        /// </summary>
        /// <param name="worldState">State to transform.</param>
        /// <returns>Item1 = Transformed state. Item2 = Transformation cost.</returns>
        public Tuple<Model, float> Apply(Model worldState)
        {
            return _apply(worldState);
        }

        /// <summary>
        /// Can the action be applied on the given state?
        /// </summary>
        /// <param name="state">State to validate.</param>
        /// <returns>True if is a valid state. Duh.</returns>
        public bool IsValid(Model worldState)
        {
            return _isValid(worldState);
        }
    }
}
