namespace AoC2022
{
    public interface ISolution
    {
        Task ReadInput(string file);
        void Solve1();
        void Solve2();
    }
}