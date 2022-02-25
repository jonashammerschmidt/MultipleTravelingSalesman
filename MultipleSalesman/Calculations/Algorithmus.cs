namespace MultipleSalesman
{
    internal class Algorithmus
    {
        private readonly Random random = new Random();

        private Point[] routeCopy = new Point[0];
        
        public Point[] Rechne(Point[] route, int plaetze, Point startingPoint)
        {
            Point[] route2 = GraphHelper.GetRouteWithReturnToStartingPoint(route, plaetze, startingPoint);
            routeCopy = new Point[route2.Length];

            for (int i = 0; i < 10000; i++)
            {
                route2 = SwapRoutePoints(route2, plaetze);
            }

            return route2.ToArray();
        }

        private Point[] SwapRoutePoints(Point[] route, int plaetze)
        {
            int swapIndex1, swapIndex2;
            do
            {
                swapIndex1 = this.random.Next() % route.Length;
                swapIndex2 = this.random.Next() % route.Length;
            } while (
                swapIndex1 % (plaetze + 1) == 0 ||
                swapIndex2 % (plaetze + 1) == 0 ||
                swapIndex1 == route.Length - 1 ||
                swapIndex2 == route.Length - 1);

            Array.Copy(route, routeCopy, route.Length);
            var tmp = routeCopy[swapIndex2];
            routeCopy[swapIndex2] = routeCopy[swapIndex1];
            routeCopy[swapIndex1] = tmp;

            return (GraphHelper.CalculateScore(routeCopy) < GraphHelper.CalculateScore(route)) ? routeCopy: route;
        }

    }
}