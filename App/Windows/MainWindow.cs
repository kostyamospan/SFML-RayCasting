using System;
using System.Collections.Generic;
using System.Linq;
using App.Extensions;
using Core.Abstract.Overlay;
using Core.Draw.Figures.Interfaces;
using Core.Draw.Figures.Primitives;
using Core.Windows.Abstract;
using Engine.Overlays.Debug;
using Engine.Overlays.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace App.Windows
{
    public class MainWindow : GameWindow
    {
        private static Random _rnd = new();

        private Vector2f? _nextLinePosStart;
        private Vector2f? _nextLinePosEnd;

        private Vector2f? _currentMouthPosition;

        private readonly List<Line> _rayCastBarrierLines = new();

        // private readonly List<Line> _raysDisplay = new();

        private const int RAYS_AMOUNT = 1000;

        private readonly List<(Color color, float angle)> _raysMetadata = new(RAYS_AMOUNT);
        
        // private readonly List<Line> _drawRays = new();

        private OverlayManager _overlayManager = new();

        public MainWindow(uint width, uint height)
            : base(
                width,
                height,
                "Main window",
                new Color(255, 200, 0, 1))
        {
            _overlayManager.AddOverlay(new DebugOverlay(this) {FontColor = Color.Blue});
        }

        protected override void Initialize()
        {
            Console.WriteLine("INITIALIZED");
            ResetDrawLineState();

            _overlayManager.LoadContentAll();

            RenderWindow.KeyPressed += OnKeyPressed;
            RenderWindow.MouseMoved += OnMouseMoved;
            RenderWindow.MouseButtonPressed += OnMouseBtnPressed;
            RenderWindow.MouseEntered += (sender, args) => { _currentMouthPosition = new Vector2f(-1, -1); };
            RenderWindow.MouseLeft += (sender, args) => { _currentMouthPosition = null; };

            const float angleStep = 365f / (float) RAYS_AMOUNT;

            for (float i = 0, angle = 0; i < RAYS_AMOUNT; i++, angle += angleStep)
            {
                var randomRayColor =
                    Color.Green; // new Color((byte) _rnd.Next(256), (byte) _rnd.Next(256), (byte) _rnd.Next(256), 30);

                _raysMetadata.Add((randomRayColor, angle));
            }

            base.Initialize();
        }

        protected override void FrameDraw()
        {
            foreach (var figure in _rayCastBarrierLines)
            {
                DrawFigure(figure);
            }

            DrawRays();

            _overlayManager.DrawAll();
        }

        private void DrawFigure(IFigure figure)
        {
            RenderWindow.Draw(
                figure.ToVertex(),
                figure.PrimitiveType
            );
        }

        private void DrawRays()
        {
            if (!_currentMouthPosition.HasValue || _currentMouthPosition.Value.X < 0) return;

            foreach (var ray in _raysMetadata.Select(rayMetadata =>
                CreateResistRayFromPenetratingRay(_currentMouthPosition.Value, rayMetadata.angle, rayMetadata.color)))
            {
                RenderWindow.Draw(
                    ray.ToVertex(),
                    ray.PrimitiveType
                );
            }
        }

        private void OnKeyPressed(object sender, KeyEventArgs args)
        {
            // Console.WriteLine($"Key {args.ToString()} is pressed");
        }

        private void OnMouseMoved(object sender, MouseMoveEventArgs args)
        {
            _currentMouthPosition = new Vector2f(args.X, args.Y);
        }

        private void OnMouseBtnPressed(object sender, MouseButtonEventArgs args)
        {
            Console.WriteLine($"Mouse click at: x: {args.X} y: {args.Y}");

            if (args.Button == Mouse.Button.Left)
            {
                HandleLeftMouseBtnPressed(args);
            }
        }

        private void HandleLeftMouseBtnPressed(MouseButtonEventArgs args)
        {
            if (!_nextLinePosStart.HasValue)
            {
                _nextLinePosStart = new Vector2f(args.X, args.Y);
            }
            else
            {
                _nextLinePosEnd = new Vector2f(args.X, args.Y);

                _rayCastBarrierLines.Add(new Line(_nextLinePosStart.Value, _nextLinePosEnd.Value, Color.Blue));
                Console.WriteLine("New line added");
                ResetDrawLineState();
            }
        }

        private Line CreateResistRayFromPenetratingRay(Vector2f rayStart, float angleDegrees, Color rayColor)
        {
            var penetratingRay = CreatePenetratingRay(rayStart, angleDegrees, rayColor);

            foreach (var barrier in _rayCastBarrierLines)
            {
                if (!penetratingRay.TryGetIntersectAt(barrier, out var intersectPoint)) continue;

                penetratingRay.End = intersectPoint;

                Console.WriteLine($"Interception found: ({intersectPoint.X}; {intersectPoint.Y})");
                break;  
            }

            return penetratingRay;
        }

        private Line CreatePenetratingRay(Vector2f rayStart, float angleDegrees, Color rayColor)
        {
            if (angleDegrees is > 365 or < 0) throw new ArgumentException("Angle value should be in [-365;365] range");

            var h = rayStart.Y;
            short xWidthResMultiplier = 1;
            float orientedAngle;

            float y = 0;

            if (angleDegrees <= 90)
            {
                orientedAngle = angleDegrees;
            }
            else if (angleDegrees <= 180)
            {
                orientedAngle = 180 - angleDegrees;
                h = RenderWindow.Size.Y - rayStart.Y;
                xWidthResMultiplier = -1;
                y = RenderWindow.Size.Y;
            }
            else if (angleDegrees <= 270)
            {
                orientedAngle = 270 - angleDegrees;
                h = RenderWindow.Size.Y - rayStart.Y;
                y = RenderWindow.Size.Y;
            }
            else
            {
                orientedAngle = 360 - angleDegrees;
                xWidthResMultiplier = -1;
            }

            var g = h / Math.Cos(ToRadians(orientedAngle));
            var xWidth = (float) Math.Sqrt(Math.Pow(g, 2) - Math.Pow(h, 2));

            var x = rayStart.X + xWidth * xWidthResMultiplier;

            // Console.WriteLine($"h: {h}; g: {g}; alpha: {angleDegrees}; x: {xWidth}");

            return new Line(rayStart, new(x, y), rayColor);
        }

        /*
        private Vector2f DirectRay(Vector2f rayStart, float angle, float triangleLeg)
        {
            if (angle is >= 0 and < 90)
                return new Vector2f(rayStart.X + triangleLeg, 0);
            if(angle is >= 90 and < 180) 
                return new Vector2f(0, rayStart.Y + triangleLeg);
            if (angle is >= 180 and < 270)
                return new Vector2f(0, rayStart.Y + triangleLeg);
            else return new Vector2f(0, rayStart.Y + triangleLeg);
        }
        */

        private void ResetDrawLineState()
        {
            _nextLinePosStart = null;
            _nextLinePosStart = null;
        }

        private float ToRadians(float degrees)
            => (float) (degrees * (Math.PI / 180.0));
    }
}