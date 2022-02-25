namespace MultipleSalesman
{
    internal class Algorithmus
    {
        private readonly Random random = new Random();

        private Point[] routeCopy;

        public Point[] Rechne(Point[] route, int plaetze, Point startingPoint)
        {
            Point[] route2 = GetRouteWithReturnToStartingPoint(route, plaetze, startingPoint);
            routeCopy = new Point[route2.Length];

            for (int i = 0; i < 100000; i++)
            {
                route2 = Swap(route2, plaetze);
            }

            return route2.ToArray();
        }

        private Point[] Swap(Point[] route, int plaetze)
        {
            int i1, i2;
            do
            {
                i1 = this.random.Next() % route.Length;
                i2 = this.random.Next() % route.Length;
            } while (i1 % plaetze == 0 || i2 % plaetze == 0 || i1 == route.Length - 1 || i2 == route.Length - 1);

            Array.Copy(route, routeCopy, route.Length);
            var tmp = routeCopy[i2];
            routeCopy[i2] = routeCopy[i1];
            routeCopy[i1] = tmp;

            return (CalculateScore(routeCopy) < CalculateScore(route)) ? routeCopy: route;
        }

        private double CalculateScore(Point[] route)
        {
            var score = 0d;
            for (int i = 0; i < route.Length - 1; i++)
            {
                score += GetDistance(route[i].X, route[i].Y, route[i + 1].X, route[i + 1].Y);
            }

            return score;
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }


        private Point[] GetRouteWithReturnToStartingPoint(Point[] route, int plaetze, Point startingPoint)
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

            return routeWithStops.ToArray();
        }
    }
}