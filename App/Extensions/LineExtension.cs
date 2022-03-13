using System;
using App.Utils;
using Core.Draw.Figures.Primitives;
using SFML.System;

namespace App.Extensions
{
    public static class LineExtension
    {
        public static bool TryGetIntersectAt(this Line line1, Line line2, out Vector2f intersectionPoint)
            => LineIntersection.TryGetIntersection(line1, line2, out intersectionPoint);
    }
}