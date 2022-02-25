namespace MultipleSalesman
{
    public partial class Form1 : Form
    {

        Point[] route = new Point[] {
            new Point(170, 50),
            new Point(270, 180),
            new Point(160, 10),
            new Point(50, 10),
            new Point(250, 130),
            new Point(180, 30),
            new Point(50, 210),
        };

        public Form1()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Algorithmus algorithmus = new Algorithmus();
                var bessereRoute = algorithmus.Rechne(route, 3, new Point(150, 150));
                this.RenderRoute(bessereRoute);
            });
        }


        private void RenderRoute(Point[] route)
        {
            Pen whitePen = new Pen(Color.White);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            var centerX = route.Average(point => point.X);
            var centerY = route.Average(point => point.Y);

            int offsetX = (int)(pictureBox1.Size.Width / 2 - centerX);
            int offsetY = (int)(pictureBox1.Size.Height / 2 - centerY);

            using (Graphics myCanvas = pictureBox1.CreateGraphics())
            {
                myCanvas.Clear(Color.Black);
                for (int i = 0; i < route.Length; i++)
                {
                    Point point = route[i];

                    if (i < route.Length - 1)
                    {
                        Point nextPoint = route[i + 1];
                        whitePen.Color = ColorScale.ColorFromHSL((float)i / route.Length, 1, 0.5);
                        myCanvas.DrawLine(whitePen, point.X + offsetX, point.Y + offsetY, nextPoint.X + offsetX, nextPoint.Y + offsetY);
                    }

                    myCanvas.FillEllipse(whiteBrush, point.X - 10 + offsetX, point.Y - 10 + offsetY, 20, 20);
                    myCanvas.DrawString(i.ToString(), new Font("Comic Sans MS", 14, FontStyle.Bold), blackBrush, point.X - 10 + offsetX, point.Y - 10 - 4 + offsetY);
                }
            }
        }
    }
}