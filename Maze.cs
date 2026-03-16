namespace Epsi.MazeCs
{
    public class Maze
    {
        private readonly Cell[,] grid;
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
                grid[Start.X, Start.Y] = new Room { IsStart = true };
            }
            Exit = FindExit();
        }

        private Vec2d FindExit()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (grid[x, y] is Room room && room.IsExit)
                        return new Vec2d(x, y);
            return new Vec2d(0, 0);
        }

        public Cell this[int x, int y] => grid[x, y];
        public Cell this[Vec2d pos] => grid[pos.X, pos.Y];

        public bool IsWall(Vec2d pos) => this[pos].IsWall;

        public void Draw(IGridDisplay display)
        {
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                display.DrawCell(x, y, grid[x, y]);
        }
    }
}