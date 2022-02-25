using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleSalesman
{
    internal class GraphHelper
    {

        public static double CalculateScore(Point[] route)
        {
            var score = 0d;
            for (int i = 0; i < route.Length - 1; i++)
            {
                score += GetDistance(route[i].X, route[i].Y, route[i + 1].X, route[i + 1].Y);
            }

            return score;
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }


        public static Point[] GetRouteWithReturnToStartingPoint(Point[] route, int plaetze, Point startingPoint)
        {
            List<Point> routeWithStops = new List<Point>();

            for (int i = 0; i < route.Length; i++)
            {
                if (i % plaetze == 0)
                {
                    routeWithStops.Add(startingPoint);
                }

                routeWithStops.Add(route[i]);
            }
            routeWithStops.Add(startingPoint);

            return routeWithStops.ToArray();
        }
    }
}
