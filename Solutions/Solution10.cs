using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC2022.Solutions
{
    public class Solution10 : ISolution
    {
        public Solution10()
        {
            Instructions = new();
        }

        public List<(string Instruction, int Value)> Instructions { get; set; }

        public async Task ReadInput(string file)
        {
            Instructions = (await System.IO.File.ReadAllLinesAsync(file)).Select(a => a.Split(' ')).Select(a => (a[0], a.Length > 1 ? Convert.ToInt32(a[1]) : 0)).ToList();
        }

        public void Solve1()
        {
            var computer = new Computer();
            computer.ProcessInstructions(Instructions);
        }

        public void Solve2()
        {

        }

        public class Computer
        {
            public int X { get; set; }
            public int Cycle { get; set; }
            public int InstructionCycle { get; set; }

            public Computer()
            {
                X = 1;
            }

            public void ProcessInstructions(List<(string Instruction, int Value)> instructions)
            {
                var sumSignalStrengths = 0;
                foreach (var instr in instructions)
                {
                    var cyclesToComplete = instr.Instruction switch
                    {
                        "addx" => 2,
                        "noop" => 1,
                        _ => 0 // invalid instruction
                    };

                    for (var c = 0; c < cyclesToComplete; c++)
                    {
                        Cycle++;

                        if ((Cycle + 20) % 40 == 0)
                        {
                            sumSignalStrengths += Cycle * X;
                        }

                        int pixelLocation = (Cycle % 40) - 1;

                        if (Math.Abs(X - pixelLocation) <= 1)
                        {
                            Console.Write('#');
                        }
                        else
                        {
                            Console.Write('.');
                        }

                        //Console.WriteLine($" {Cycle} {pixelLocation} {Cycle % 40} {X} {Math.Abs((Cycle % 40) - X)}");

                        if (Cycle % 40 == 0)
                        {
                            Console.WriteLine();
                        }
                    }

                    if (instr.Instruction == "addx")
                    {
                        X += instr.Value;
                    }

                    // Console.WriteLine($"Cycle: {Cycle}, X: {X}");
                }

                Console.WriteLine($"Sum Signal Strengths: {sumSignalStrengths}");
            }
        }

    }
}