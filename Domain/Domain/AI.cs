using System;
using System.Drawing;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.Domain
{
    public class Ai : Entity<string>, IPlayer
    {
        private GameGridTree solveTree;
        
        public Point MakeMove(GameGrid gameGrid, CellInstance instance)
        {
            solveTree ??= GameSolveBuilder.BuildSolveTree(gameGrid.Size);
            ActualizeSolveTree(gameGrid);
            DropSolveTree(instance);
            return FindMovePoint(gameGrid);
        }

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

        private void DropSolveTree(CellInstance instance)
        {
            solveTree = instance switch
            {
                CellInstance.Cross => solveTree.Children.OrderBy(c => c.Score.CrossesWinCount).First(),
                CellInstance.Nought => solveTree.Children.OrderBy(c => c.Score.NoughtsWinCount).First(),
                CellInstance.Empty => throw new ArgumentException()
            };
        }

        private void ActualizeSolveTree(GameGrid gameGrid)
        {
            if (!gameGrid.Equals(solveTree.Grid))
            {
                solveTree = solveTree.Children.Single(c => c.Grid.Equals(gameGrid));
            }
        }

        public Ai(string id) : base(id)
        {
        }
    }
}