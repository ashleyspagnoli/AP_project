using System;
using System.Collections.Generic;
using System.Text;

namespace AP_project.Engine
{
    public class Scene
    {
        private List<Entity> entities = new List<Entity>();
        private Control renderTarget;

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
            // Update all entities
            foreach (var entity in entities)
            {
                if (entity.IsActive)
                    entity.Update(deltaTime);
            }

            // Check collisions
            CollisionManager.CheckCollisions(entities);

            // Remove inactive entities
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
            foreach (var entity in entities)
            {
                if (entity.IsActive)
                    entity.Draw(g);
            }
        }

        public void ClearAllEntities()
        {
            entities.Clear();
        }
    }
}
