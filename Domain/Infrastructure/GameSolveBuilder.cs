using System;
using System.Collections.Generic;
using System.Drawing;
using Domain.Domain;

namespace Domain.Infrastructure
{
    public static class GameSolveBuilder
    {
        private const CellInstance StartInstance = CellInstance.Cross;
        
        public static GameGridTree BuildSolveTree(int gridSize)
        {
            var tree = new GameGridTree(new GameGrid(gridSize));
            InspectTree(tree, StartInstance);
            return tree;
        }

        private static void InspectTree(GameGridTree tree, CellInstance instance)
        {
            switch (Game.CheckWinner(tree.Grid))
                {
                    case GameWinner.Crosses:
                        tree.Score.CrossesWinCount += 1;
                        break;
                    case GameWinner.Noughts:
                        tree.Score.NoughtsWinCount += 1;
                        break;
                    case GameWinner.Draw:
                        tree.Score.DrawsCount += 1;
                        break;
                    case GameWinner.None:
                        var nextInstance = instance == CellInstance.Cross ? CellInstance.Nought : CellInstance.Cross;
                        foreach (var cell in tree.Grid.GetEmptyCells())
                        {
                            var child = new GameGridTree(tree.Grid.SetCellInstance(cell, instance));
                            tree.AddChild(child);
                            InspectTree(child, nextInstance);
                            tree.Score += child.Score;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        private static IEnumerable<Point> GetEmptyCells(GameGrid grid)
        {
            for (var i = 0; i < grid.Size; i++)
            {
                for (var j = 0; j < grid.Size; j++)
                {
                    if (grid.Grid[i, j] != CellInstance.Empty) continue;
                    yield return new Point(i, j);
                }
            }
            
        }
    }
}