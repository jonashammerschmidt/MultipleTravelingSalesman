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

        Image map;

        private int iteration = 0;
        private int bestIteration = 0;

        private bool isCancled = false;

        public Form1()
        {
            InitializeComponent();

            map = Image.FromFile("map.png");
            StartAlgorithmus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            isCancled = true;
            Thread.Sleep(1000);
            isCancled = false;
            iteration = 0;
            bestIteration = 0;
            this.StartAlgorithmus();
        }

        private void StartAlgorithmus()
        {
            Algorithmus algorithmus = new Algorithmus();

            var besteRoute = algorithmus.Rechne(route, Plaetze, startingPoint);
            this.RenderRoute(besteRoute, Plaetze);
            Task.Run(async () =>
            {
                await Task.Delay(100);
                while (!isCancled)
                {
                    if (iteration % 800 == 0)
                    {
                        await Task.Delay(100);
                        this.RenderRoute(besteRoute, Plaetze);
                    }
                    Algorithmus2 algorithmus2 = new Algorithmus2();
                    var bessereRoute = algorithmus2.Rechne(besteRoute, Plaetze);
                    iteration++;
                    if (GraphHelper.CalculateScore(bessereRoute) < GraphHelper.CalculateScore(besteRoute))
                    {
                        besteRoute = bessereRoute;
                        bestIteration = iteration;
                    }
                }
            });

            //Task.Run(async () =>
            //{
            //    while (!isCancled)
            //    {
            //        await Task.Delay(500);
            //        this.RenderRoute(besteRoute, Plaetze);
            //    }
            //});
        }

        private void RenderRoute(Point[] route, int plaetze)
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
                    Point point = route[i];

                    if (i < route.Length - 1)
                    {
                        Point nextPoint = route[i + 1];

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

                myCanvas.DrawString(iteration.ToString(), new Font("Comic Sans MS", 14, FontStyle.Bold), blackBrush, 10, 680);
                myCanvas.DrawString(GraphHelper.CalculateScore(route).ToString() + " in Iteration: " + bestIteration, new Font("Comic Sans MS", 14, FontStyle.Bold), blackBrush, 10, 700);
            }
        }
    }
}