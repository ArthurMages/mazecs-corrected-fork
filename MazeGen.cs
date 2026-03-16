using System;

namespace Epsi.MazeCs
{
    public class MazeGen : IMazeGenerator
    {
        public int Width { get; }
        public int Height { get; }

        public MazeGen(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public CellType[,] Generate()
        {
            var grid = new CellType[Width, Height];
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    grid[x, y] = CellType.Wall;

            int[][] orders = [
                [ 0, 1, 2, 3 ], [ 0, 1, 3, 2 ], [ 0, 2, 1, 3 ], [ 0, 2, 3, 1 ], [ 0, 3, 1, 2 ], [ 0, 3, 2, 1 ],
                [ 1, 0, 2, 3 ], [ 1, 0, 3, 2 ], [ 1, 2, 0, 3 ], [ 1, 2, 3, 0 ], [ 1, 3, 0, 2 ], [ 1, 3, 2, 0 ],
                [ 2, 0, 1, 3 ], [ 2, 0, 3, 1 ], [ 2, 1, 0, 3 ], [ 2, 1, 3, 0 ], [ 2, 3, 0, 1 ], [ 2, 3, 1, 0 ],
                [ 3, 0, 1, 2 ], [ 3, 0, 2, 1 ], [ 3, 1, 0, 2 ], [ 3, 1, 2, 0 ], [ 3, 2, 0, 1 ], [ 3, 2, 1, 0 ]
            ];
            var rng = new Random();
            var dirs = Vec2d.Directions;

            GenerateMazeRec(new Vec2d(0, 0));

            var outPos = new Vec2d((Width - 1) & ~1, (Height - 1) & ~1);
            grid[outPos.X, outPos.Y] = CellType.Exit;

            void GenerateMazeRec(Vec2d p)
            {
                grid[p.X, p.Y] = CellType.Corridor;
                foreach (var dir in orders[rng.Next(orders.Length)])
                {
                    var delta = dirs[dir];
                    var next = p + delta * 2;
                    if (next.InBounds(Width, Height) && grid[next.X, next.Y] == CellType.Wall)
                    {
                        var between = p + delta;
                        grid[between.X, between.Y] = CellType.Corridor;
                        GenerateMazeRec(next);
                    }
                }
            }

            return grid;
        }
    }
}