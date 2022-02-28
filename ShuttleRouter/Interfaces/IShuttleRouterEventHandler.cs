namespace ShuttleRouting
{
    public interface IShuttleRouterEventHandler<T>
    {
        Task OnImprovedRouteFound(Waypoint<T>[] route, double score, int iteration);
    }
}
