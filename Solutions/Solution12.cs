using System.Numerics;

namespace AoC2022.Solutions
{
    public class Solution12 : ISolution
    {
        public string[]? Lines { get; set; }

        public char[][]? Grid { get; set; }

        public async Task ReadInput(string file)
        {
            Lines = await File.ReadAllLinesAsync(file);
            IntitializeGrid();
        }

        public void IntitializeGrid()
        {
            if (Lines != null)
            {
                Grid = new char[Lines.Length][];
                for (var i = 0; i < Lines.Length; i++)
                {
                    Grid[i] = Lines[i].ToArray();
                }

            }
        }

        public ((int X, int Y) Start, (int X, int Y) End) FindStartAndEnd()
        {
            (int X, int Y) start = (-1, -1);
            (int X, int Y) end = (-1, -1);

            if (Grid != null)
            {
                for (var y = 0; y < Grid.Count(); y++)
                {
                    for (var x = 0; x < Grid[0].Count(); x++)
                    {
                        if (Grid[y][x] == 'S') start = (x, y);
                        if (Grid[y][x] == 'E') end = (x, y);
                        if (start.X > -1 && end.X > -1) return (start, end);
                    }
                }
            }
            return (start, end);
        }

        public List<(int X, int Y)> GetValidMoves((int X, int Y) currentLocation, List<(int X, int Y)> visitedLocations)
        {
            var moves = new List<(int X, int Y)>();

            if (Grid != null)
            {
                var x = currentLocation.X;
                var y = currentLocation.Y;

                var level = Grid[y][x];

                if (y > 0 && Math.Abs((int)(Grid[y - 1][x] - level)) <= 1)
                    moves.Add((x, y - 1));

                //if (y > 0 && x > 0 && Math.Abs((int)(Grid[y - 1][x - 1] - level)) <= 1)
                //    moves.Add((x - 1, y - 1));

                if (x > 0 && Math.Abs((int)(Grid[y][x - 1] - level)) <= 1)
                    moves.Add((x - 1, y));

                //if (y + 1 < Grid.Length && x > 0 && Math.Abs((int)(Grid[y + 1][x - 1] - level)) <= 1)
                //    moves.Add((x - 1, y + 1));

                if (y + 1 < Grid.Length && Math.Abs((int)(Grid[y + 1][x] - level)) <= 1)
                    moves.Add((x, y + 1));

                //if (y + 1 < Grid.Length && x + 1 < Grid[0].Length && Math.Abs((int)(Grid[y + 1][x + 1] - level)) <= 1)
                //    moves.Add((x + 1, y + 1));

                if (x + 1 < Grid[0].Length && Math.Abs((int)(Grid[y][x + 1] - level)) <= 1)
                    moves.Add((x + 1, y));

                //if (y > 0 && x + 1 < Grid[0].Length && Math.Abs((int)(Grid[y - 1][x + 1] - level)) <= 1)
                //    moves.Add((x + 1, y - 1));

                moves = moves.Where(a => !visitedLocations.Contains(a)).ToList();
            }
            return moves;
        }

        public void GetPathLength((int X, int Y) start, (int X, int Y) end)
        {
            if (Grid != null)
            {
                var visited = new List<(int X, int Y)>();
                var costs = new SortedSet<(int P, (int X, int Y) Point)>();

                visited.Add(start);
                var neighbors = GetValidMoves(start, visited);
                neighbors.ForEach(n => costs.Add((1, n)));

                var current = (P: 0, Point: start);

                while (current.Point != end && costs.Any())
                {
                    current = costs.First();
                    visited.Add(current.Point);
                    GetValidMoves(current.Point, visited).ForEach(n => costs.Add((current.P + 1, (n.X, n.Y))));
                    if (current.Point != end)
                    {
                        Console.WriteLine(current);
                        costs.Remove(current);
                    }
                }

                Console.WriteLine($"Start: {start}, End: {end}");

                if (costs.Any())
                {
                    Console.WriteLine(costs.First().P);
                }
            }
        }

        public void Solve1()
        {
            var visited = new HashSet<(int X, int Y)>();

            if (Grid != null)
            {
                var startAndEnd = FindStartAndEnd();

                Grid[startAndEnd.Start.Y][startAndEnd.Start.X] = 'a';
                Grid[startAndEnd.End.Y][startAndEnd.End.X] = 'z';

                GetPathLength(startAndEnd.Start, startAndEnd.End);
            }
        }

        public void Solve2()
        {

        }
    }
}