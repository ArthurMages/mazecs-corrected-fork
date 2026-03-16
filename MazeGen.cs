using System;

namespace Epsi.MazeCs
{
    public class MazeGen : IMazeGenerator
    {
        public int Width { get; }
        public int Height { get; }
        private readonly double coinProbability;
        private readonly double doorProbability;

        public MazeGen(int width, int height, double coinProbability = 0.0, double doorProbability = 0.0)
        {
            Width = width;
            Height = height;
            this.coinProbability = coinProbability;
            this.doorProbability = doorProbability;
        }

        public Cell[,] Generate()
        {
            var grid = new Cell[Width, Height];
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    grid[x, y] = new Wall();

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
            grid[outPos.X, outPos.Y] = new Room { IsExit = true };

            void GenerateMazeRec(Vec2d p)
            {
                grid[p.X, p.Y] = new Room();
                foreach (var dir in orders[rng.Next(orders.Length)])
                {
                    var delta = dirs[dir];
                    var next = p + delta * 2;
                    if (next.InBounds(Width, Height) && grid[next.X, next.Y] is Wall)
                    {
                        var between = p + delta;
                        grid[between.X, between.Y] = new Room();
                        GenerateMazeRec(next);
                    }
                }

                // Place key in the room after recursion
                if (grid[p.X, p.Y] is Room room && !room.IsStart && !room.IsExit && rng.NextDouble() < 0.5) // 50% chance for key on path
                {
                    room.Collectable = new Key();
                }

                // Optionally place a door on adjacent walls
                foreach (var dir in dirs)
                {
                    var wallPos = p + dir;
                    if (wallPos.InBounds(Width, Height) && grid[wallPos.X, wallPos.Y] is Wall && rng.NextDouble() < doorProbability)
                    {
                        grid[wallPos.X, wallPos.Y] = new Door();
                    }
                }
            }

            // Add coins to rooms with probability
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (grid[x, y] is Room room && !room.IsStart && !room.IsExit && rng.NextDouble() < coinProbability)
                    {
                        room.Collectable = new Coin();
                    }
                }
            }

            return grid;
        }
    }
}