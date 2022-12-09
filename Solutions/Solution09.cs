using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC2022.Solutions
{
    public class Solution09 : ISolution
    {
        public Solution09()
        {
            Input = new();
        }

        public List<(string Direction, int Steps)> Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = (await System.IO.File.ReadAllLinesAsync(file)).Select(a => a.Split(' ')).Select(a => (a[0], Convert.ToInt32(a[1]))).ToList();
        }

        public void Solve1() => Solve(2);

        public void Solve2() => Solve(10);

        private void Solve(int knots)
        {
            var rope = new Rope(knots);
            foreach (var d in Input)
            {
                rope.Move(d.Direction, d.Steps);
            }
            Console.WriteLine(rope.TailVisitedLocations.Count());
        }

        public class Rope
        {
            public Rope(int knotCount)
            {
                Knots = new List<Location>();
                for (var i = 0; i < knotCount; i++)
                {
                    Knots.Add(new(0, 0));
                }

                TailVisitedLocations = new();
                TailVisitedLocations.Add(new(0, 0));
            }
            public List<Location> Knots { get; set; }

            public HashSet<Location> TailVisitedLocations { get; set; }

            public void Move(string direction, int steps)
            {
                for (int s = 0; s < steps; s++)
                {
                    switch (direction)
                    {
                        case "L":
                            Knots[0].X -= 1;
                            break;
                        case "R":
                            Knots[0].X += 1;
                            break;
                        case "U":
                            Knots[0].Y += 1;
                            break;
                        case "D":
                            Knots[0].Y -= 1;
                            break;
                    }

                    for (var i = 0; i < Knots.Count - 1; i++)
                    {
                        var head = Knots[i];
                        var tail = Knots[i + 1];
                        MoveTail(head, tail, i == Knots.Count - 2);
                    }
                }
            }

            public void MoveTail(Location head, Location tail, bool isLast)
            {
                var surroundingLocations = head.SurroundingLocations();
                if (!surroundingLocations.Contains(tail))
                {
                    tail.Y += Math.Sign(head.Y - tail.Y);
                    tail.X += Math.Sign(head.X - tail.X);
                    if (isLast)
                    {
                        TailVisitedLocations.Add(tail.Copy());
                    }
                }
            }
        }

        public class Location : IEquatable<Location>
        {
            public Location(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }

            public bool Equals(Location? other)
            {
                return other?.X == X && other?.Y == Y;
            }

            public override int GetHashCode()
            {
                return X.GetHashCode() + Y.GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                return Equals((Location?)obj);
            }

            public Location Copy()
            {
                return new Location(X, Y);
            }

            public List<Location> SurroundingLocations()
            {
                return new List<Location> {
                    new(X-1, Y),
                    new(X-1, Y+1),
                    new(X, Y+1),
                    new(X+1, Y+1),
                    new(X+1, Y),
                    new(X+1, Y-1),
                    new(X, Y-1),
                    new(X-1, Y-1),
                    new(X,Y)
                };
            }
        }
    }
}