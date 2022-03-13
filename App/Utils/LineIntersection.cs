using System;
using System.Drawing;
using Core.Draw.Figures.Primitives;
using SFML.System;

namespace App.Utils
{
    public class LineIntersection
    {
        public static bool TryGetIntersection(Line lineA, Line lineB, out Vector2f intersectionPoint,
            double tolerance = 0.001)
        {
            float x1 = lineA.Start.X, y1 = lineA.Start.Y;
            float x2 = lineA.End.X, y2 = lineA.End.Y;

            float x3 = lineB.Start.X, y3 = lineB.Start.Y;
            float x4 = lineB.End.X, y4 = lineB.End.Y;

            intersectionPoint = default;

            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
                return false;

            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
                return false;

            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
                return false;

            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
                return false;

            float x, y;


            if (Math.Abs(x1 - x2) < tolerance)
            {
                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;

                x = x1;
                y = c2 + m2 * x1;
            }
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;

                x = x3;
                y = c1 + m1 * x3;
            }
            else
            {
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;

                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;

                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                      && Math.Abs(-m2 * x + y - c2) < tolerance))
                    return false;
            }

            if (IsInsideLine(lineA, x, y) &&
                IsInsideLine(lineB, x, y))
            {
                intersectionPoint = new Vector2f(x, y);
                return true;
            }

            return false;
        }

        private static bool IsInsideLine(Line line, double x, double y)
        {
            return (x >= line.Start.X && x <= line.End.X
                    || x >= line.End.X && x <= line.Start.X)
                   && (y >= line.Start.Y && y <= line.End.Y
                       || y >= line.End.Y && y <= line.Start.Y);
        }
    }
}