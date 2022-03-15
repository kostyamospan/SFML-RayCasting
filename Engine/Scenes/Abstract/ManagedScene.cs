using System;
using Core.Abstract.Scenes;
using Engine.Scenes.Utils;
using SFML.Graphics;

namespace Engine.Scenes.Abstract
{
    public abstract class ManagedScene : Scene
    {
        public delegate void Callback();

        public Callback OnNextSceneRequestCallback { get; init; }

        public Callback OnPrevSceneRequestCallback { get; init; }

        protected readonly ScenesManager ScenesManager;

        protected ManagedScene(
            RenderWindow renderWindow,
            ScenesManager scenesManager
        ) : base(renderWindow)
        {
            ScenesManager = scenesManager;
        }
    }
}