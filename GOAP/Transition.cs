namespace GOAP
{
    public class Transition<Model>
    {
        public Model State { get; }
        public float Cost { get; }

        public Transition(Model state, float cost)
        {
            this.State = state;
            this.Cost = cost;
        }        
    }

    public static class Transition
    {
        //Only for that juicy inference
        public static Transition<T> Create<T>(T state, float cost)
        {
            return new Transition<T>(state, cost);
        }
    }
}