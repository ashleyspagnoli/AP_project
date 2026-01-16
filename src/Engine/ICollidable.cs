namespace AP_project.Engine
{
    // Interface for collidable entities
    public interface ICollidable
    {
        void OnCollide(Entity other);
    }
}
