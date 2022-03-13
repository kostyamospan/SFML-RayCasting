using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Abstract.Scenes;

namespace Engine.Scenes.Utils
{
    public class ScenesManager
    {
        public Scene CurrentScene { get; private set; }

        public ReadOnlyCollection<Scene> Scenes => _scenes.AsReadOnly();

        private readonly List<Scene> _scenes = new();

        public ScenesManager()
        {
        }

        public void AddScene(Scene scene)
        {
            _scenes.Add(scene);
        }

        public void AddSceneAndSetAsCurrent(Scene scene)
        {
            _scenes.Add(scene);
            SetCurrentScene(scene);
        }

        private void SetCurrentScene(Scene newCurrentScene)
        {
            CurrentScene = newCurrentScene;
        }
    }
}