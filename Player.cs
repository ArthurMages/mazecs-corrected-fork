namespace Epsi.MazeCs
{
    public class Player
    {
        public Vec2d Position { get; private set; }
        private readonly Maze maze;
        private readonly IGridDisplay display;
        private readonly IController controller;

        public Player(Maze maze, IGridDisplay display, IController controller)
        {
            this.maze = maze;
            this.display = display;
            this.controller = controller;
            Position = maze.Start;
            Draw();
        }

        public bool Update(out bool reachedExit)
        {
            reachedExit = false;
            var (delta, canceled) = controller.ReadInput();
            if (canceled)
                return false;
            if (TryMove(delta, out reachedExit))
                return true;
            return true; // continue playing
        }

        public bool TryMove(Vec2d delta, out bool reachedExit)
        {
            reachedExit = false;
            var next = Position + delta;
            if (!next.InBounds(maze.Width, maze.Height))
                return false;
            if (maze.IsWall(next))
                return false;
            display.DrawCell(Position.X, Position.Y, maze[Position]);
            Position = next;
            Draw();
            if (Position == maze.Exit)
            {
                reachedExit = true;
            }
            return true;
        }

        public void Draw()
        {
            display.DrawTextXY(display.OffsetX + Position.X, display.OffsetY + Position.Y, "@", display.PlayerColor);
        }
    }
}