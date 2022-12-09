using System.Linq;
using static AoC2022.Solutions.Solution02.Round;

namespace AoC2022.Solutions
{
    public class Solution04 : ISolution
    {
        public Solution04()
        {
            Assignments = new List<Assignment>();
        }

        public List<Assignment> Assignments { get; set; }

        public async Task ReadInput(string file)
        {
            var input = await File.ReadAllLinesAsync(file);
            foreach (var line in input)
            {
                var pair = line.Split(",");
                foreach (var p in pair)
                {
                    var parts = p.Split('-');
                    Assignments.Add(new Assignment(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1])));
                }
            }
        }

        public void Solve1()
        {
            int sum = 0;
            for (var i = 0; i < Assignments.Count() - 2; i += 2)
            {
                var a1 = Assignments[i];
                var a2 = Assignments[i + 1];
                sum += Contained(a1.StartSection, a1.EndSection, a2.StartSection, a2.EndSection) ? 1 : 0;
            }
            Console.WriteLine(sum);
        }
        public void Solve2()
        {
            int sum = 0;
            for (var i = 0; i < Assignments.Count(); i += 2)
            {
                var a1 = Assignments[i];
                var a2 = Assignments[i + 1];
                sum += Overlap(a1.StartSection, a1.EndSection, a2.StartSection, a2.EndSection) ? 1 : 0;
            }
            Console.WriteLine(sum);
        }


        public bool Contained(int s1, int e1, int s2, int e2)
        {
            return (s1 >= s2 && e1 <= e2) || (s2 >= s1 && e2 <= e1);
        }

        public bool Overlap(int s1, int e1, int s2, int e2)
        {
            //Console.WriteLine($"{s1}-{e1},{s2}-{e2}");
            if (s2 < s1)
            {
                var t = s1;
                s1 = s2;
                s2 = t;

                t = e1;
                e1 = e2;
                e2 = t;
            }

            //Console.WriteLine(Contained(s1, e1, s2, e2) || s2 <= e1 ? "Yes" : "No");

            return Contained(s1, e1, s2, e2) || s2 <= e1;
        }

        public class Assignment
        {
            public Assignment(int start, int end)
            {
                StartSection = start;
                EndSection = end;
            }
            public int StartSection { get; set; }
            public int EndSection { get; set; }
        }
    }
}