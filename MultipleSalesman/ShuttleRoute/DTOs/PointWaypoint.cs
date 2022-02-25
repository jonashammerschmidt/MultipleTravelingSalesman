namespace MultipleSalesman
{
    internal class PointWaypoint : IWaypoint<PointF>
    {
        private PointF location;

        public PointWaypoint(float x, float y)
        {
            this.location = new PointF(x, y);
        }

        public double GetCost(PointF otherWaypointValue)
        {
            return Math.Sqrt(Math.Pow((this.location.X - otherWaypointValue.X), 2) + Math.Pow((this.location.Y - otherWaypointValue.Y), 2));
        }

        public bool AreEqual(PointF otherWaypointValue)
        {
            return location.X == otherWaypointValue.X && location.Y == otherWaypointValue.Y;
        }

        public PointF GetValue()
        {
            return location;
        }
    }
}
