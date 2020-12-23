using System.Drawing;

namespace Domain.Domain
{
    /// <summary>
    ///     Базовый интерфейс сущности игрока
    /// </summary>
    public interface IPlayer
    {
        Point MakeMove(GameGrid gameGrid, CellInstance instance);
    }
}