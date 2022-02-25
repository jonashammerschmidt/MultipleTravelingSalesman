namespace MultipleSalesman
{
    internal interface IShuttleRouteOptimizationNotifier
    {
        void OnImprovedRouteFound(Point[] route, int iteration);
    }
}
