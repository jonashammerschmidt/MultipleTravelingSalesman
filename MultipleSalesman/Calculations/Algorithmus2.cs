namespace MultipleSalesman
{
    internal class Algorithmus2
    {
        private readonly Random random = new Random();

        public Point[] Rechne(Point[] route, int plaetze)
        {
            Point[] routeCopy = new Point[route.Length];

            Array.Copy(route, routeCopy, route.Length);
            routeCopy = SwapRoutePoints(routeCopy, plaetze);
            routeCopy = SwapRoutePoints(routeCopy, plaetze);
            routeCopy = SwapRoutePoints(routeCopy, plaetze);

            return (GraphHelper.CalculateScore(routeCopy) < GraphHelper.CalculateScore(route)) ? routeCopy : route;
        }

        private Point[] SwapRoutePoints(Point[] route, int plaetze)
        {
            int swapIndex1, swapIndex2;
            do
            {
                swapIndex1 = this.random.Next() % route.Length;
                swapIndex2 = this.random.Next() % route.Length;
            } while (!IsValidSwap(route, plaetze, swapIndex1, swapIndex2));

            var tmp = route[swapIndex2];
            route[swapIndex2] = route[swapIndex1];
            route[swapIndex1] = tmp;

            return route;
        }

        private bool IsValidSwap(Point[] route, int plaetze, int swapIndex1, int swapIndex2)
        {
            if (swapIndex1 == 0 || swapIndex2 == 0 || swapIndex1 == route.Length - 1 || swapIndex2 == route.Length - 1)
            {
                return false;
            }

            Point startingPoint = route[0];

            Point[] routeCopy2 = new Point[route.Length];
            Array.Copy(route, routeCopy2, route.Length);
            var tmp = routeCopy2[swapIndex2];
            routeCopy2[swapIndex2] = routeCopy2[swapIndex1];
            routeCopy2[swapIndex1] = tmp;

            int usedPlaetze = 0;
            foreach (var stop in routeCopy2)
            {
                if (stop.X != startingPoint.X || stop.Y != startingPoint.Y)
                {
                    usedPlaetze++;

                    if (usedPlaetze > plaetze)
                    {
                        return false;
                    }
                }
                else
                {
                    usedPlaetze = 0;
                }
            }

            return true;
        }
    }
}