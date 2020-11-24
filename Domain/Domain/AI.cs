using System;
using System.Drawing;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.Domain
{
    /// <summary>
    /// Класс ИИ выбирающий ход на основе построенного полного древа решений
    /// </summary>
    public class Ai : Entity<string>, IPlayer
    {
        private GameGridTree solveTree;
        
        public Ai(string id) : base(id)
        {
        }
        
        public Point MakeMove(GameGrid gameGrid, CellInstance instance)
        {
            solveTree ??= GameSolveBuilder.BuildSolveTree(gameGrid.Size);
            ActualizeSolveTree(gameGrid);
            DropSolveTree(instance);
            return FindMovePoint(gameGrid);
        }

        /// <summary>
        /// Метод который ищет разницу между акутальной сеткой и сеткой в текущем узле дерева решений
        /// </summary>
        /// <param name="sourceGrid">Актуальная сетка</param>
        /// <returns>Возвращает точку в которой сетки различны</returns>
        private Point FindMovePoint(GameGrid sourceGrid)
        {
            for (int i = 0; i < sourceGrid.Size; i++)
            {
                for (int j = 0; j < sourceGrid.Size; j++)
                {
                    if (sourceGrid.Grid[i, j] != solveTree.Grid.Grid[i, j])
                        return new Point(i, j);
                }
            }
            throw new ArithmeticException();
        }

        /// <summary>
        /// Метод продвигающий дерево к наилучшей сетке в древе решений, которая станет следующей актуальной
        /// </summary>
        /// <param name="instance"></param>
        private void DropSolveTree(CellInstance instance)
        {
            solveTree = instance switch
            {
                CellInstance.Cross => solveTree.Children.OrderBy(c => c.Score.CrossesWinCount).First(),
                CellInstance.Nought => solveTree.Children.OrderBy(c => c.Score.NoughtsWinCount).First(),
                CellInstance.Empty => throw new ArgumentException()
            };
        }

        /// <summary>
        /// Метод сдвигающий дерево к сетке, соответсвуюшей ходу противника
        /// </summary>
        /// <param name="gameGrid">Актуальная сетка игры</param>
        private void ActualizeSolveTree(GameGrid gameGrid)
        {
            if (!gameGrid.Equals(solveTree.Grid))
            {
                solveTree = solveTree.Children.Single(c => c.Grid.Equals(gameGrid));
            }
        }
    }
}