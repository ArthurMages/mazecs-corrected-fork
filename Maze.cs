namespace Epsi.MazeCs
{
    public class Maze
    {
        private readonly CellType[,] grid;
        public int Width { get; }
        public int Height { get; }
        public Vec2d Start { get; }
        public Vec2d Exit { get; }

        public Maze(IMazeGenerator generator)
        {
            Width = generator.Width;
            Height = generator.Height;
            grid = generator.Generate();
            Start = new Vec2d(0, 0);
            if (Start.InBounds(Width, Height))
            {
                grid[Start.X, Start.Y] = CellType.Start;
            }
            Exit = FindExit();
        }

        private Vec2d FindExit()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (grid[x, y] == CellType.Exit)
                        return new Vec2d(x, y);
            return new Vec2d(0, 0);
        }

        public CellType this[int x, int y] => grid[x, y];
        public CellType this[Vec2d pos] => grid[pos.X, pos.Y];

        public bool IsWall(Vec2d pos) => this[pos] == CellType.Wall;

        public void Draw(ConsoleScreen screen)
        {
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                screen.DrawCell(x, y, grid[x, y]);
        }
    }
}