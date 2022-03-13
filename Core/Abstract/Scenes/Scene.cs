using SFML.Graphics;

namespace Core.Abstract.Scenes
{
    public abstract class Scene
    {
        protected readonly RenderWindow RenderWindow;

        protected Scene(RenderWindow renderWindow)
        {
            RenderWindow = renderWindow;
        }
        
        public abstract void Draw();
        public abstract void Initialize();
    }
}