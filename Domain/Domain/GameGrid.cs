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
                var copyGrid = new CellInstance[Size, Size];
                Array.Copy(grid, copyGrid, Size * Size);
                return copyGrid;
            }
        }

        public int Size { get; }

        public GameGrid(int size)
        {
            Size = size;
            grid = new CellInstance[size, size];
        }

        private GameGrid(GameGrid grid) : this(grid.Size)
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    this.grid[i, j] = grid.grid[i, j];
                }
            }
        }
        
        public GameGrid SetCellInstance(Point point, CellInstance instance)
        {
            var result = new GameGrid(this);
            result.grid[point.X, point.Y] = instance;
            return result;
        }
    }
}