using System.Diagnostics;

namespace ShuttleRouting
{
    public class ShuttleRouter2<T> : IShuttleRouter<T>
    {
        private const double CostPower = 4d;
        private const double PheremonePower = 1d;
        private const double PheremoneIntensity = 500d;
        private const double InitialPheremoneIntensity = 1d;
        private const double EvaporationRate = 0.3d;
        private const double NumberOfAnts = 20;

        private readonly Random random = new Random();

        private double[,] CachedDistances = new double[0, 0];
        private double[,] Pheromones = new double[0, 0];

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

            CachedDistances = new double[destinations.Length, destinations.Length];
            Pheromones = new double[destinations.Length, destinations.Length];
            for (int i = 0; i < destinations.Length; i++)
            {
                var iValue = destinations[i].GetValue();
                destinations[i].Index = i;
                for (int j = 0; j < destinations.Length; j++)
                {
                    CachedDistances[i, j] = destinations[j].GetCost(iValue);
                    Pheromones[i, j] = InitialPheremoneIntensity;
                }
            }

            int iteration = 0;
            double bestScore = double.MaxValue;
            Waypoint<T>[] bestRoute = new Waypoint<T>[0];
            while (!cancellationToken.IsCancellationRequested)
            {
                iteration++;

                double localBestScore = double.MaxValue;
                Waypoint<T>[] localBestRoute = new Waypoint<T>[0];
                for (int i = 0; i < NumberOfAnts; i++)
                {
                    var antRoute = GetAntRoute(destinations, pickupLocation, capacity);
                    var antRouteScore = CalculateScore(antRoute);
                    if (antRouteScore < localBestScore)
                    {
                        localBestScore = antRouteScore;
                        localBestRoute = antRoute;
                    }
                }

                for (int i = 0; i < localBestRoute.Length - 1; i++)
                {
                    Pheromones[localBestRoute[i].Index, localBestRoute[i + 1].Index] *= PheremoneIntensity;
                    Pheromones[localBestRoute[i + 1].Index, localBestRoute[i].Index] *= PheremoneIntensity;
                }

                for (int i = 0; i < localBestRoute.Length; i++)
                {
                    for (int j = 0; j < localBestRoute.Length; j++)
                    {
                        Pheromones[localBestRoute[i].Index, localBestRoute[j].Index] *= EvaporationRate;
                    }
                }

                if (localBestScore < bestScore) 
                {
                    bestScore = localBestScore;
                    bestRoute = localBestRoute;
                    shuttleRouterEventHandler.OnImprovedRouteFound(bestRoute, bestScore, iteration);
                }

                if (iteration % 500 == 0)
                {
                    shuttleRouterEventHandler.OnImprovedRouteFound(bestRoute, bestScore, iteration);
                }
            }

            return Task.CompletedTask;
        }

        public Waypoint<T>[] GetAntRoute(
            Waypoint<T>[] destinations,
            Waypoint<T> pickupLocation,
            int capacity)
        {
            List<Waypoint<T>> waypoints = new List<Waypoint<T>>();
            waypoints.Add(pickupLocation);

            List<Waypoint<T>> remainingWaypoints = new List<Waypoint<T>>(destinations);

            int remainingCapacity = capacity;
            while (remainingWaypoints.Count > 0)
            {
                Waypoint<T>? nextWaypoint = this.GetNextWaypoint(waypoints.Last(), remainingWaypoints);
                if (remainingCapacity == 0)
                {
                    nextWaypoint = pickupLocation;
                    remainingCapacity = capacity + 1;
                }

                remainingWaypoints.Remove(nextWaypoint);
                waypoints.Add(nextWaypoint);

                remainingCapacity--;
            }

            waypoints.Add(pickupLocation);

            return waypoints.ToArray();
        }

        private Waypoint<T>? GetNextWaypoint(Waypoint<T> lastWaypoint, List<Waypoint<T>> remainingWaypoints)
        {
            foreach (var waypoint in remainingWaypoints)
            {
                GetDesirebility(lastWaypoint, waypoint);
            }

            int index = this.GetWeightedRandom(
                remainingWaypoints.Select(waypoint => GetDesirebility(lastWaypoint, waypoint)));

            return remainingWaypoints[index];
        }

        public double CalculateScore(Waypoint<T>[] route)
        {
            var score = 0d;
            for (int i = 0; i < route.Length - 1; i++)
            {
                score += GetCosts(route[i], route[i + 1]);
            }

            return score;
        }

        private double GetDesirebility(Waypoint<T> waypoint, Waypoint<T> waypoint2)
        {
            double cost = GetCosts(waypoint, waypoint2);
            double pheromoneStrength = GetPheromoneStrength(waypoint, waypoint2);
            return Math.Pow(1 / cost, CostPower) * Math.Pow(pheromoneStrength, PheremonePower);
        }

        private double GetCosts(Waypoint<T> waypoint1, Waypoint<T> waypoint2)
        {
            return CachedDistances[waypoint1.Index, waypoint2.Index];
        }

        private double GetPheromoneStrength(Waypoint<T> waypoint1, Waypoint<T> waypoint2)
        {
            return Pheromones[waypoint1.Index, waypoint2.Index];
        }

        private int GetWeightedRandom(IEnumerable<double> weights)
        {
            double totalWeight = weights.Sum();
            double randomNumber = this.random.NextDouble() * totalWeight;

            int weightsCount = weights.Count();
            for (int i = 0; i < weightsCount; i++)
            {
                randomNumber -= weights.ElementAt(i);
                if (randomNumber < 0)
                {
                    return i;
                }
            }

            return weights.Count() - 1;
        }
    }
}