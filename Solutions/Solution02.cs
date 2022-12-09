using static AoC2022.Solutions.Solution02.Round;

namespace AoC2022.Solutions
{
    public class Solution02 : ISolution
    {
        public Solution02()
        {
            Rounds = new() { };
        }

        private List<Round> Rounds { get; set; }

        public async Task ReadInput(string file)
        {
            var input = await File.ReadAllLinesAsync(file);

            GameShape GetShape(string s)
            {
                return s switch
                {
                    "A" or "X" => GameShape.Rock,
                    "B" or "Y" => GameShape.Paper,
                    "C" or "Z" => GameShape.Scissors,
                    _ => throw new Exception("Undefined shape.")
                };
            }

            Outcome GetDesiredOutcome(string s)
            {
                return s switch
                {
                    "X" => Outcome.Lose,
                    "Y" => Outcome.Draw,
                    "Z" => Outcome.Win,
                    _ => throw new Exception("No outcome defined.")
                };
            }

            foreach (var line in input)
            {
                var parts = line.Split(" ");
                Rounds.Add(new Round() { Opponent = GetShape(parts[0]), Me = GetShape(parts[1]), DesiredOutcome = GetDesiredOutcome(parts[1]) });
            }
        }

        public void Solve1() => Console.WriteLine(Rounds.Sum(a => a.ScorePart1()));
        public void Solve2() => Console.WriteLine(Rounds.Sum(a => a.ScorePart2()));

        public class Round
        {
            public GameShape Opponent { get; set; }
            public GameShape Me { get; set; }
            public Outcome DesiredOutcome { get; set; }

            Dictionary<GameShape, (GameShape Win, GameShape Lose)> Game = new() {
                {GameShape.Rock, (GameShape.Paper, GameShape.Scissors)},
                {GameShape.Paper, (GameShape.Scissors, GameShape.Rock)},
                {GameShape.Scissors, (GameShape.Rock, GameShape.Paper)},
            };

            public int ScorePart1() => Score();

            public int ScorePart2()
            {
                Me = GetMyMove();
                return Score();
            }

            public int Score() => (int)Me + (int)(IsWin() ? Outcome.Win : (IsDraw() ? Outcome.Draw : Outcome.Lose));
            public bool IsDraw() => Me == Opponent;
            public bool IsWin() => Me == Game[Opponent].Win;
            public GameShape GetMyMove() => DesiredOutcome == Outcome.Win ? Game[Opponent].Win : (DesiredOutcome == Outcome.Lose ? Game[Opponent].Lose : Opponent);

            public enum GameShape
            {
                Rock = 1,
                Paper = 2,
                Scissors = 3
            }

            public enum Outcome
            {
                Win = 6,
                Lose = 0,
                Draw = 3
            }
        }
    }
}