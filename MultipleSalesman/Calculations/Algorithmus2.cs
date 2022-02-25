namespace MultipleSalesman
{
    internal class Algorithmus2
    {
        private readonly Random random = new Random();

        private Point[] routeCopy = new Point[0];
        
        public Point[] Rechne(Point[] route, int plaetze)
        {
            routeCopy = new Point[route.Length];

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
            } while (
                swapIndex1 % (plaetze + 1) == 0 ||
                swapIndex2 % (plaetze + 1) == 0 ||
                swapIndex1 == route.Length - 1 ||
                swapIndex2 == route.Length - 1);

            var tmp = route[swapIndex2];
            route[swapIndex2] = route[swapIndex1];
            route[swapIndex1] = tmp;

            return route;
        }

    }
}