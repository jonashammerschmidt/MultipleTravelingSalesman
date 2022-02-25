namespace MultipleSalesman
{
    internal interface IShuttleRouteEventHandler<T>
    {
        Task OnImprovedRouteFound(IWaypoint<T>[] route, double score, int iteration);
    }
}
