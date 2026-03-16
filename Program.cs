using System;
using Epsi.MazeCs;

namespace Epsi.MazeCs;

const int width = 50;
const int height = 20;

var screen = new ConsoleScreen();
var keyboard = new KeyboardController();

var mazeGen = new MazeGen(width, height, 0.1, 0.05);
var maze = new Maze(mazeGen);
var player = new Player(maze, screen, keyboard);
player.PointsChanged += points => screen.DrawTextXY(0, 0, $"Points: {points}", screen.SuccessColor);
player.InventoryChanged += () =>
{
    var inventoryText = string.Join(", ", player.Inventory.Select(c => $"{c.GetType().Name} ({c.Points} pts)"));
    screen.DrawTextXY(0, 1, $"Inventory: {inventoryText}", screen.InfoColor);
};
var mode = State.Playing;

maze.Draw(screen);
player.Draw();

while (mode == State.Playing)
{
    if (!player.Update(out var reachedExit))
    {
        mode = State.Canceled;
        break;
    }
    if (reachedExit)
        mode = State.Won;
}

var endMessage = mode == State.Won ? screen.WinText : screen.CanceledText;
var endColor = mode == State.Won ? screen.SuccessColor : screen.DangerColor;
screen.DrawFramedText(0, screen.OffsetY + height + screen.MarginYMessage, endMessage, endColor);
screen.DrawTextXY(0, screen.OffsetY + height + screen.MarginYMessage + screen.MessageHeight, screen.PressKey);
Console.CursorVisible = true;
Console.ReadKey(true);



