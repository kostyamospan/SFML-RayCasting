using System;
using System.Collections.Generic;
using System.Linq;
using App.Extensions;
using Core.Abstract.Scenes;
using Core.Draw.Figures.Interfaces;
using Core.Draw.Figures.Primitives;
using Engine.Overlays.Debug;
using Engine.Overlays.Utils;
using Engine.Scenes.Abstract;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace App.Scenes
{
    public class RayCastSandbox : ManagedScene
    {
        private Vector2f? _nextLinePosStart;
        private Vector2f? _nextLinePosEnd;

        private Vector2f? _currentMouthPosition;

        private readonly List<Line> _rayCastBarrierLines = new();

        private const int RaysAmount = 100;

        private readonly List<(Color color, float angle)> _raysMetadata = new(RaysAmount);

        public RayCastSandbox(RenderWindow renderWindow) : base(renderWindow, new())
        {
        }

        public override void Initialize()
        {
            Console.WriteLine("INITIALIZED");
            ResetDrawLineState();

            RenderWindow.KeyPressed += OnKeyPressed;
            RenderWindow.MouseMoved += OnMouseMoved;
            RenderWindow.MouseButtonPressed += OnMouseBtnPressed;
            RenderWindow.MouseEntered += (sender, args) => { _currentMouthPosition = new Vector2f(-1, -1); };
            RenderWindow.MouseLeft += (sender, args) => { _currentMouthPosition = null; };

            const float angleStep = 365f / (float) RaysAmount;

            for (float i = 0, angle = 0; i < RaysAmount; i++, angle += angleStep)
            {
                var randomRayColor =
                    Color.White; // new Color((byte) _rnd.Next(256), (byte) _rnd.Next(256), (byte) _rnd.Next(256), 30);

                _raysMetadata.Add((randomRayColor, angle));
            }
        }

        public override void Draw()
        {
            foreach (var figure in _rayCastBarrierLines)
            {
                DrawFigure(figure);
            }

            DrawRays();
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
            if (args.Code == Keyboard.Key.Escape)
                OnPrevSceneRequestCallback?.Invoke();                
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
                _rayCastBarrierLines.Add(new Line(_nextLinePosStart.Value, _nextLinePosEnd.Value, Color.White));

                ResetDrawLineState();
            }
        }

        private Line CreateResistRayFromPenetratingRay(Vector2f rayStart, float angleDegrees, Color rayColor)
        {
            var penetratingRay = CreatePenetratingRay(rayStart, angleDegrees, rayColor);

            Vector2f closestIntersectPoint = default;

            foreach (var barrier in _rayCastBarrierLines)
            {
                if (!penetratingRay.TryGetIntersectAt(barrier, out var intersectPoint)) continue;

                closestIntersectPoint = ClosestToPoint(rayStart, intersectPoint, closestIntersectPoint);
            }

            if (closestIntersectPoint != default)
            {
                penetratingRay.End = closestIntersectPoint;
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

            switch (angleDegrees)
            {
                case <= 90:
                    orientedAngle = angleDegrees;
                    break;
                case <= 180:
                    orientedAngle = 180 - angleDegrees;
                    h = RenderWindow.Size.Y - rayStart.Y;
                    xWidthResMultiplier = -1;
                    y = RenderWindow.Size.Y;
                    break;
                case <= 270:
                    orientedAngle = 270 - angleDegrees;
                    h = RenderWindow.Size.Y - rayStart.Y;
                    y = RenderWindow.Size.Y;
                    break;
                default:
                    orientedAngle = 360 - angleDegrees;
                    xWidthResMultiplier = -1;
                    break;
            }

            var g = h / Math.Cos(ToRadians(orientedAngle));
            var xWidth = (float) Math.Sqrt(Math.Pow(g, 2) - Math.Pow(h, 2));

            var x = rayStart.X + xWidth * xWidthResMultiplier;

            return new Line(rayStart, new(x, y), rayColor);
        }

        private void ResetDrawLineState()
        {
            _nextLinePosStart = null;
            _nextLinePosStart = null;
        }

        private Vector2f ClosestToPoint(Vector2f targetPoint, Vector2f p1, Vector2f p2)
        {
            if (p1 == default && p2 != default)
                return p2;

            if (p2 == default && p1 != default)
                return p1;

            var widthP1 = (float) Math.Sqrt(Math.Pow(Math.Abs(p1.X - targetPoint.X), 2) + Math.Pow(
                Math.Abs(p1.Y - targetPoint.Y), 2));

            var widthP2 = (float) Math.Sqrt(Math.Pow(Math.Abs(p2.X - targetPoint.X), 2) + Math.Pow(
                Math.Abs(p2.Y - targetPoint.Y), 2));

            Console.WriteLine($"Width 1: {widthP1}; width 2: {widthP2}");
            return widthP1 > widthP2 ? p2 : p1;
        }

        private float ToRadians(float degrees)
            => (float) (degrees * (Math.PI / 180.0));
    }
}