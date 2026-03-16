namespace Epsi.MazeCs
{
    public interface IGridDisplay
    {
        int OffsetX { get; }
        int OffsetY { get; }
        ConsoleColor PlayerColor { get; }
        void DrawCell(int x, int y, Cell cell);
        void DrawTextXY(int x, int y, string text, ConsoleColor? color = null);
    }
}