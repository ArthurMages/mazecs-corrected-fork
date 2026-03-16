namespace Epsi.MazeCs
{
    public class Door : Cell
    {
        public ICollectable Key { get; }

        public Door()
        {
            Key = new Key();
        }

        public override bool IsWall => true; // By default, door is wall unless key is in inventory
        public override (string Symbol, ConsoleColor Color) GetDisplayInfo(ConsoleScreen screen) => ("🚪", screen.WallColor);
    }
}