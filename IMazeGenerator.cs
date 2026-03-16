namespace Epsi.MazeCs
{
    public interface IMazeGenerator
    {
        int Width { get; }
        int Height { get; }
        CellType[,] Generate();
    }
}