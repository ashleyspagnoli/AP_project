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

        private InputManager() { }

        public void Update()
        {
            previousKeys = new HashSet<Keys>(currentKeys);
        }

        public void SetKeyDown(Keys key)
        {
            currentKeys.Add(key);
        }

        public void SetKeyUp(Keys key)
        {
            currentKeys.Remove(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return currentKeys.Contains(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return currentKeys.Contains(key) && !previousKeys.Contains(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return !currentKeys.Contains(key) && previousKeys.Contains(key);
        }
    }
}
