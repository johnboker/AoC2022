namespace AoC2022.Solutions
{
    public class Solution01 : ISolution
    {
        public Solution01()
        {
            Input = new string[] { };
        }
        
        private string[] Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = await File.ReadAllLinesAsync(file);
        }

        public void Solve1()
        {
            var elves = new List<Elf>();

            var elf = new Elf(1);
            elves.Add(elf);

            foreach (var line in Input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    elf = new Elf(elf.ElfNumber + 1);
                    elves.Add(elf);
                    continue;
                }
                elf.Calories.Add(Convert.ToInt32(line));
            }

            var max = elves.OrderByDescending(a => a.Calories.Sum()).First();
            Console.WriteLine($"elf {max.ElfNumber} cals: {max.Calories.Sum()}");
        }

        public void Solve2()
        {
            var elves = new List<Elf>();

            var elf = new Elf(1);
            elves.Add(elf);

            foreach (var line in Input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    elf = new Elf(elf.ElfNumber + 1);
                    elves.Add(elf);
                    continue;
                }
                elf.Calories.Add(Convert.ToInt32(line));
            }

            var top3 = elves.OrderByDescending(a => a.Calories.Sum()).Take(3);
            Console.WriteLine($"elves: {string.Join(",", top3.Select(a => a.ElfNumber))} cals: {top3.Sum(a => a.Calories.Sum())}");
        }

        public class Elf
        {
            public Elf(int elfNumber)
            {
                ElfNumber = elfNumber;
                Calories = new List<int>();
            }
            public int ElfNumber { get; set; }
            public List<int> Calories { get; set; }

            public int TotalCalories => Calories.Sum();
        }
    }
}