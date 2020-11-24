namespace Domain.Infrastructure
{
    /// <summary>
    /// Класс счета побед крестиков и ноликов, и ничьих
    /// </summary>
    public class Score
    {
        public int CrossesWinCount;
        public int NoughtsWinCount;
        public int DrawsCount;

        public static Score operator +(Score s1, Score s2)
        {
            return new Score()
            {
                CrossesWinCount = s1.CrossesWinCount + s2.CrossesWinCount,
                NoughtsWinCount = s1.NoughtsWinCount + s2.NoughtsWinCount,
                DrawsCount = s1.DrawsCount + s2.DrawsCount
            };
        }
    }
}