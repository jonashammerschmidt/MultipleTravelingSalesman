using ShuttleRouting;

namespace MultipleSalesman
{
    internal class ShuttleRouterEventHandler<T> : IShuttleRouterEventHandler<T>
    {
        private Action<Waypoint<T>[], double, int> onImprovedRouteFoundAction;

        public ShuttleRouterEventHandler(Action<Waypoint<T>[], double, int> onImprovedRouteFoundAction)
        {
            this.onImprovedRouteFoundAction = onImprovedRouteFoundAction;
        }

        public Task OnImprovedRouteFound(Waypoint<T>[] route, double score, int iteration)
        {
            this.onImprovedRouteFoundAction.Invoke(route, score, iteration);
            return Task.CompletedTask;
        }
    }
}