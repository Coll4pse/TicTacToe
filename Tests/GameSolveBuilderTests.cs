using System.Linq;
using Domain.Infrastructure;
using NUnit.Framework;

namespace Tests
{
    public class GameSolveBuilderTests
    {
        [Test]
        public void GameGrid3X3CrossesScoreShouldBeEquals()
        {
            var trueCrossesWins = new[] {14652, 14232, 14652, 14232, 15648, 14232, 14652, 14232, 14652};
            var actualCrossesWins = GameSolveBuilder.BuildSolveTree(3)
                .Children
                .Select(c => c.Score.CrossesWinCount);
            CollectionAssert.AreEqual(trueCrossesWins, actualCrossesWins);
        }

        [Test]
        public void GameGrid3X3NoughtsScoreShouldBeEquals()
        {
            var trueNoughtsWins = new[] {7896, 10176, 7896, 10176, 5616, 10176, 7896, 10176, 7896};
            var actualNoughtsWins = GameSolveBuilder.BuildSolveTree(3)
                .Children
                .Select(c => c.Score.NoughtsWinCount);
            CollectionAssert.AreEqual(trueNoughtsWins, actualNoughtsWins);
        }

        [Test]
        public void GameGrid3X3DrawsScoreShouldBeEquals()
        {
            var trueDrawsCount = new[] {5184, 5184, 5184, 5184, 4608, 5184, 5184, 5184, 5184};
            var actrualDrawsCount = GameSolveBuilder.BuildSolveTree(3)
                .Children
                .Select(c => c.Score.DrawsCount);
            CollectionAssert.AreEqual(trueDrawsCount, actrualDrawsCount);
        }
    }
}