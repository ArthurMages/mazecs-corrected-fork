namespace Epsi.MazeCs
{
    public class Player
    {
        public Vec2d Position { get; private set; }
        private readonly Maze maze;
        private readonly ConsoleScreen screen;
        private readonly IController controller;

        public Player(Maze maze, ConsoleScreen screen, IController controller)
        {
            this.maze = maze;
            this.screen = screen;
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
            screen.DrawCell(Position.X, Position.Y, maze[Position]);
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
            screen.DrawTextXY(screen.OffsetX + Position.X, screen.OffsetY + Position.Y, "@", screen.PlayerColor);
        }
    }
}