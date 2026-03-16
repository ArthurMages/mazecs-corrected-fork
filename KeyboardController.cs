namespace Epsi.MazeCs
{
    public class KeyboardController : IController
    {
        public (Vec2d delta, bool canceled) ReadInput()
        {
            var key = Console.ReadKey(true).Key;
            return key switch
            {
                ConsoleKey.Z or ConsoleKey.UpArrow    => (Vec2d.Up, false),
                ConsoleKey.S or ConsoleKey.DownArrow  => (Vec2d.Down, false),
                ConsoleKey.Q or ConsoleKey.LeftArrow  => (Vec2d.Left, false),
                ConsoleKey.D or ConsoleKey.RightArrow => (Vec2d.Right, false),
                ConsoleKey.Escape                     => (new Vec2d(0, 0), true),
                _                                     => (new Vec2d(0, 0), false),
            };
        }

        public bool IsUpPressed => Console.KeyAvailable && Console.ReadKey(false).Key is ConsoleKey.Z or ConsoleKey.UpArrow;
        public bool IsDownPressed => Console.KeyAvailable && Console.ReadKey(false).Key is ConsoleKey.S or ConsoleKey.DownArrow;
        public bool IsLeftPressed => Console.KeyAvailable && Console.ReadKey(false).Key is ConsoleKey.Q or ConsoleKey.LeftArrow;
        public bool IsRightPressed => Console.KeyAvailable && Console.ReadKey(false).Key is ConsoleKey.D or ConsoleKey.RightArrow;
        public bool IsEscPressed => Console.KeyAvailable && Console.ReadKey(false).Key == ConsoleKey.Escape;
    }
}