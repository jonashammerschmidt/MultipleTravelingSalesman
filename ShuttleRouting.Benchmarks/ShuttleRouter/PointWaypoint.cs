namespace ShuttleRouting.Benchmarks
{
    internal class PointWaypoint : IWaypoint<XY>
    {
        private XY location;

        public PointWaypoint(float x, float y)
        {
            location = new XY(x, y);
        }

        public double GetCost(XY otherWaypointValue)
        {
            return Math.Sqrt(Math.Pow(location.X - otherWaypointValue.X, 2) + Math.Pow(location.Y - otherWaypointValue.Y, 2));
        }

        public bool AreEqual(XY otherWaypointValue)
        {
            return location.X == otherWaypointValue.X && location.Y == otherWaypointValue.Y;
        }

        public XY GetValue()
        {
            return location;
        }
    }
}