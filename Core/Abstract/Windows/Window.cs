using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Core.Abstract.Windows
{
    public abstract class Window : IDisposable
    {
        private bool _disposed;
        
        public virtual uint TargetFps { get; init; } = 60;

        public float FrameTime => 1f / TargetFps;

        public RenderWindow RenderWindow { get; private set; }

        // public GameTime

        public Color WindowClearColor { get; protected set; }

        public GameTime GameTime { get; set; } = new();
        
        protected Window(uint width, uint height, string windowTitle, Color windowClearColor)
        {
            WindowClearColor = windowClearColor;
            RenderWindow = new RenderWindow(new VideoMode(width, height), windowTitle,  Styles.Close );
            
            RenderWindow.SetVerticalSyncEnabled(true);
            RenderWindow.SetFramerateLimit(TargetFps);
        }

        public void Run()
        {
            RenderWindow.ResetGLStates();
            RenderWindow.SetActive(true);
            RenderWindow.SetMouseCursor(new Cursor(Cursor.CursorType.Arrow));

            Initialize();


            float totalTimeBeforeUpdate = 0;
            float prevTimeElapsed = 0;
            float deltaTime;
            float totalTimeElapsed;

            var clock = new Clock();
            
            while (RenderWindow.IsOpen)
            {
                RenderWindow.DispatchEvents();

                totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                deltaTime = totalTimeElapsed - prevTimeElapsed;
                prevTimeElapsed = totalTimeElapsed;
                totalTimeBeforeUpdate += deltaTime;

                if (!(totalTimeElapsed >= FrameTime)) continue;
                
                GameTime.Update(totalTimeBeforeUpdate, totalTimeElapsed);
                totalTimeBeforeUpdate = 0;

                if(!ShouldRender()) continue;
                
                BeforeRenderFrame();
                OnRenderFrame();
            }
        }

        protected virtual void Initialize()
        {
            RenderWindow.Closed += ((sender, args) => RenderWindow.Close());
        }
        
        protected virtual bool ShouldRender() => true;
        protected virtual void BeforeRenderFrame() {}
        protected abstract void OnRenderFrame();
    
        public void Dispose()
        {
            if(_disposed) return;
            _disposed = true;
            RenderWindow?.Dispose();
        }
    }
}