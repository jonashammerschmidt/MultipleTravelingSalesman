namespace ShuttleRouting
{
    public interface IShuttleRouter<T>
    {
        Task OptimizeAsync(Waypoint<T>[] destinations, Waypoint<T> pickupLocation, int capacity, IShuttleRouterEventHandler<T> shuttleRouteEventHandler, CancellationToken cancellationToken);
        
        Task OptimizeParallelAsync(Waypoint<T>[] destinations, Waypoint<T> pickupLocation, int capacity, IShuttleRouterEventHandler<T> shuttleRouterEventHandler, CancellationToken cancellationToken, int threads);
    }
}