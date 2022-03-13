using System;
using Core.Abstract.Windows;
using SFML.Graphics;

namespace Core.Windows.Abstract
{
    public abstract class GameWindow : Window
    {
        
        protected GameWindow(uint width, uint height, string windowTitle, Color windowClearColor) : base(width, height,
            windowTitle, windowClearColor)
        {
        }
    

        protected  override void OnRenderFrame()
        {
            RenderWindow.Clear(WindowClearColor);

            FrameDraw();
            
            RenderWindow.Display();
        }

        protected virtual void FrameDraw() {}
    }
}
