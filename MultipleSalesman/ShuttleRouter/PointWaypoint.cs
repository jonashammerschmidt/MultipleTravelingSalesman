using ShuttleRouting;

namespace MultipleSalesman
{
    internal class PointWaypoint : Waypoint<PointF>
    {
        private PointF location;

        public PointWaypoint(float x, float y)
        {
            this.location = new PointF(x, y);
        }

        public override double GetCost(PointF otherWaypointValue)
        {
            return Math.Sqrt(Math.Pow((this.location.X - otherWaypointValue.X), 2) + Math.Pow((this.location.Y - otherWaypointValue.Y), 2));
        }

        public override bool AreEqual(PointF otherWaypointValue)
        {
            return location.X == otherWaypointValue.X && location.Y == otherWaypointValue.Y;
        }

        public override PointF GetValue()
        {
            return location;
        }
    }
}