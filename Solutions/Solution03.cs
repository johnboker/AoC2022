using System.Linq;
using static AoC2022.Solutions.Solution02.Round;

namespace AoC2022.Solutions
{
    public class Solution03 : ISolution
    {
        public Solution03()
        {
            RuckSacks = new() { };
        }

        private List<RuckSack> RuckSacks { get; set; }

        public async Task ReadInput(string file)
        {
            var input = await File.ReadAllLinesAsync(file);
            foreach (var line in input)
            {
                RuckSacks.Add(new RuckSack(line));
            }
        }

        public void Solve1()
        {
            Console.WriteLine(RuckSacks.Sum(r => RuckSack.ItemPriority(r.Intersection[0])));
        }
        public void Solve2()
        {
            var sum = 0;
            for (var i = 0; i < RuckSacks.Count(); i += 3)
            {
                var elfGroup = RuckSacks.Skip(i).Take(3).ToList();
                var intersection = elfGroup[0].AllContents.Intersect(elfGroup[1].AllContents).ToList();
                intersection = intersection.Intersect(elfGroup[2].AllContents).ToList();
                sum += RuckSack.ItemPriority(intersection[0]);
            }
            Console.WriteLine(sum);
        }

        public class RuckSack
        {
            public RuckSack(string input)
            {
                Compartment1 = input.Substring(0, input.Length / 2).ToArray();
                Compartment2 = input.Substring(input.Length / 2).ToArray();
            }

            public char[] AllContents => (string.Join("", Compartment1) + string.Join("", Compartment2)).ToArray();
            public char[] Compartment1 { get; set; }
            public char[] Compartment2 { get; set; }

            public char[] Intersection => string.Join("", Compartment1.Intersect(Compartment2))?.ToArray() ?? new[] { '-' };
            public static int ItemPriority(char c) => c - ((c >= 'a' && c <= 'z') ? 'a' : ('A' - 26)) + 1;
        }
    }
}