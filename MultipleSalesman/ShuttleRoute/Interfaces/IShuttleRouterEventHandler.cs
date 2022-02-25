namespace MultipleSalesman
{
    internal interface IShuttleRouterEventHandler<T>
    {
        Task OnImprovedRouteFound(IWaypoint<T>[] route, double score, int iteration);
    }
}
