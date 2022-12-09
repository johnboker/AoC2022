using System.Text.RegularExpressions;

namespace AoC2022.Solutions
{
    public class Solution05 : ISolution
    {
        public Solution05()
        {
            Ship = new CargoShip();
            Operations = new List<CraneOperation>();
            Input = new string[] { };
        }

        public CargoShip Ship { get; set; }
        public List<CraneOperation> Operations { get; set; }

        public string[] Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = await File.ReadAllLinesAsync(file);
        }

        private void Initialize()
        {
            Ship = new CargoShip();
            Operations = new List<CraneOperation>();

            var section = 0;
            var stackCount = Input[0].Length / 4 + 1;
            for (var i = 0; i < stackCount; i++)
            {
                Ship.Stacks.Add(new Stack<string>());
            }

            foreach (var line in Input)
            {
                if (section == 0 && string.IsNullOrWhiteSpace(line))
                {
                    section = 1;
                    continue;
                }

                if (section == 0)
                {
                    for (int i = 0; i < stackCount; i++)
                    {
                        if (line.Contains("["))
                        {
                            var cargo = line.Substring(i * 4, 3);
                            if (!string.IsNullOrWhiteSpace(cargo))
                            {
                                Ship.Stacks[i].Push(cargo.Trim(new[] { '[', ']' }));
                            }
                        }
                    }
                }
                else
                {
                    Operations.Add(new CraneOperation(line));
                }
            }

            foreach (var stack in Ship.Stacks)
            {
                var list = new List<string>();
                while (stack.Count() > 0)
                {
                    var item = stack.Pop();
                    list.Add(item);
                }
                foreach (var item in list)
                {
                    stack.Push(item);
                }
            }
        }

        public void Solve1()
        {
            Initialize();

            foreach (var operation in Operations)
            {
                operation.ApplyTo9000(Ship);
            }
            foreach (var c in Ship.Stacks)
            {
                Console.Write(c.Peek());
            }
        }
        public void Solve2()
        {

            Initialize();

            foreach (var operation in Operations)
            {
                operation.ApplyTo9001(Ship);
            }
            foreach (var c in Ship.Stacks)
            {
                Console.Write(c.Peek());
            }
        }

        public class CraneOperation
        {
            private Regex LineFormatRegex = new Regex(@"move (?<move>\d+) from (?<from>\d+) to (?<to>\d+)");
            public CraneOperation(string line)
            {
                var match = LineFormatRegex.Match(line);
                Number = Convert.ToInt32(match.Groups["move"].Value);
                FromStack = Convert.ToInt32(match.Groups["from"].Value);
                ToStack = Convert.ToInt32(match.Groups["to"].Value);
            }
            public int Number { get; set; }
            public int FromStack { get; set; }
            public int ToStack { get; set; }

            public void ApplyTo9001(CargoShip ship)
            {
                var from = ship.Stacks[FromStack - 1];
                var to = ship.Stacks[ToStack - 1];

                var list = new List<string>();
                for (var i = 0; i < Number; i++)
                {
                    list.Add(from.Pop());
                }
                list.Reverse();
                foreach (var item in list)
                {
                    to.Push(item);
                }
            }

            public void ApplyTo9000(CargoShip ship)
            {
                var from = ship.Stacks[FromStack - 1];
                var to = ship.Stacks[ToStack - 1];

                for (var i = 0; i < Number; i++)
                {
                    to.Push(from.Pop());
                }
            }
        }

        public class CargoShip
        {
            public CargoShip()
            {
                Stacks = new List<Stack<string>>();
            }
            public List<Stack<string>> Stacks { get; set; }
        }

    }
}