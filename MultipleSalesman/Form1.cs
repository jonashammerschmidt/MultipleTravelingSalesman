namespace MultipleSalesman
{
    public partial class Form1 : Form
    {

        Point[] route = new Point[] {
            new Point(50, 10),
            new Point(170, 50),
            new Point(250, 150),
            new Point(50, 210),
            new Point(180, 130),
        };

        public Form1()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Algorithmus algorithmus = new Algorithmus();
                var bessereRoute = algorithmus.Rechne(route);
                this.RenderRoute(bessereRoute);
            });
        }


        private void RenderRoute(Point[] route)
        {
            Pen whitePen = new Pen(Color.White);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

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
                    myCanvas.FillEllipse(whiteBrush, point.X - 5 + offsetX, point.Y - 5 + offsetY, 10, 10);

                    if (i < route.Length - 1)
                    {
                        Point nextPoint = route[i + 1];
                        myCanvas.DrawLine(whitePen, point.X + offsetX, point.Y + offsetY, nextPoint.X + offsetX, nextPoint.Y + offsetY);
                    }
                }
            }
        }
    }
}