﻿namespace Core
{
    public class GameTime
    {
        private float _deltaTime = 0f;
        private float _timeScale = 1f;
        
        public float TimeScale
        {
            get => _deltaTime;
            set => _timeScale = value;
        }

        public float DeltaTime
        {
            get => _deltaTime * _timeScale;
            set => _deltaTime = value;
        }

        public float DeltaTimeUnscaled => _deltaTime;

        public float TotalTimeElapsed { get; private set; }

        public void Update(float deltaTime, float totalTimeElapsed)
        {
            _deltaTime = deltaTime;
            TotalTimeElapsed = totalTimeElapsed;
        }
    }
}
