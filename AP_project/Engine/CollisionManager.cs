namespace AP_project.Engine
{
    public static class CollisionManager
    {
        public static void CheckCollisions(List<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].IsActive) continue;

                for (int j = i + 1; j < entities.Count; j++)
                {
                    if (!entities[j].IsActive) continue;

                    if (CheckCircleCollision(entities[i], entities[j]))
                    {
                        OnCollision(entities[i], entities[j]);
                    }
                }
            }
        }

        private static bool CheckCircleCollision(Entity a, Entity b)
        {
            float dx = a.Position.X - b.Position.X;
            float dy = a.Position.Y - b.Position.Y;
            float distanceSquared = dx * dx + dy * dy;
            float radiusSum = a.Radius + b.Radius;

            return distanceSquared < radiusSum * radiusSum;
        }

        private static void OnCollision(Entity a, Entity b)
        {
            // Entities can implement ICollidable for custom behavior
            (a as ICollidable)?.OnCollide(b);
            (b as ICollidable)?.OnCollide(a);
        }
    }
}
