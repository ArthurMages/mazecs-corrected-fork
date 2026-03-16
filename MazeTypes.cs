namespace Epsi.MazeCs
{
    public enum State
    {
        Playing,
        Won,
        Canceled
    }

    public abstract class Cell
    {
        public abstract bool IsWall { get; }
        public abstract (string Symbol, ConsoleColor Color) GetDisplayInfo(ConsoleScreen screen);
    }

    public class Wall : Cell
    {
        public override bool IsWall => true;
        public override (string Symbol, ConsoleColor Color) GetDisplayInfo(ConsoleScreen screen) => ("█", screen.WallColor);
    }

    public class Room : Cell
    {
        public bool IsStart { get; set; }
        public bool IsExit { get; set; }
        public override bool IsWall => false;
        public override (string Symbol, ConsoleColor Color) GetDisplayInfo(ConsoleScreen screen)
        {
            if (IsStart) return ("S", screen.PlayerColor);
            if (IsExit) return ("★", screen.ExitColor);
            return ("·", screen.CorridorColor);
        }
    }
}