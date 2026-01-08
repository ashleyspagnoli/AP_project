using System;
using System.Collections.Generic;
using System.Text;

namespace AP_project.Engine
{
    public class Scene
    {
        protected List<Entity> entities = new List<Entity>();
        protected List<Entity> entitiesToAdd = new List<Entity>();
        private Control renderTarget;
        protected readonly object entityLock = new object();

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

        public void RequestRedraw()
        {
            if (renderTarget.InvokeRequired)
            {
                renderTarget.BeginInvoke(new Action(() => renderTarget.Invalidate()));
            }
            else
            {
                renderTarget.Invalidate();
            }
        }

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
