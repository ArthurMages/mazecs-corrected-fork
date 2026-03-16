namespace Epsi.MazeCs
{
    public interface ICollectable
    {
        bool IsPersistent { get; }
        int Points { get; }
    }
}