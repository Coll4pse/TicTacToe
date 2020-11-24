using System.Drawing;

namespace Domain.Domain
{
    /// <summary>
    /// Базовый интерфейс для GUI игры
    /// </summary>
    public interface IUi
    {
        void DrawInstance(Point point, CellInstance instance);
    }
}