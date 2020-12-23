using System;
using System.Collections.Generic;
using System.Drawing;
using Domain.Infrastructure;

namespace Domain.Domain
{
    /// <summary>
    ///     Класс представляющий собой сетку игры
    /// </summary>
    public class GameGrid : ValueType<GameGrid>
    {
        private readonly CellInstance[,] grid;

        public GameGrid(int size)
        {
            Size = size;
            grid = new CellInstance[size, size];
        }

        private GameGrid(GameGrid grid) : this(grid.Size)
        {
            for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                this.grid[i, j] = grid.grid[i, j];
        }

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

        public GameGrid SetCellInstance(Point point, CellInstance instance)
        {
            var result = new GameGrid(this);
            result.grid[point.X, point.Y] = instance;
            return result;
        }

        public IEnumerable<Point> GetEmptyCells()
        {
            for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
            {
                if (grid[i, j] != CellInstance.Empty) continue;
                yield return new Point(i, j);
            }
        }

        public bool Equals(GameGrid gameGrid)
        {
            if (Size != gameGrid.Size)
                return false;
            for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                if (grid[i, j] != gameGrid.grid[i, j])
                    return false;

            return true;
        }
    }
}