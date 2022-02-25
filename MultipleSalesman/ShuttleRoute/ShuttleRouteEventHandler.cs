namespace MultipleSalesman
{
    internal class ShuttleRouteEventHandler<T> : IShuttleRouteEventHandler<T>
    {

        private Action<IWaypoint<T>[], double, int> onImprovedRouteFoundAction;

        public ShuttleRouteEventHandler(Action<IWaypoint<T>[], double, int>  onImprovedRouteFoundAction)
        {
            this.onImprovedRouteFoundAction = onImprovedRouteFoundAction;
        }

        public Task OnImprovedRouteFound(IWaypoint<T>[] route, double score, int iteration)
        {
            this.onImprovedRouteFoundAction.Invoke(route, score, iteration);
            return Task.CompletedTask;
        }
    }
}
