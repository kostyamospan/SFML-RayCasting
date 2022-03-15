using Core.Abstract.Scenes;
using Engine.Managers;
using Engine.Scenes.Abstract;
using SFML.Graphics;
using SFML.Window;

namespace App.Scenes
{
    public class StartScreenScene : ManagedScene
    {
        private const string FontPath = "./StaticFiles/Fonts/ARCADECLASSIC.TTF";

        private Font _textFont;

        public StartScreenScene(RenderWindow renderWindow) : base(renderWindow, new())
        {
        }

        public override void Draw()
        {
            const string text = "Press any button to start";
            const int fontSize = 14;

            var displayFpsText = new Text(text, _textFont, fontSize)
            {
                Position =
                    new((RenderWindow.Size.X - text.Length * fontSize) / 2 , (RenderWindow.Size.Y + fontSize) / 2),
                FillColor = Color.White
            };
            
            RenderWindow.Draw(displayFpsText);;
        }

        public override void Initialize()
        {
            _textFont = FontManager.LoadFontFromPath(FontPath);
            
            RenderWindow.KeyPressed += OnKeyboardKeyPressed;
        }

        private void OnKeyboardKeyPressed(object? sender, KeyEventArgs e)
        {
            OnNextSceneRequestCallback?.Invoke();
        }
    }
}