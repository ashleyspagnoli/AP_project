namespace AP_project.Engine
{
    // Manages all game objects (entities) in the current scene
    public class Scene
    {
        protected List<Entity> entities = new List<Entity>();
        protected List<Entity> entitiesToAdd = new List<Entity>();
        private Control renderTarget;
        protected readonly object entityLock = new object(); // prevents race condition between game thread (updating) and UI thread (drawing)

        public Scene(Control renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entity.IsActive = false;
        }

        // Called on every entity to check collisions and update their state
        public virtual void Update(double deltaTime)
        {
            lock (entityLock) {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].IsActive)
                        entities[i].Update(deltaTime);
                }
            }

            CollisionManager.CheckCollisions(entities);

            entities.RemoveAll(e => !e.IsActive);
        }

        // Thread-safe way to trigger repaint from game thread
        public void RequestRedraw()
        {
            // If called from a different thread, marshal the call to the UI thread
            if (renderTarget.InvokeRequired)
            {
                renderTarget.BeginInvoke(new Action(() => renderTarget.Invalidate()));
            }
            else
            {
                renderTarget.Invalidate();
            }
        }

        // Renders all active entities
        public virtual void Draw(Graphics g)
        {
            lock (entityLock)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].IsActive)
                        entities[i].Draw(g);
                }
            }
        }

        public void ClearAllEntities()
        {
            lock (entityLock)
            {
                entities.Clear();
            }
        }
    }
}
