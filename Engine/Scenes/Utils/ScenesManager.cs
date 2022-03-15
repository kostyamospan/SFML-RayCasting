using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Abstract.Scenes;

namespace Engine.Scenes.Utils
{
    public class ScenesManager
    {
        public Scene CurrentScene { get; private set; }

        public ReadOnlyDictionary<uint, Scene> Scenes => new(_scenes);

        private readonly Dictionary<uint, Scene> _scenes = new();

        public ScenesManager()
        {
        }

        public void AddScene(uint sceneIndex, Scene scene)
        {
            AddScenePrivate(sceneIndex, scene);
        }

        public void AddSceneAndSetAsCurrent(uint sceneIndex, Scene scene)
        {
            AddScenePrivate(sceneIndex, scene);
            ChangeCurrentScene(sceneIndex);
        }

        public void SetCurrent(uint sceneIndex)
        {
            ChangeCurrentScene(sceneIndex);
        } 
            

        private void AddScenePrivate(uint sceneIndex, Scene scene)
        {
            if (_scenes.ContainsKey(sceneIndex))
                throw new InvalidOperationException("Scene under provided sceneIndex is already exist");
            
            _scenes.Add(sceneIndex, scene);
        }

        private void ChangeCurrentScene(uint sceneIndex)
        {
            CurrentScene = _scenes[sceneIndex];
        }
    }
}