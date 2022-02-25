namespace MultipleSalesman
{
    internal class ShuttleRoute
    {
        private const int SwappingTimes = 3;

        private readonly Random random = new Random();

        public Task OptimizeAsync(
            Point[] destinations,
            Point pickupLocation,
            int capacity,
            IShuttleRouteOptimizationNotifier shuttleRouteOptimizationNotifier,
            CancellationToken cancellationToken)
        {
            Point[] route = GetRouteWithPickupLocations(destinations, pickupLocation, capacity);
            Point[] bestRoute = route;
            int iteration = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                iteration++;
                var swappedRoute = SwapWaypoints(bestRoute, capacity);
                if (GraphHelper.CalculateScore(swappedRoute) < GraphHelper.CalculateScore(bestRoute))
                {
                    bestRoute = swappedRoute;
                    shuttleRouteOptimizationNotifier.OnImprovedRouteFound(bestRoute, iteration);
                }
            }

            return Task.CompletedTask;
        }

        private Point[] SwapWaypoints(Point[] route, int capacity)
        {
            Point[] routeCopy = new Point[route.Length];
            Array.Copy(route, routeCopy, route.Length);

            for (int i = 0; i < SwappingTimes; i++)
            {
                SwapWaypoint(routeCopy, capacity);
            }

            return routeCopy;
        }

        private void SwapWaypoint(Point[] route, int capacity)
        {
            int swapIndex1, swapIndex2;
            do
            {
                swapIndex1 = this.random.Next() % route.Length;
                swapIndex2 = this.random.Next() % route.Length;
            } while (!IsWaypointSwapValid(route, capacity, swapIndex1, swapIndex2));

            Point tempWaypoint = route[swapIndex2];
            route[swapIndex2] = route[swapIndex1];
            route[swapIndex1] = tempWaypoint;
        }

        private bool IsWaypointSwapValid(Point[] route, int capacity, int swapIndex1, int swapIndex2)
        {
            if (swapIndex1 == 0 || 
                swapIndex2 == 0 || 
                swapIndex1 == route.Length - 1 || 
                swapIndex2 == route.Length - 1)
            {
                return false;
            }

            Point pickupLocation = route[0];

            int occupiedCapacity = 0;
            for (int i = 0; i < route.Length; i++)
            {
                int waypointIndexAfterSwap = i;

                if (i == swapIndex1)
                {
                    waypointIndexAfterSwap = swapIndex2;
                }
                else if (i == swapIndex2)
                {
                    waypointIndexAfterSwap = swapIndex1;
                }

                if (route[waypointIndexAfterSwap].X != pickupLocation.X || route[waypointIndexAfterSwap].Y != pickupLocation.Y)
                {
                    occupiedCapacity++;

                    if (occupiedCapacity > capacity)
                    {
                        return false;
                    }
                }
                else
                {
                    occupiedCapacity = 0;
                }
            }

            return true;
        }


        private Point[] GetRouteWithPickupLocations(Point[] destinations, Point pickupLocation, int capacity)
        {
            List<Point> route = new List<Point>();

            for (int i = 0; i < destinations.Length; i++)
            {
                if (i % capacity == 0)
                {
                    route.Add(pickupLocation);
                }

                route.Add(destinations[i]);
            }

            route.Add(pickupLocation);

            return route.ToArray();
        }
    }
}