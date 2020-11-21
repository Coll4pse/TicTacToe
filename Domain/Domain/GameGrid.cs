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

        public GameGrid(int rowCount, int columnCount) : this(new Size(rowCount, columnCount)) {}

        public CellInstance this[int row, int column]
        {
            get => grid[row, column];
            set => grid[row, column] = value;
        }
    }
}