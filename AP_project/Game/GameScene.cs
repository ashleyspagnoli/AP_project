using System;
using System.Drawing;
using System.Windows.Forms;

namespace AP_project.Game
{
    public class GameScene : Engine.Scene
    {
        private Ship playerShip;
        private Random random = new Random();
        private float asteroidSpawnTimer = 0f;
        private const float ASTEROID_SPAWN_INTERVAL = 3f;
        private int score = 0;
        private int lives = 3;
        private bool gameOver = false;
        private float respawnTimer = 0f;
        private const float RESPAWN_DELAY = 1f;
        private bool isRespawning = false;
        private const float RESPAWN_TIME_SCALE = 0f; // Freeze (modify to implement slow motion)
        
        public Rectangle Bounds { get; private set; }
        public bool IsRespawning => isRespawning;
        public float TimeScale => isRespawning ? RESPAWN_TIME_SCALE : 1.0f;
        
        public GameScene(Control renderTarget, Rectangle bounds) : base(renderTarget)
        {
            Bounds = bounds;
            SpawnShip();
            
            // Spawn initial asteroids
            for (int i = 0; i < 3; i++)
            {
                AddEntity(Asteroid.CreateRandom(this, Bounds));
            }
        }
        
        private void SpawnShip()
        {
            playerShip = new Ship(this, new Engine.Vector2(Bounds.Width / 2, Bounds.Height / 2));
            AddEntity(playerShip);
        }
        
        public override void Update(double deltaTime)
        {
            if (gameOver)
            {
                // Check for restart
                if (Engine.InputManager.Instance.IsKeyPressed(Keys.R))
                {
                    RestartGame();
                }
                return;
            }
            
            // Handle respawn
            if (playerShip != null && !playerShip.IsActive && !isRespawning)
            {
                // Start respawn sequence
                isRespawning = true;
                respawnTimer = 0f;
            }
            
            if (isRespawning)
            {
                respawnTimer += (float)deltaTime;
                if (respawnTimer >= RESPAWN_DELAY)
                {
                    if (lives > 0)
                    {
                        SpawnShip();
                    }
                    isRespawning = false;
                    respawnTimer = 0f;
                }
            }
            
            // Apply time scale to delta time for slow motion effect
            double scaledDeltaTime = deltaTime * TimeScale;
            
            // Update all entities with scaled time
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].IsActive)
                {
                    // Ship always updates at normal speed during respawn
                    if (entities[i] is Ship)
                    {
                        entities[i].Update(deltaTime);
                    }
                    else
                    {
                        entities[i].Update(scaledDeltaTime);
                    }
                }
            }
            
            // Check collisions
            Engine.CollisionManager.CheckCollisions(entities);
            
            // Remove inactive entities
            entities.RemoveAll(e => !e.IsActive);
            
            // Add new entities that were spawned during this frame
            if (entitiesToAdd.Count > 0)
            {
                entities.AddRange(entitiesToAdd);
                entitiesToAdd.Clear();
            }
            
            // Spawn new asteroids periodically (not during respawn)
            if (!isRespawning)
            {
                asteroidSpawnTimer += (float)deltaTime;
                if (asteroidSpawnTimer >= ASTEROID_SPAWN_INTERVAL)
                {
                    AddEntity(Asteroid.CreateRandom(this, Bounds));
                    asteroidSpawnTimer = 0f;
                }
            }
        }
        
        public void OnShipDestroyed(Asteroid collidingAsteroid)
        {
            lives--;
            playerShip.IsActive = false;
            
            // Destroy the asteroid that hit the ship
            if (collidingAsteroid != null)
            {
                collidingAsteroid.IsActive = false;
            }
            
            if (lives <= 0)
            {
                gameOver = true;
            }
        }
        
        public void OnAsteroidDestroyed(Asteroid asteroid)
        {
            switch (asteroid.Size)
            {
                case AsteroidSize.Large:
                    score += 20;
                    break;
                case AsteroidSize.Medium:
                    score += 50;
                    break;
                case AsteroidSize.Small:
                    score += 100;
                    break;
            }
        }
        
        private void RestartGame()
        {
            // Clear all entities
            ClearAllEntities();
            
            // Reset game state
            score = 0;
            lives = 3;
            gameOver = false;
            asteroidSpawnTimer = 0f;
            respawnTimer = 0f;
            isRespawning = false;
            
            // Spawn initial setup
            SpawnShip();
            for (int i = 0; i < 3; i++)
            {
                AddEntity(Asteroid.CreateRandom(this, Bounds));
            }
        }
        
        public override void Draw(Graphics g)
        {
            // Draw all entities
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].IsActive)
                    entities[i].Draw(g);
            }
            
            // Draw HUD
            using (Font font = new Font("Arial", 16))
            {
                g.DrawString($"Score: {score}", font, Brushes.White, 10, 10);
                g.DrawString($"Lives: {lives}", font, Brushes.White, 10, 35);
                
                // Show respawn indicator
                if (isRespawning)
                {
                    string respawnText = "Respawning...";
                    SizeF textSize = g.MeasureString(respawnText, font);
                    g.DrawString(respawnText, font, Brushes.Yellow, 
                        (Bounds.Width - textSize.Width) / 2, 
                        Bounds.Height - 60);
                }
                
                if (gameOver)
                {
                    string gameOverText = "GAME OVER";
                    string restartText = "Press R to Restart";
                    SizeF gameOverSize = g.MeasureString(gameOverText, font);
                    SizeF restartSize = g.MeasureString(restartText, font);
                    
                    g.DrawString(gameOverText, font, Brushes.Red, 
                        (Bounds.Width - gameOverSize.Width) / 2, 
                        (Bounds.Height - gameOverSize.Height) / 2 - 20);
                    g.DrawString(restartText, font, Brushes.White, 
                        (Bounds.Width - restartSize.Width) / 2, 
                        (Bounds.Height - restartSize.Height) / 2 + 20);
                }
            }
        }
    }
}