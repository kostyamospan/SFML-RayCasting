using SFML.Graphics;

namespace Core.Draw.Figures.Interfaces
{
    public interface IFigure
    {
        Vertex[] ToVertex();
        PrimitiveType PrimitiveType { get; }
    }
}