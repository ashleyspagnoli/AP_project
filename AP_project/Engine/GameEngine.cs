using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AP_project.Engine
{
    public class GameEngine
    {
        private volatile bool isRunning;
        private Thread gameThread;
        private const double TARGET_FPS = 60;
        private const double TIME_PER_FRAME = 1000.0 / TARGET_FPS; // ms

        public Scene CurrentScene { get; set; }

        public void Start()
        {
            isRunning = true;
            gameThread = new Thread(GameLoop);
            gameThread.IsBackground = true;
            gameThread.Start();
        }

        private void GameLoop()
        {
            Stopwatch timer = Stopwatch.StartNew();
            double previous = timer.Elapsed.TotalMilliseconds;
            double lag = 0.0;

            while (isRunning)
            {
                double current = timer.Elapsed.TotalMilliseconds;
                double elapsed = current - previous;
                previous = current;
                lag += elapsed;

                // Process input
                InputManager.Instance.Update();

                // Fixed timestep updates for consistent physics
                while (lag >= TIME_PER_FRAME)
                {
                    CurrentScene?.Update(TIME_PER_FRAME / 1000.0); // Convert to seconds
                    lag -= TIME_PER_FRAME;
                }

                // Render with interpolation factor
                double interpolation = lag / TIME_PER_FRAME;
                CurrentScene?.RequestRedraw();

                // Sleep to avoid burning CPU
                Thread.Sleep(1);
            }
        }

        public void Stop()
        {
            isRunning = false;
            gameThread?.Join();
        }
    }
}
