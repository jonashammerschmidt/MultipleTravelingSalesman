﻿using System.Diagnostics;

namespace ShuttleRouting
{
    public class ShuttleRouter<T> : IShuttleRouter<T>
    {
        private const int SwappingTimes = 3;

        private readonly Random random = new Random();

        public Task OptimizeParallelAsync(
            Waypoint<T>[] destinations,
            Waypoint<T> pickupLocation,
            int capacity,
            IShuttleRouterEventHandler<T> shuttleRouterEventHandler,
            CancellationToken cancellationToken,
            int threads)
        {
            ParallelShuttleRouterEventHandler<T> paralellShuttleRouterEventHandler =
                new ParallelShuttleRouterEventHandler<T>(shuttleRouterEventHandler);

            for (int i = 0; i < threads; i++)
            {
                Task.Run(() => OptimizeAsync(destinations, pickupLocation, capacity, paralellShuttleRouterEventHandler, cancellationToken));
            }

            return Task.CompletedTask;
        }

        public Task OptimizeAsync(
            Waypoint<T>[] destinations,
            Waypoint<T> pickupLocation,
            int capacity,
            IShuttleRouterEventHandler<T> shuttleRouterEventHandler,
            CancellationToken cancellationToken)
        {
            destinations = destinations
                .Where(destination => !destination.AreEqual(pickupLocation.GetValue()))
                .ToArray();

            Waypoint<T>[] route = GetRouteWithPickupLocations(destinations, pickupLocation, capacity);
            Waypoint<T>[] bestRoute = route;
            double bestScore = CalculateScore(bestRoute);
            int iteration = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                iteration++;
                var swappedRoute = SwapWaypoints(bestRoute, capacity);
                double swappedRouteScore = CalculateScore(swappedRoute);

                if (swappedRouteScore < bestScore)
                {
                    bestRoute = swappedRoute;
                    bestScore = swappedRouteScore;
                    shuttleRouterEventHandler.OnImprovedRouteFound(bestRoute, bestScore, iteration);
                }
            }

            return Task.CompletedTask;
        }

        private Waypoint<T>[] SwapWaypoints(Waypoint<T>[] route, int capacity)
        {
            Waypoint<T>[] routeCopy = new Waypoint<T>[route.Length];
            Array.Copy(route, routeCopy, route.Length);

            for (int i = 0; i < SwappingTimes; i++)
            {
                SwapWaypoint(routeCopy, capacity);
            }

            return routeCopy;
        }

        private void SwapWaypoint(Waypoint<T>[] route, int capacity)
        {
            int swapIndex1, swapIndex2;
            do
            {
                swapIndex1 = this.random.Next() % route.Length;
                swapIndex2 = this.random.Next() % route.Length;
            } while (!IsWaypointSwapValid(route, capacity, swapIndex1, swapIndex2));

            Waypoint<T> tempWaypoint = route[swapIndex2];
            route[swapIndex2] = route[swapIndex1];
            route[swapIndex1] = tempWaypoint;
        }

        private bool IsWaypointSwapValid(Waypoint<T>[] route, int capacity, int swapIndex1, int swapIndex2)
        {
            if (swapIndex1 == 0 ||
                swapIndex2 == 0 ||
                swapIndex1 == route.Length - 1 ||
                swapIndex2 == route.Length - 1)
            {
                return false;
            }

            Waypoint<T> pickupLocation = route[0];

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

                if (!route[waypointIndexAfterSwap].AreEqual(pickupLocation.GetValue()))
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


        private Waypoint<T>[] GetRouteWithPickupLocations(Waypoint<T>[] destinations, Waypoint<T> pickupLocation, int capacity)
        {
            List<Waypoint<T>> route = new List<Waypoint<T>>();

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

        public static double CalculateScore(Waypoint<T>[] route)
        {
            var score = 0d;
            for (int i = 0; i < route.Length - 1; i++)
            {
                score += route[i].GetCost(route[i + 1].GetValue());
            }

            return score;
        }
    }
}