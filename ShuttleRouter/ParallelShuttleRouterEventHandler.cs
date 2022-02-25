namespace ShuttleRouting
{
    public class ParallelShuttleRouterEventHandler<T> : IShuttleRouterEventHandler<T>
    {
        private IShuttleRouterEventHandler<T> shuttleRouterEventHandler;

        private double? bestScore;

        public ParallelShuttleRouterEventHandler(IShuttleRouterEventHandler<T> shuttleRouterEventHandler)
        {
            this.shuttleRouterEventHandler = shuttleRouterEventHandler;
        }

        public Task OnImprovedRouteFound(IWaypoint<T>[] route, double score, int iteration)
        {
            if (!bestScore.HasValue || score < bestScore)
            {
                this.bestScore = score;
                shuttleRouterEventHandler.OnImprovedRouteFound(route, score, iteration);
            }

            return Task.CompletedTask;
        }
    }
}