using System.Drawing;

namespace Domain.Domain
{
    public interface IPlayer
    {
        Point MakeMove(GameGrid gameGrid, CellInstance instance);
    }
}