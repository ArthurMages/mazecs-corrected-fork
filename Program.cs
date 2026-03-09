using System;
using Epsi.MazeCs;

namespace Epsi.MazeCs;

const int width = 50;
const int height = 20;

var screen = new ConsoleScreen();
var keyboard = new KeyboardController();

var grid = new CellType[width, height];

var playerPos = new Vec2d(0, 0);
var mode = State.Playing;

GenerateMaze(grid, playerPos);
DrawScreen();

while (mode == State.Playing)
{
    var (delta, canceled) = keyboard.ReadInput();
    if (canceled)
    {
        mode = State.Canceled;
        break;
    }

    var newPos = playerPos + delta;
    if (newPos.InBounds(width, height) && grid[newPos.X, newPos.Y] != CellType.Wall)
    {
        if (grid[newPos.X, newPos.Y] == CellType.Exit) mode = State.Won;

        UpdateCell(playerPos, CellType.Corridor);
        UpdateCell(playerPos = newPos, CellType.Player);
    }
}

var endMessage = mode == State.Won ? screen.WinText : screen.CanceledText;
var endColor = mode == State.Won ? screen.SuccessColor : screen.DangerColor;
screen.DrawFramedText(0, screen.OffsetY + height + screen.MarginYMessage, endMessage, endColor);
screen.DrawTextXY(0, screen.OffsetY + height + screen.MarginYMessage + screen.MessageHeight, screen.PressKey);
Console.CursorVisible = true;
Console.ReadKey(true);

#region Functions

void UpdateCell(Vec2d pos, CellType type)
{
    grid[pos.X, pos.Y] = type;
    screen.DrawCell(pos.X, pos.Y, type);
}

void DrawScreen()
{
    Console.Clear();
    Console.CursorVisible = false;

    screen.DrawFramedText(0, 0, screen.HeaderText, screen.InfoColor);
    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            screen.DrawCell(x, y, grid[x, y]);
        }
    }
    screen.DrawTextXY(0, screen.OffsetY + height, screen.Instructions, screen.InstructionColor);
}

void GenerateMaze(CellType[,] grid, Vec2d playerStart)
{
    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            grid[x, y] = CellType.Wall;

    int[][] orders = [
        [ 0, 1, 2, 3 ], [ 0, 1, 3, 2 ], [ 0, 2, 1, 3 ], [ 0, 2, 3, 1 ], [ 0, 3, 1, 2 ], [ 0, 3, 2, 1 ],
        [ 1, 0, 2, 3 ], [ 1, 0, 3, 2 ], [ 1, 2, 0, 3 ], [ 1, 2, 3, 0 ], [ 1, 3, 0, 2 ], [ 1, 3, 2, 0 ],
        [ 2, 0, 1, 3 ], [ 2, 0, 3, 1 ], [ 2, 1, 0, 3 ], [ 2, 1, 3, 0 ], [ 2, 3, 0, 1 ], [ 2, 3, 1, 0 ],
        [ 3, 0, 1, 2 ], [ 3, 0, 2, 1 ], [ 3, 1, 0, 2 ], [ 3, 1, 2, 0 ], [ 3, 2, 0, 1 ], [ 3, 2, 1, 0 ]
    ];
    var rng = new Random();
    var dirs = Vec2d.Directions;

    GenerateMazeRec(playerStart);

    var outPos = new Vec2d((width  - 1) & ~1, (height - 1) & ~1);

    grid[playerStart.X, playerStart.Y] = CellType.Player;
    grid[outPos.X, outPos.Y] = CellType.Exit;
    
    void GenerateMazeRec(Vec2d p)
    {
        grid[p.X, p.Y] = CellType.Corridor;
        foreach (var dir in orders[rng.Next(orders.Length)])
        {
            var delta = dirs[dir];
            var next = p + delta * 2;
            if (next.InBounds(width, height) && grid[next.X, next.Y] == CellType.Wall)
            {
                var between = p + delta;
                grid[between.X, between.Y] = CellType.Corridor;
                GenerateMazeRec(next);
            }
        }
    }
}
#endregion

