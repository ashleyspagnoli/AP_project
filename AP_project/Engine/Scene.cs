using System;
using System.Collections.Generic;
using System.Text;

namespace AP_project.Engine
{
    public class Scene
    {
        private List<Entity> entities = new List<Entity>();
        private List<Entity> entitiesToAdd = new List<Entity>();
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
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].IsActive)
                    entities[i].Update(deltaTime);
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
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].IsActive)
                    entities[i].Draw(g);
            }
        }

        public void ClearAllEntities()
        {
            entities.Clear();
        }
    }
}
