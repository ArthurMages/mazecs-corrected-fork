namespace Epsi.MazeCs
{
    public class KeyboardController
    {
        /// <summary>
        /// Reads a key from the console and returns a movement delta and a cancellation flag.
        /// </summary>
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
    }
}