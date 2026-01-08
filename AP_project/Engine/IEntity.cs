namespace AP_project.Engine
{
    // Base entity interface
    public interface IEntity
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        float Rotation { get; set; }
        bool IsActive { get; set; }

        void Update(double deltaTime);
        void Draw(Graphics g);
    }
}
