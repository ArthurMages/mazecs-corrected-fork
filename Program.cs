using System;
using Epsi.MazeCs;

namespace Epsi.MazeCs;

#region Constants
const int width = 50;
const int height = 20;

const int offsetY = 3;
const int offsetX = 0;

const int marginYMessage = 3;
const int messageHeight = 5;

const string sHeader = """
    ╔══════════════════════════════════════════════════╗
    ║          🏃 LABYRINTHE ASCII  C#  🏃             ║
    ╚══════════════════════════════════════════════════╝
    """;
const string sInstructions = "  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter";
const string sWin = """
    ╔════════════════════════════════════════╗
    ║   🎉  FÉLICITATIONS !  🎉      ║
    ║   Vous avez trouvé la sortie ! ║
    ╚════════════════════════════════════════╝
""";
const string sCanceled = "\n  Partie abandonnée. À bientôt !";
const string sPressKey = "  Appuyez sur une key pour quitter...";

const ConsoleColor SuccessColor     = ConsoleColor.Green;
const ConsoleColor DangerColor      = ConsoleColor.Red;
const ConsoleColor InfoColor        = ConsoleColor.Cyan;
const ConsoleColor InstructionColor = ConsoleColor.DarkCyan;
const ConsoleColor WallColor        = ConsoleColor.DarkGray;
const ConsoleColor CorridorColor    = ConsoleColor.DarkBlue;
const ConsoleColor PlayerColor      = ConsoleColor.Yellow;
const ConsoleColor ExitColor        = ConsoleColor.Green;
#endregion 

var grid = new CellType[width, height];

var playerPos = new Vec2d(0, 0);
var mode = State.Playing;

GenerateMaze(grid, playerPos);
DrawScreen();

while (mode == State.Playing)
{
    var key = Console.ReadKey(true).Key;

    var newPos = playerPos;

    switch (key)
    {
        case ConsoleKey.Z or ConsoleKey.UpArrow:    newPos = playerPos + Vec2d.Up; break;
        case ConsoleKey.S or ConsoleKey.DownArrow:  newPos = playerPos + Vec2d.Down; break;
        case ConsoleKey.Q or ConsoleKey.LeftArrow:  newPos = playerPos + Vec2d.Left; break;
        case ConsoleKey.D or ConsoleKey.RightArrow: newPos = playerPos + Vec2d.Right; break;
        case ConsoleKey.Escape: mode = State.Canceled; break;
    }
    if (newPos.InBounds(width, height) && grid[newPos.X, newPos.Y] != CellType.Wall)
    {
        if (grid[newPos.X, newPos.Y] == CellType.Exit) mode = State.Won;

        UpdateCell(playerPos, CellType.Corridor);
        UpdateCell(playerPos = newPos, CellType.Player  );
    }
}

DrawTextColorXY(0, offsetY + height + marginYMessage,
    mode == State.Won 
    ? (sWin, SuccessColor) 
    : (sCanceled, DangerColor)
);
DrawTextXY(0, offsetY + height + marginYMessage + messageHeight, sPressKey);
Console.CursorVisible = true;
Console.ReadKey(true);

#region Functions

void DrawTextXY(int x, int y, string text, ConsoleColor? color = null)
{
    Console.SetCursorPosition(x, y);
    if(color.HasValue)
    {
        Console.ForegroundColor = color.Value;
    }
    Console.Write(text);
    Console.ResetColor();
}

void DrawTextColorXY(int x, int y, (string text, ConsoleColor color) info) =>
    DrawTextXY(x, y, info.text, info.color);

void DrawCell(Vec2d pos) => DrawTextColorXY(
    offsetX + pos.X, 
    offsetY + pos.Y,
    grid[pos.X, pos.Y] switch
    {
        CellType.Wall   => ("█", WallColor),
        CellType.Player => ("@", PlayerColor),
        CellType.Exit   => ("★", ExitColor),
        _               => ("·", CorridorColor)
    });

void UpdateCell(Vec2d pos, CellType type)
{
    grid[pos.X, pos.Y] = type;
    DrawCell(pos);
}

void DrawScreen()
{
    Console.Clear();
    Console.CursorVisible = false;

    DrawTextXY(0, 0, sHeader, InfoColor);
    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            DrawCell(new Vec2d(x, y));
        }
    }
    DrawTextXY(0, offsetY + height, sInstructions, InstructionColor);
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

