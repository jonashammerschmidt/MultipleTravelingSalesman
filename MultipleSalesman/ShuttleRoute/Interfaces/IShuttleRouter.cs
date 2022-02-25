
namespace MultipleSalesman
{
    internal interface IShuttleRouter<T>
    {
        Task OptimizeAsync(IWaypoint<T>[] destinations, IWaypoint<T> pickupLocation, int capacity, IShuttleRouterEventHandler<T> shuttleRouteEventHandler, CancellationToken cancellationToken);
    }
}