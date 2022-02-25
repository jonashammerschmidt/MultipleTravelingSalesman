namespace MultipleSalesman
{
    public partial class Form1 : Form
    {
        private Point[] route = new Point[] {
            new Point(80, 120),
            new Point(150, 150),
            new Point(60, 220),
            new Point(140, 350),
            new Point(220, 420),
            new Point(170, 470),
            new Point(250, 450),
            new Point(330, 590),
            new Point(420, 600),
            new Point(350, 660),
            new Point(640, 300),
            new Point(620, 250),
            new Point(520, 60),
        };

        private Point startingPoint = new Point(420, 340);

        private const int Plaetze = 5;

        Image map;

        private int iteration = 0;
        private int bestIteration = 0;

        public Form1()
        {
            InitializeComponent();

            map = Image.FromFile("map.png");

            Algorithmus algorithmus = new Algorithmus();

            var besteRoute = algorithmus.Rechne(route, Plaetze, startingPoint);
            Task.Run(async () =>
            {
                await Task.Delay(100);
                while (true)
                {
                    if (iteration > 1000000) {
                        iteration = 0;
                        bestIteration = 0;
                        besteRoute = algorithmus.Rechne(route, Plaetze, startingPoint);
                    }

                    //await Task.Delay(100);
                    Algorithmus2 algorithmus2 = new Algorithmus2();
                    var bessereRoute = algorithmus2.Rechne(besteRoute, Plaetze);
                    iteration++;
                    if (GraphHelper.CalculateScore(bessereRoute) < GraphHelper.CalculateScore(besteRoute))
                    {
                        besteRoute = bessereRoute;
                        bestIteration = iteration;
                        this.RenderRoute(besteRoute, Plaetze);
                    }
                }
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Iteration: " + iteration.ToString());
                }
            });
        }


        private void RenderRoute(Point[] route, int plaetze)
        {
            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            using (Graphics myCanvas = pictureBox1.CreateGraphics())
            {
                myCanvas.DrawImage(map, 0 , 0);
                for (int i = 0; i < route.Length; i++)
                {
                    Point point = route[i];

                    if (i < route.Length - 1)
                    {
                        Point nextPoint = route[i + 1];
                        blackPen.Color = ColorScale.ColorFromHSL((i / (plaetze + 1)) * 0.3f, 1, 0.5);
                        myCanvas.DrawLine(blackPen, point.X, point.Y, nextPoint.X, nextPoint.Y);
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