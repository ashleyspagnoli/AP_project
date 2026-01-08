namespace AP_project.Game
{
    public class Bullet : Engine.Entity, Engine.ICollidable
    {
        private const float LIFETIME = 2f;
        private float timeAlive = 0f;
        private Rectangle bounds;

        public Bullet(Engine.Vector2 position, Engine.Vector2 velocity, Rectangle bounds)
        {
            Position = position;
            Velocity = velocity;
            Radius = 2f;
            this.bounds = bounds;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            timeAlive += (float)deltaTime;
            if (timeAlive > LIFETIME)
            {
                IsActive = false;
            }

            // Remove if off screen
            if (Position.X < 0 || Position.X > bounds.Width ||
                Position.Y < 0 || Position.Y > bounds.Height)
            {
                IsActive = false;
            }
        }

        public void OnCollide(Engine.Entity other)
        {
            if (other is Asteroid)
            {
                IsActive = false;
            }
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.White,
                Position.X - Radius,
                Position.Y - Radius,
                Radius * 2,
                Radius * 2);
        }
    }
}