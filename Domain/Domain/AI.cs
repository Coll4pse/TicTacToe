using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Domain
{
    /// <summary>
    ///     Класс ИИ выбирающий ход на основе построенного полного древа решений
    /// </summary>
    public class Ai : IPlayer
    {
        private static readonly Dictionary<GameWinner, int> crossScoreMap = new Dictionary<GameWinner, int>
        {
            {GameWinner.Crosses, 100},
            {GameWinner.Noughts, -100},
            {GameWinner.Draw, 0}
        };

        private static readonly Dictionary<GameWinner, int> noughtsScoreMap = new Dictionary<GameWinner, int>
        {
            {GameWinner.Crosses, -100},
            {GameWinner.Noughts, 100},
            {GameWinner.Draw, 0}
        };

        private static readonly Dictionary<CellInstance, CellInstance> nextInstance =
            new Dictionary<CellInstance, CellInstance>
            {
                {CellInstance.Cross, CellInstance.Nought},
                {CellInstance.Nought, CellInstance.Cross}
            };
        
        public Point MakeMove(GameGrid gameGrid, CellInstance instance)
        {
            return BestMove(gameGrid, instance);
        }

        private static Point BestMove(GameGrid gameGrid, CellInstance instance)
        {
            var bestScore = int.MinValue;
            Point move = default;
            Parallel.ForEach(gameGrid.GetEmptyCells(), (emptyCell) =>
            {
                var maxDepth = gameGrid.Size == 3 ? 9 : gameGrid.GetEmptyCells().Count() % 5;
                var score = Minimax(gameGrid.SetCellInstance(emptyCell, instance), 0, maxDepth, nextInstance[instance],
                    instance == CellInstance.Cross ? crossScoreMap : noughtsScoreMap, false);
                if (score > bestScore)
                {
                    bestScore = score;
                    move = emptyCell;
                }
            }); 
            
            return move;
        }

        private static int Minimax(GameGrid gameGrid, int depth, int maxDepth, CellInstance instance,
            Dictionary<GameWinner, int> scoreMap, bool isMaximizing)
        {
            var result = Game.CheckWinner(gameGrid);
            if (result != GameWinner.None)
                return scoreMap[result] - depth;

            if (depth == maxDepth)
                return 0;

            if (isMaximizing)
            {
                var bestScore = int.MinValue;
                Parallel.ForEach(gameGrid.GetEmptyCells(), (emptyCell) =>
                {
                    var score = Minimax(gameGrid.SetCellInstance(emptyCell, instance), depth + 1, maxDepth,
                        nextInstance[instance], scoreMap, false);
                    bestScore = Math.Max(bestScore, score);
                });

                return bestScore;
            }
            else
            {
                var bestScore = int.MaxValue;
                Parallel.ForEach(gameGrid.GetEmptyCells(), (emptyCell) =>
                {
                    var score = Minimax(gameGrid.SetCellInstance(emptyCell, instance), depth + 1, maxDepth,
                        nextInstance[instance], scoreMap, true);
                    bestScore = Math.Min(bestScore, score);
                });

                return bestScore;
            }
        }
    }
}