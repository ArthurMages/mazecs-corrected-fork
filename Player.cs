namespace Epsi.MazeCs
{
    public class Player
    {
        public Vec2d Position { get; private set; }
        private readonly Maze maze;
        private readonly IGridDisplay display;
        private readonly IController controller;

        public int Points { get; private set; }
        public List<ICollectable> Inventory { get; } = new();

        public event Action<int>? PointsChanged;
        public event Action? InventoryChanged;

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

            if (controller.IsPickupPressed)
            {
                TryPickup();
            }

            if (TryMove(delta, out reachedExit))
                return true;
            return true; // continue playing
        }

        public void TryPickup()
        {
            if (maze[Position] is Room room && room.Collectable != null)
            {
                var collectable = room.Collectable;
                Points += collectable.Points;
                PointsChanged?.Invoke(Points);
                if (collectable.IsPersistent)
                {
                    Inventory.Add(collectable);
                    InventoryChanged?.Invoke();
                }
                room.Collectable = null;
                // Redraw the cell after pickup
                display.DrawCell(Position.X, Position.Y, room);
            }
        }

        public bool TryMove(Vec2d delta, out bool reachedExit)
        {
            reachedExit = false;
            var next = Position + delta;
            if (!next.InBounds(maze.Width, maze.Height))
                return false;

            var cell = maze[next];
            if (cell.IsWall)
            {
                if (cell is Door)
                {
                    var hasKey = Inventory.Any(item => item is Key);
                    if (!hasKey)
                        return false;
                }
                else
                {
                    return false;
                }
            }

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