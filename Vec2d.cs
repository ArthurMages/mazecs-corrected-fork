namespace Epsi.MazeCs
{
    public record Vec2d(int X, int Y)
    {
        public Vec2d Add(Vec2d other) => this with { X = X + other.X, Y = Y + other.Y };
        public bool InBounds(int width, int height) => X >= 0 && X < width && Y >= 0 && Y < height;

        public static Vec2d operator +(Vec2d a, Vec2d b) => new(a.X + b.X, a.Y + b.Y);
        public static Vec2d operator -(Vec2d a, Vec2d b) => new(a.X - b.X, a.Y - b.Y);
        public static Vec2d operator *(Vec2d v, int scalar) => new(v.X * scalar, v.Y * scalar);

        public static readonly Vec2d Up    = new(0, -1);
        public static readonly Vec2d Down  = new(0, 1);
        public static readonly Vec2d Left  = new(-1, 0);
        public static readonly Vec2d Right = new(1, 0);

        public static readonly Vec2d[] Directions = { Up, Right, Down, Left };
    }
}