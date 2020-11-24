using System.Drawing;
using NUnit.Framework;
using Domain.Domain;
namespace Tests
{
    public class CheckWinnerTests
    {
        [Test]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        public void IsWinnerCrossesWhenMainDiagonalFilled(int size)
        {
            var grid = MakeMainDiagonalGrid(size, CellInstance.Cross);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Crosses);
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        public void IsWinnerNoughtsWhenMainDiagonalFilled(int size)
        {
            var grid = MakeMainDiagonalGrid(size, CellInstance.Nought);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Noughts);
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(12)]
        public void IsWinnerCrossesWhenSecondaryDiagonalFilled(int size)
        {
            var grid = MakeSecondaryDiagonalGrid(size, CellInstance.Cross);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Crosses);
        }
        
        [Test]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(15)]
        public void IsWinnerNoughtsWhenSecondaryDiagonalFilled(int size)
        {
            var grid = MakeSecondaryDiagonalGrid(size, CellInstance.Nought);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Noughts);
        }

        [Test]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(13, 0)]
        [TestCase(13, 7)]
        [TestCase(5, 4)]
        [TestCase(5, 2)]
        [TestCase(4, 1)]
        public void IsWinnerCrossesWhenRowFilled(int size, int row)
        {
            var grid = MakeRowFilledGrid(size, row, CellInstance.Cross);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Crosses);
        }
        
        [Test]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(13, 0)]
        [TestCase(13, 7)]
        [TestCase(5, 4)]
        [TestCase(5, 2)]
        [TestCase(4, 1)]
        public void IsWinnerNoughtsWhenRowFilled(int size, int row)
        {
            var grid = MakeRowFilledGrid(size, row, CellInstance.Nought);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Noughts);
        }
        
        [Test]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(13, 0)]
        [TestCase(13, 7)]
        [TestCase(5, 4)]
        [TestCase(5, 2)]
        [TestCase(4, 1)]
        public void IsWinnerCrossesWhenColumnFilled(int size, int column)
        {
            var grid = MakeColumnFilledGrid(size, column, CellInstance.Cross);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Crosses);
        }
        
        [Test]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(13, 0)]
        [TestCase(13, 7)]
        [TestCase(5, 4)]
        [TestCase(5, 2)]
        [TestCase(4, 1)]
        public void IsWinnerNoughtsWhenColumnFilled(int size, int column)
        {
            var grid = MakeColumnFilledGrid(size, column, CellInstance.Nought);
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Noughts);
        }

        [Test]
        public void IsDrawWhenGridFullFilledWithoutWinner()
        {
            var grid = MakeFullFilledGridWithoutWinner();
            Assert.IsTrue(Game.CheckWinner(grid) == GameWinner.Draw);
        }

        private static GameGrid MakeSecondaryDiagonalGrid(int size, CellInstance instance)
        {
            var grid = new GameGrid(size);
            for (int i = 0; i < size; i++)
            {
                grid = grid.SetCellInstance(new Point(i, size - i - 1), instance);
            }

            return grid;
        }

        private static GameGrid MakeMainDiagonalGrid(int size, CellInstance instance)
        {
            var grid = new GameGrid(size);
            for (int i = 0; i < size; i++)
            {
                grid = grid.SetCellInstance(new Point(i, i), instance);
            }

            return grid;
        }

        private static GameGrid MakeRowFilledGrid(int size, int row, CellInstance instance)
        {
            var grid = new GameGrid(size);
            for (int i = 0; i < size; i++)
            {
                grid = grid.SetCellInstance(new Point(row, i), instance);
            }

            return grid;
        }
        
        private static GameGrid MakeColumnFilledGrid(int size, int column, CellInstance instance)
        {
            var grid = new GameGrid(size);
            for (int i = 0; i < size; i++)
            {
                grid = grid.SetCellInstance(new Point(i, column), instance);
            }

            return grid;
        }

        private static GameGrid MakeFullFilledGridWithoutWinner()
        {
            var gridTemplate = new CellInstance[,]
            {
                {CellInstance.Cross, CellInstance.Nought, CellInstance.Nought, CellInstance.Cross},
                {CellInstance.Nought, CellInstance.Nought, CellInstance.Cross, CellInstance.Cross},
                {CellInstance.Cross, CellInstance.Cross, CellInstance.Nought, CellInstance.Nought},
                {CellInstance.Nought, CellInstance.Nought, CellInstance.Nought, CellInstance.Cross}
            };
            var grid = new GameGrid(gridTemplate.GetLength(0));
            for (int i = 0; i < grid.Size; i++)
            {
                for (int j = 0; j < grid.Size; j++)
                {
                    grid = grid.SetCellInstance(new Point(i, j), gridTemplate[i, j]);
                }
            }

            return grid;
        }
    }
}