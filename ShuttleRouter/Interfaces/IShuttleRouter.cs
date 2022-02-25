namespace ShuttleRouting
{
    public interface IShuttleRouter<T>
    {
        Task OptimizeAsync(IWaypoint<T>[] destinations, IWaypoint<T> pickupLocation, int capacity, IShuttleRouterEventHandler<T> shuttleRouteEventHandler, CancellationToken cancellationToken);
        
        Task OptimizeParallelAsync(IWaypoint<T>[] destinations, IWaypoint<T> pickupLocation, int capacity, IShuttleRouterEventHandler<T> shuttleRouterEventHandler, CancellationToken cancellationToken, int threads);
    }
}