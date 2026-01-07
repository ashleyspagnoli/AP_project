using System;
using System.Drawing;
using System.Windows.Forms;

namespace AP_project.Game
{
    public class Ship : Engine.Entity, Engine.ICollidable
    {
        private const float THRUST_POWER = 200f;
        private const float ROTATION_SPEED = 4f;
        private const float DRAG = 0.98f;
        private const float MAX_SPEED = 400f;
        private const float FIRE_COOLDOWN = 0.25f;

        private float fireCooldownTimer = 0f;
        private GameScene gameScene;

        public bool IsThrusting { get; private set; }

        public Ship(GameScene scene, Engine.Vector2 position)
        {
            this.gameScene = scene;
            Position = position;
            Velocity = new Engine.Vector2(0, 0);
            Rotation = 0f;
            Radius = 15f;
        }

        public override void Update(double deltaTime)
        {
            float dt = (float)deltaTime;

            // Handle input
            IsThrusting = false;

            if (Engine.InputManager.Instance.IsKeyDown(Keys.Left))
            {
                Rotation -= ROTATION_SPEED * dt;
            }
            if (Engine.InputManager.Instance.IsKeyDown(Keys.Right))
            {
                Rotation += ROTATION_SPEED * dt;
            }
            if (Engine.InputManager.Instance.IsKeyDown(Keys.Up))
            {
                IsThrusting = true;
                float thrustX = (float)Math.Sin(Rotation) * THRUST_POWER * dt;
                float thrustY = -(float)Math.Cos(Rotation) * THRUST_POWER * dt;
                Velocity = new Engine.Vector2(Velocity.X + thrustX, Velocity.Y + thrustY);
            }

            // Fire bullets
            fireCooldownTimer -= dt;
            if (Engine.InputManager.Instance.IsKeyDown(Keys.Space) && fireCooldownTimer <= 0)
            {
                FireBullet();
                fireCooldownTimer = FIRE_COOLDOWN;
            }

            // Apply drag
            Velocity = Velocity * DRAG;

            // Limit max speed
            float speed = Velocity.Length();
            if (speed > MAX_SPEED)
            {
                Velocity = Velocity * (MAX_SPEED / speed);
            }

            // Update position
            base.Update(deltaTime);

            // Wrap around screen
            WrapAroundScreen();
        }

        private void FireBullet()
        {
            float bulletSpeed = 500f;
            float angle = Rotation;

            // Spawn bullet at ship's nose
            float noseX = Position.X + (float)Math.Sin(angle) * 20f;
            float noseY = Position.Y - (float)Math.Cos(angle) * 20f;

            Engine.Vector2 bulletVel = new Engine.Vector2(
                (float)Math.Sin(angle) * bulletSpeed,
                -(float)Math.Cos(angle) * bulletSpeed
            );

            // Add ship's velocity to bullet
            bulletVel = bulletVel + Velocity;

            Bullet bullet = new Bullet(new Engine.Vector2(noseX, noseY), bulletVel, gameScene.Bounds);
            gameScene.AddEntity(bullet);
        }

        private void WrapAroundScreen()
        {
            if (Position.X < 0) Position = new Engine.Vector2(gameScene.Bounds.Width, Position.Y);
            if (Position.X > gameScene.Bounds.Width) Position = new Engine.Vector2(0, Position.Y);
            if (Position.Y < 0) Position = new Engine.Vector2(Position.X, gameScene.Bounds.Height);
            if (Position.Y > gameScene.Bounds.Height) Position = new Engine.Vector2(Position.X, 0);
        }

        public void OnCollide(Engine.Entity other)
        {
            if (other is Asteroid asteroid)
            {
                gameScene.OnShipDestroyed(asteroid);
            }
        }

        public override void Draw(Graphics g)
        {
            // Save graphics state
            var state = g.Save();

            // Transform to ship position and rotation
            g.TranslateTransform(Position.X, Position.Y);
            g.RotateTransform((float)(Rotation * 180 / Math.PI));

            // Draw ship triangle
            PointF[] shipPoints = new PointF[]
            {
                new PointF(0, -15),      // Nose
                new PointF(-10, 15),     // Left back
                new PointF(10, 15)       // Right back
            };

            g.DrawPolygon(Pens.White, shipPoints);

            // Draw thrust flame
            if (IsThrusting)
            {
                PointF[] flamePoints = new PointF[]
                {
                    new PointF(-5, 15),
                    new PointF(0, 25),
                    new PointF(5, 15)
                };
                g.DrawLines(Pens.Orange, flamePoints);
            }

            // Restore graphics state
            g.Restore(state);
        }
    }
}