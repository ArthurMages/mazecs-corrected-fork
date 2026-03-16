namespace Epsi.MazeCs
{
    public interface IController
    {
        (Vec2d delta, bool canceled) ReadInput();
        bool IsUpPressed { get; }
        bool IsDownPressed { get; }
        bool IsLeftPressed { get; }
        bool IsRightPressed { get; }
        bool IsEscPressed { get; }
        bool IsPickupPressed { get; }
    }
}