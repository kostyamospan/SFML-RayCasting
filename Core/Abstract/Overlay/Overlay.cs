using System;
using System.Collections.Generic;
using Core.Abstract.Windows;
using SFML.System;

namespace Core.Abstract.Overlay
{
    public abstract class Overlay
    {
        public Vector2f PositionOffset { get; protected set; }

        protected readonly Window TargetWindow;

        
        protected Overlay(Window window)
        {
            TargetWindow = window;
        }

        public abstract void LoadContent();
        public abstract void Draw();
    }
}