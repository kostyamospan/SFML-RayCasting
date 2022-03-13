using System;
using Core.Abstract.Overlay;
using Core.Abstract.Windows;
using SFML.Graphics;

namespace Engine.Overlays.Debug
{
    public class DebugOverlay : Overlay
    {
        private const string ConsoleFontPath = "./StaticFiles/Fonts/ARCADECLASSIC.TTF";

        private Font _consoleFont;

        public Color FontColor { get; init; }

        public DebugOverlay(Window window) : base(window)
        {
        }

        public override void LoadContent()
        {
            _consoleFont = new Font(ConsoleFontPath);
        }

        public override void Draw()
        {
            if (_consoleFont is null) throw new InvalidOperationException("LoadContent method was`nt called");

            var fps = 1f / TargetWindow.GameTime.DeltaTime;
            var fpsString = fps.ToString("0");

            var displayFpsText = new Text(fpsString, _consoleFont, 14)
            {
                Position = new(0, 0),
                FillColor = FontColor
            };

            TargetWindow.RenderWindow.Draw(displayFpsText);
        }
    }
}