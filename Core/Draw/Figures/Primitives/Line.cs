using System.Drawing;
using Core.Draw.Figures.Interfaces;
using SFML.Graphics;
using SFML.System;
using Color = SFML.Graphics.Color;

namespace Core.Draw.Figures.Primitives
{
    public class Line : IFigure
    {
        public Vector2f Start { get; set; }
        public Vector2f End { get; set; }

        public Color FillColor { get; init; }

        public float StrokeWidth { get; init; }

        public Line() { }

        public Line(Vector2f start, Vector2f end, Color fillColor)
        {
            Start = start;
            End = end;
            FillColor = fillColor;
        }

        public Vertex[] ToVertex()
        {
            return new[] {new Vertex(Start, FillColor), new Vertex(End, FillColor)};
        }

        public PrimitiveType PrimitiveType => PrimitiveType.Lines;
    }
}