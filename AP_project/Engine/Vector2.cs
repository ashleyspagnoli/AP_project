namespace AP_project.Engine
{ 
    public struct Vector2
    {
        public float X, Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Length() => (float)Math.Sqrt(X * X + Y * Y);

        public static Vector2 operator +(Vector2 a, Vector2 b)
            => new Vector2(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator *(Vector2 v, float scalar)
            => new Vector2(v.X * scalar, v.Y * scalar);
    }
}
