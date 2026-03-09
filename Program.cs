using System;
using Epsi.MazeCs;

namespace Epsi.MazeCs;

const int width = 50;
const int height = 20;

var screen = new ConsoleScreen();
var keyboard = new KeyboardController();

var mazeGen = new MazeGen(width, height);
var maze = new Maze(mazeGen);
var player = new Player(maze, screen);
var mode = State.Playing;

maze.Draw(screen);
player.Draw();

while (mode == State.Playing)
{
    var (delta, canceled) = keyboard.ReadInput();
    if (canceled)
    {
        mode = State.Canceled;
        break;
    }

    if (player.TryMove(delta, out var reachedExit))
    {
        if (reachedExit)
            mode = State.Won;
    }
}

var endMessage = mode == State.Won ? screen.WinText : screen.CanceledText;
var endColor = mode == State.Won ? screen.SuccessColor : screen.DangerColor;
screen.DrawFramedText(0, screen.OffsetY + height + screen.MarginYMessage, endMessage, endColor);
screen.DrawTextXY(0, screen.OffsetY + height + screen.MarginYMessage + screen.MessageHeight, screen.PressKey);
Console.CursorVisible = true;
Console.ReadKey(true);



