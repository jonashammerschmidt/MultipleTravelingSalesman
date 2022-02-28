namespace ShuttleRouting
{
    public abstract class Waypoint<T>
    {
        public int Index { get; set; }

        public abstract double GetCost(T waypoint);

        public abstract bool AreEqual(T waypoint);

        public abstract T GetValue();
    }
}
