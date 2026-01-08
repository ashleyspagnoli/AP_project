using System;
using System.Collections.Generic;
using System.Text;

namespace AP_project.Engine
{
    public class InputManager
    {
        private static InputManager instance;
        public static InputManager Instance => instance ??= new InputManager();

        private HashSet<Keys> currentKeys = new HashSet<Keys>();
        private HashSet<Keys> previousKeys = new HashSet<Keys>();
        private readonly object inputLock = new object(); // ADD THIS

        private InputManager() { }

        public void Update()
        {
            lock (inputLock) // ADD THIS
            {
                previousKeys = new HashSet<Keys>(currentKeys);
            }
        }

        public void SetKeyDown(Keys key)
        {
            lock (inputLock) // ADD THIS
            {
                currentKeys.Add(key);
            }
        }

        public void SetKeyUp(Keys key)
        {
            lock (inputLock) // ADD THIS
            {
                currentKeys.Remove(key);
            }
        }

        public bool IsKeyDown(Keys key)
        {
            lock (inputLock) // ADD THIS
            {
                return currentKeys.Contains(key);
            }
        }

        public bool IsKeyReleased(Keys key)
        {
            lock (inputLock) // ADD THIS
            {
                return !currentKeys.Contains(key) && previousKeys.Contains(key);
            }
        }
    }
}
