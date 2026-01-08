namespace AP_project.Game
{
    public enum AsteroidSize
    {
        Large,
        Medium,
        Small
    }

    public class Asteroid : Engine.Entity, Engine.ICollidable
    {
        private static Random random = new Random();
        private AsteroidSize size;
        private float rotationSpeed;
        private PointF[] shape;
        private Rectangle bounds;
        private GameScene gameScene;

        public AsteroidSize Size => size;

        public Asteroid(GameScene scene, Engine.Vector2 position, Engine.Vector2 velocity, AsteroidSize size, Rectangle bounds)
        {
            this.gameScene = scene;
            this.bounds = bounds;
            this.size = size;
            Position = position;
            Velocity = velocity;
            Rotation = (float)(random.NextDouble() * Math.PI * 2);
            rotationSpeed = (float)(random.NextDouble() * 2 - 1) * 2f;

            // Set radius based on size
            switch (size)
            {
                case AsteroidSize.Large:
                    Radius = 40f;
                    break;
                case AsteroidSize.Medium:
                    Radius = 20f;
                    break;
                case AsteroidSize.Small:
                    Radius = 10f;
                    break;
            }

            GenerateShape();
        }

        private void GenerateShape()
        {
            int points = 8;
            shape = new PointF[points];

            for (int i = 0; i < points; i++)
            {
                float angle = (float)(i * Math.PI * 2 / points);
                float distance = Radius * (0.7f + (float)random.NextDouble() * 0.3f);
                shape[i] = new PointF(
                    (float)Math.Cos(angle) * distance,
                    (float)Math.Sin(angle) * distance
                );
            }
        }

        public override void Update(double deltaTime)
        {
            Rotation += rotationSpeed * (float)deltaTime;
            base.Update(deltaTime);
            WrapAroundScreen();
        }

        private void WrapAroundScreen()
        {
            if (Position.X < -Radius) Position = new Engine.Vector2(bounds.Width + Radius, Position.Y);
            if (Position.X > bounds.Width + Radius) Position = new Engine.Vector2(-Radius, Position.Y);
            if (Position.Y < -Radius) Position = new Engine.Vector2(Position.X, bounds.Height + Radius);
            if (Position.Y > bounds.Height + Radius) Position = new Engine.Vector2(Position.X, -Radius);
        }

        public void OnCollide(Engine.Entity other)
        {
            if (other is Bullet)
            {
                Split();
                IsActive = false;
                gameScene.OnAsteroidDestroyed(this);
            }
        }

        private void Split()
        {
            if (size == AsteroidSize.Small)
                return;

            AsteroidSize newSize = size == AsteroidSize.Large ? AsteroidSize.Medium : AsteroidSize.Small;

            // Create two smaller asteroids
            for (int i = 0; i < 2; i++)
            {
                float angle = (float)(random.NextDouble() * Math.PI * 2);
                float speed = 50f + (float)random.NextDouble() * 100f;

                Engine.Vector2 newVelocity = new Engine.Vector2(
                    (float)Math.Cos(angle) * speed,
                    (float)Math.Sin(angle) * speed
                );

                Asteroid newAsteroid = new Asteroid(gameScene, Position, newVelocity, newSize, bounds);
                gameScene.AddEntity(newAsteroid);
            }
        }

        public override void Draw(Graphics g)
        {
            var state = g.Save();

            g.TranslateTransform(Position.X, Position.Y);
            g.RotateTransform((float)(Rotation * 180 / Math.PI));

            // Draw asteroid shape
            if (shape.Length > 0)
            {
                g.DrawPolygon(Pens.White, shape);
            }

            g.Restore(state);
        }

        public static Asteroid CreateRandom(GameScene scene, Rectangle bounds)
        {
            // Spawn from edges
            Engine.Vector2 position;
            int edge = random.Next(4);

            switch (edge)
            {
                case 0: // Top
                    position = new Engine.Vector2(random.Next(bounds.Width), -50);
                    break;
                case 1: // Right
                    position = new Engine.Vector2(bounds.Width + 50, random.Next(bounds.Height));
                    break;
                case 2: // Bottom
                    position = new Engine.Vector2(random.Next(bounds.Width), bounds.Height + 50);
                    break;
                default: // Left
                    position = new Engine.Vector2(-50, random.Next(bounds.Height));
                    break;
            }

            // Velocity toward center
            Engine.Vector2 center = new Engine.Vector2(bounds.Width / 2, bounds.Height / 2);
            float dirX = center.X - position.X;
            float dirY = center.Y - position.Y;
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            float speed = 30f + (float)random.NextDouble() * 50f;
            Engine.Vector2 velocity = new Engine.Vector2(
                (dirX / length) * speed,
                (dirY / length) * speed
            );

            return new Asteroid(scene, position, velocity, AsteroidSize.Large, bounds);
        }
    }
}