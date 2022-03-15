using Engine.Scenes.Abstract;
using Engine.Scenes.Utils;
using SFML.Graphics;

namespace App.Scenes
{
    public class IndexScene : ManagedScene
    {
        private const int START_SCENE_INDEX = 0;
        private const int RAY_CAST_SCENE_INDEX = 1;

        public IndexScene(RenderWindow renderWindow) : base(renderWindow, new())
        {
        }

        public override void Draw()
        {
            ScenesManager.CurrentScene.Draw();
        }

        public override void Initialize()
        {
            ScenesManager.AddSceneAndSetAsCurrent(START_SCENE_INDEX,
                new StartScreenScene(RenderWindow)
                {
                    OnNextSceneRequestCallback = () =>
                    {
                        ScenesManager.SetCurrent(RAY_CAST_SCENE_INDEX);
                        ScenesManager.CurrentScene.Initialize();
                    }
                });

            ScenesManager.AddScene(RAY_CAST_SCENE_INDEX,
                new RayCastSandbox(RenderWindow)
                {
                    OnPrevSceneRequestCallback = () =>
                    {
                        ScenesManager.SetCurrent(START_SCENE_INDEX);
                        ScenesManager.CurrentScene.Initialize();
                    }
                });

            ScenesManager.CurrentScene.Initialize();
        }
    }
}