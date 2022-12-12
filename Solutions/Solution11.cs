using System.Numerics;

namespace AoC2022.Solutions
{
    public class Solution11 : ISolution
    {
        public Solution11()
        {
            Monkeys = new();
        }

        public string[]? Lines { get; set; }

        public List<Monkey> Monkeys { get; set; }

        public async Task ReadInput(string file)
        {
            Lines = await File.ReadAllLinesAsync(file);
        }

        public void IntitializeMonkeys()
        {
            Monkeys = new();
            if (Lines != null)
            {
                for (var i = 0; i < Lines.Length; i += 7)
                {
                    var monkey = new Monkey(Lines[i]);
                    Monkeys.Add(monkey);

                    var items = Lines[i + 1][18..].Split(',').Select(a => a.Trim()).Select(a => BigInteger.Parse(a));
                    monkey.Items.AddRange(items);

                    var op = Lines[i + 2][19..].Split(' ');
                    monkey.Operation.LHS = op[0];
                    monkey.Operation.Operator = op[1];
                    monkey.Operation.RHS = op[2];

                    monkey.Test = Convert.ToUInt64(Lines[i + 3][21..]);
                    monkey.IfTrue = Convert.ToInt32(Lines[i + 4][29..]);
                    monkey.IfFalse = Convert.ToInt32(Lines[i + 5][30..]);
                }
            }
        }

        public void Solve1()
        {
            IntitializeMonkeys();

            for (var r = 0; r < 20; r++)
            {
                for (var i = 0; i < Monkeys.Count(); i++)
                {
                    var monkey = Monkeys[i];
                    while (monkey.Items.Any())
                    {
                        var item = monkey.Items[0];
                        monkey.Items.RemoveAt(0);
                        item = monkey.Operation.Apply(item);
                        monkey.Inspections++;
                        item = item / 3;

                        Monkeys[item % monkey.Test == 0 ? monkey.IfTrue : monkey.IfFalse].Items.Add(item);
                    }
                }
            }


            foreach (var m in Monkeys)
            {
                Console.WriteLine($"{m.Name} {string.Join(", ", m.Items)} {m.Inspections}");
            }
            var mostActive = Monkeys.OrderByDescending(a => a.Inspections).Take(2).ToList();
            Console.WriteLine(mostActive[0].Inspections * mostActive[1].Inspections);
        }

        public void Solve2()
        {
            IntitializeMonkeys();

            BigInteger bigMod = 1;
            foreach (var m in Monkeys)
            {
                bigMod *= m.Test;
            }

            for (var r = 0; r < 10000; r++)
            {
                for (var i = 0; i < Monkeys.Count(); i++)
                {
                    var monkey = Monkeys[i];
                    while (monkey.Items.Any())
                    {
                        var item = monkey.Items[0];
                        monkey.Items.RemoveAt(0);
                        item = monkey.Operation.Apply(item);
                        monkey.Inspections++;
                        var test = item % monkey.Test;
                        Monkeys[test == 0 ? monkey.IfTrue : monkey.IfFalse].Items.Add(item % bigMod);
                    }
                } 
            }

            foreach (var m in Monkeys)
            {
                Console.WriteLine($"{m.Name} {string.Join(", ", m.Items)} {m.Inspections}");
            }
            var mostActive = Monkeys.OrderByDescending(a => a.Inspections).Take(2).ToList();
            Console.WriteLine(mostActive[0].Inspections * mostActive[1].Inspections);
        }

        public class Monkey
        {
            public Monkey(string name)
            {
                Name = name;
                Items = new List<BigInteger>();
                Operation = new Operation("", "", "");
            }
            public BigInteger Inspections { get; set; }
            public string Name { get; set; }
            public List<BigInteger> Items { get; set; }
            public Operation Operation { get; set; }
            public BigInteger Test { get; set; }
            public int IfTrue { get; set; }
            public int IfFalse { get; set; }
        }

        public class Operation
        {
            public Operation(string op, string lhs, string rhs)
            {
                Operator = op;
                LHS = lhs;
                RHS = rhs;
            }
            public string Operator { get; set; }
            public string LHS { get; set; }
            public string RHS { get; set; }

            public BigInteger Apply(BigInteger item)
            {
                var lhs = LHS == "old" ? item : Convert.ToInt32(LHS);
                var rhs = RHS == "old" ? item : Convert.ToInt32(RHS);

                return Operator switch
                {
                    "*" => lhs * rhs,
                    "+" => lhs + rhs,
                    _ => 0
                };
            }
        }
    }
}