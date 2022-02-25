namespace ShuttleRouting
{
    public interface IShuttleRouterEventHandler<T>
    {
        Task OnImprovedRouteFound(IWaypoint<T>[] route, double score, int iteration);
    }
}
