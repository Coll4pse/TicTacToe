using System.Drawing;

namespace Domain.Domain
{
    public interface IUi
    {
        void DrawInstance(Point point, CellInstance instance);
    }
}