using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.SKCharts;
using ShuttleRouting;
using ShuttleRouting.Benchmarks;

XY[] Route = new XY[] {
    new XY(80, 120),
    new XY(150, 150),
    new XY(60, 220),
    new XY(590, 160),
    new XY(220, 420),
    new XY(170, 470),
    new XY(250, 450),
    new XY(330, 590),
    new XY(420, 600),
    new XY(350, 660),
    new XY(640, 300),
    new XY(620, 250),
    new XY(520, 60),
    new XY(10, 210),
    new XY(510, 510),
    new XY(610, 220),
    new XY(350, 610),
    new XY(220, 240),
    new XY(210, 340),
    new XY(520, 540),
    new XY(330, 550),
    new XY(240, 060),
    new XY(530, 460),
    new XY(460, 030),
    new XY(260, 520),
    new XY(250, 06),
};

XY StartingPoint = new XY(420, 340);

const int Plaetze = 5;

List<ObservablePoint> observablePoints = new List<ObservablePoint>();

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
Task.Run(() =>
{
    var shuttleRoute = new ShuttleRouter<XY>();
    ShuttleRouterEventHandler<XY> shuttleRouteEventHandler =
        new ShuttleRouterEventHandler<XY>((route, score, iteration) =>
        {
            Console.WriteLine($"New best score {score} in iteration {iteration}");
            observablePoints.Add(new ObservablePoint(iteration, (int)score));
        });

    shuttleRoute.OptimizeAsync(
        Route.Select(waypoint => new PointWaypoint(waypoint.X, waypoint.Y)).ToArray(),
        new PointWaypoint(StartingPoint.X, StartingPoint.Y),
        Plaetze,
        shuttleRouteEventHandler,
        cancellationTokenSource.Token);
});

Console.ReadLine();
cancellationTokenSource.Cancel();
Console.WriteLine();
Console.WriteLine();
Task.Delay(100);


var cartesianChart = new SKCartesianChart
{
    Width = 900,
    Height = 600,
    Series = new ISeries[]
    {
        new LineSeries<ObservablePoint> { Values = observablePoints.ToArray() },
    }
};

// you can save the image to png (by default)
// or use the second argument to specify another format.
cartesianChart.SaveImage("cartesianChart-" + DateTime.Now.ToFileTime() + ".png");

// alternatively you can get the image and do different operations:
using var image = cartesianChart.GetImage();
using var data = image.Encode();
var base64CartesianChart = Convert.ToBase64String(data.AsSpan());

Console.WriteLine("Images saved at the root folder!");