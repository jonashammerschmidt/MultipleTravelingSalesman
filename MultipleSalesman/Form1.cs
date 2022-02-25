using System.Diagnostics;

namespace MultipleSalesman
{
    public partial class Form1 : Form
    {
        private Point[] route = new Point[] {
            new Point(80, 120),
            new Point(150, 150),
            new Point(60, 220),
            new Point(590, 160),
            new Point(220, 420),
            new Point(170, 470),
            new Point(250, 450),
            new Point(330, 590),
            new Point(420, 600),
            new Point(350, 660),
            new Point(640, 300),
            new Point(620, 250),
            new Point(520, 60),
            new Point(10, 210),
            new Point(510, 510),
            new Point(610, 220),
            new Point(350, 610),
            new Point(220, 240),
            new Point(210, 340),
            new Point(520, 540),
            new Point(330, 550),
            new Point(240, 060),
            new Point(530, 460),
            new Point(460, 030),
            new Point(260, 520),
            new Point(250, 06),
        };

        private Point startingPoint = new Point(420, 340);

        private const int Plaetze = 5;

        private Image map;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();

            map = Image.FromFile("map.png");
            StartAlgorithmus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            this.StartAlgorithmus();
        }

        private void StartAlgorithmus()
        {
            IWaypoint<PointF>[] bestRoute = this.route.Select(waypoint => new PointWaypoint(waypoint.X, waypoint.Y)).ToArray();
            double bestScore = 1000000000;
            int bestIteration = 0;
            int lastIteration = 0;

            var cancellationTokenSource = new CancellationTokenSource();
            this.cancellationTokenSource = cancellationTokenSource;
            Task.Run(() => {
                Task.Delay(100);

                var shuttleRoute = new ShuttleRoute<PointF>();
                ShuttleRouteEventHandler<PointF> shuttleRouteEventHandler =
                    new ShuttleRouteEventHandler<PointF>((route, score, iteration) =>
                    {
                        bestRoute = route;
                        bestScore = score;
                        bestIteration = iteration;
                    });

                shuttleRoute.OptimizeAsync(
                    bestRoute,
                    new PointWaypoint(startingPoint.X, startingPoint.Y),
                    Plaetze,
                    shuttleRouteEventHandler,
                    cancellationTokenSource.Token);
            });

            Task.Run(async () =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                { 
                    await Task.Delay(100);
                    if (lastIteration < bestIteration)
                    {
                        this.RenderRoute(bestRoute, bestScore, bestIteration);
                        lastIteration = bestIteration;
                    }
                }
            });
        }

        private void RenderRoute(IWaypoint<PointF>[] route, double score, int iteration)
        {
            Pen pen = new Pen(Color.Black);
            pen.Width = 3;
            Pen penBigger = new Pen(Color.Black);
            penBigger.Width = 5;
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            int trip = 0;
            using (Graphics myCanvas = pictureBox1.CreateGraphics())
            {
                myCanvas.DrawImage(map, 0 , 0);
                for (int i = 0; i < route.Length; i++)
                {
                    PointF point = route[i].GetValue();

                    if (i < route.Length - 1)
                    {
                        PointF nextPoint = route[i + 1].GetValue();

                        if (point == startingPoint)
                        {
                            pen.Color = ColorScale.ColorFromHSL(new Random(trip+=5).NextSingle(), 1, 0.5);
                        }

                        myCanvas.DrawLine(penBigger, point.X, point.Y, nextPoint.X, nextPoint.Y);
                        myCanvas.DrawLine(pen, point.X, point.Y, nextPoint.X, nextPoint.Y);
                    }

                    myCanvas.FillEllipse(whiteBrush, point.X - 10, point.Y - 10, 20, 20);
                    myCanvas.DrawString(i.ToString(), new Font("Comic Sans MS", 14, FontStyle.Bold), blackBrush, point.X - 10, point.Y - 10 - 4);
                }

                myCanvas.DrawString(score.ToString() + " in Iteration: " + iteration, new Font("Comic Sans MS", 14, FontStyle.Bold), blackBrush, 10, 700);
            }
        }
    }
}