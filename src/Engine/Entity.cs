namespace AP_project.Engine
{

    // Base entity implementation
    public abstract class Entity : IEntity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public bool IsActive { get; set; } = true;

        // For collision detection
        public virtual float Radius { get; protected set; }

        public virtual void Update(double deltaTime)
        {
            // Basic physics integration
            Position += Velocity * (float)deltaTime;
        }

        public abstract void Draw(Graphics g);
    }
}
