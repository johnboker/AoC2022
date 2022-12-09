using System.Text.RegularExpressions;

namespace AoC2022.Solutions
{
    public class Solution06 : ISolution
    {
        public Solution06()
        {
            Input = new List<string>();
        }

        public List<string> Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = (await File.ReadAllLinesAsync(file)).ToList();
        }

        public void Solve1() => Input.ForEach(a => Solve(a, 4));

        public void Solve2() => Input.ForEach(a => Solve(a, 14));

        public void Solve(string line, int characters)
        {
            var markerLocation = GetMarkerLocation(line, characters);
            Console.WriteLine($"{markerLocation}");
        }

        public int GetMarkerLocation(string line, int characters)
        {
            for (var i = 0; i < line.Length - characters; i++)
            {
                var str = line[i..(i + characters)];
                if (str.Distinct().Count() == characters) return i + characters;
            }
            return -1;
        }
    }
}