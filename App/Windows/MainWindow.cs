using System;
using System.Collections.Generic;
using System.Linq;
using App.Extensions;
using App.Scenes;
using Core.Abstract.Overlay;
using Core.Draw.Figures.Interfaces;
using Core.Draw.Figures.Primitives;
using Core.Windows.Abstract;
using Engine.Overlays.Debug;
using Engine.Overlays.Utils;
using Engine.Scenes.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace App.Windows
{
    public class MainWindow : GameWindow
    {
        private static Random _rnd = new();

        private readonly OverlayManager _overlayManager = new();

        private readonly ScenesManager _scenesManager = new();

        public MainWindow(uint width, uint height)
            : base(
                width,
                height,
                "Main window",
                new Color(0, 0, 0, 1))
        {
        }

        protected override void Initialize()
        {
            _overlayManager.AddOverlay(new DebugOverlay(this) {FontColor = Color.Red});
            _scenesManager.AddSceneAndSetAsCurrent(0, new IndexScene(RenderWindow));

            _overlayManager.LoadContentAll();

            _scenesManager.CurrentScene.Initialize();

            base.Initialize();
        }

        protected override void FrameDraw()
        {
            _scenesManager.CurrentScene.Draw();
            _overlayManager.DrawAll();

            base.FrameDraw();
        }
    }
}