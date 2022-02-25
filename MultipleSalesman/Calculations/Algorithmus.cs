namespace MultipleSalesman
{
    internal class Algorithmus
    {
        public Point[] Rechne(Point[] route, int plaetze, Point startingPoint)
        {
            var route2 = GetRouteWithReturnToStartingPoint(route, plaetze, startingPoint);



            return route2.ToArray();
        }

        private List<Point> GetRouteWithReturnToStartingPoint(Point[] route, int plaetze, Point startingPoint)
        {
            List<Point> routeWithStops = new List<Point>();

            for (int i = 0; i < route.Length; i++)
            {
                if (i % plaetze == 0)
                {
                    routeWithStops.Add(startingPoint);
                }

                routeWithStops.Add(route[i]);
            }
            routeWithStops.Add(startingPoint);

            return routeWithStops;
        }
    }
}