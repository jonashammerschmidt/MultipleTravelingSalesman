
namespace MultipleSalesman
{
    internal interface IShuttleRoute<T>
    {
        Task OptimizeAsync(IWaypoint<T>[] destinations, IWaypoint<T> pickupLocation, int capacity, IShuttleRouteEventHandler<T> shuttleRouteEventHandler, CancellationToken cancellationToken);
    }
}