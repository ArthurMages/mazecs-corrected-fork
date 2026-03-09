namespace Epsi.MazeCs
{
    public class Player
    {
        public Vec2d Position { get; private set; }
        private readonly Maze maze;
        private readonly ConsoleScreen screen;

        public Player(Maze maze, ConsoleScreen screen)
        {
            this.maze = maze;
            this.screen = screen;
            Position = maze.Start;
            Draw();
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