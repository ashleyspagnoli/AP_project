namespace AP_project.Engine
{
    // Tracks keyboard state across frames
    public class InputManager
    {
        private static InputManager instance;
        public static InputManager Instance => instance ??= new InputManager();

        private HashSet<Keys> currentKeys = new HashSet<Keys>();
        private HashSet<Keys> previousKeys = new HashSet<Keys>();
        private readonly object inputLock = new object();

        private InputManager() { }

        public void Update()
        {
            lock (inputLock)
            {
                previousKeys = new HashSet<Keys>(currentKeys);
            }
        }

        public void SetKeyDown(Keys key)
        {
            lock (inputLock)
            {
                currentKeys.Add(key);
            }
        }

        public void SetKeyUp(Keys key)
        {
            lock (inputLock)
            {
                currentKeys.Remove(key);
            }
        }

        public bool IsKeyDown(Keys key)
        {
            lock (inputLock)
            {
                return currentKeys.Contains(key);
            }
        }
    }
}
