using System;
using System.Drawing;
using Domain.Infrastructure;

namespace Domain.Domain
{
    public class GameGrid : ValueType<GameGrid>
    {
        private readonly CellInstance[,] grid;

        public CellInstance[,] Grid
        {
            get
            {
                var copyGrid = new CellInstance[Size.Width, Size.Height];
                Array.Copy(grid, copyGrid, Size.Width * Size.Height);
                return copyGrid;
            }
        }

        public Size Size { get; }

        public GameGrid(Size size)
        {
            Size = size;
            grid = new CellInstance[size.Width, size.Height];
        }

        private GameGrid(CellInstance[,] grid) : this(grid.GetLength(0), grid.GetLength(1))
        {
            for (var i = 0; i < Size.Width; i++)
            {
                for (var j = 0; j < Size.Height; j++)
                {
                    this.grid[i, j] = grid[i, j];
                }
            }
        }

        public GameGrid(int rowCount, int columnCount) : this(new Size(rowCount, columnCount)) {}

        public GameGrid SetCellInstance(Point point, CellInstance instance)
        {
            var result = new GameGrid(Grid);
            result.grid[point.X, point.Y] = instance;
            return result;
        }
    }
}