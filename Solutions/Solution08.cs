using System.Text.RegularExpressions;

namespace AoC2022.Solutions
{
    public class Solution08 : ISolution
    {
        public Solution08()
        {
            Input = new List<string>();
        }

        public List<string> Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = (await System.IO.File.ReadAllLinesAsync(file)).ToList();
        }

        public void Solve1()
        {
            var fg = Console.ForegroundColor;

            var visibleTrees = 0;

            for (var row = 0; row < Input.Count(); row++)
            {
                for (var col = 0; col < Input[0].Length; col++)
                {
                    var isVisible = IsVisible(row, col);
                    visibleTrees += (isVisible ? 1 : 0);
                    Console.ForegroundColor = isVisible ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.Write(Input[row][col]);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = fg;

            Console.WriteLine(visibleTrees);
        }

        public void Solve2()
        {
            var maxScore = -1;
            for (var row = 1; row < Input.Count() - 1; row++)
            {
                for (var col = 1; col < Input[0].Length - 1; col++)
                {
                    var score = ScenicScore(row, col);
                    if (score > maxScore) maxScore = score;
                }
            }
            Console.WriteLine(maxScore);
        }


        public int ScenicScore(int row, int col)
        {
            var vleft = 0;
            for (int x = col - 1; x >= 0; x--)
            {
                vleft++;
                if (Input[row][x] >= Input[row][col])
                {
                    break;
                }
            }

            var vright = 0;
            for (int x = col + 1; x < Input[0].Length; x++)
            {
                vright++;
                if (Input[row][x] >= Input[row][col])
                {
                    break;
                }
            }

            var vtop = 0;
            for (var y = row - 1; y >= 0; y--)
            {
                vtop++;
                if (Input[y][col] >= Input[row][col])
                {
                    break;
                }
            }

            var vbottom = 0;
            for (var y = row + 1; y < Input.Count(); y++)
            {
                vbottom++;
                if (Input[y][col] >= Input[row][col])
                {
                    break;
                }
            }

            return vleft * vright * vtop * vbottom;
        }

        public bool IsVisible(int row, int col)
        {
            var height = Input[row][col];
            var visible = (left: true, right: true, top: true, bottom: true);

            for (int x = 0; x < Input[0].Length; x++)
            {
                var t2 = Input[row][x];
                if (height <= t2)
                {
                    if (x < col)
                    {
                        visible.left = false;
                        x = col;
                    }
                    else if (x > col)
                    {
                        visible.right = false;
                        break;
                    }
                }
            }

            for (int y = 0; y < Input.Count; y++)
            {
                var t2 = Input[y][col];
                if (height <= t2)
                {
                    if (y < row)
                    {
                        visible.top = false;
                        y = row;
                    }
                    else if (y > row)
                    {
                        visible.bottom = false;
                        break;
                    }
                }
            }

            return visible.top || visible.left || visible.bottom || visible.right;
        }
    }
}