using System.Drawing;
using Domain.Infrastructure;

namespace Domain.Domain
{
    public class Player : Entity<string>, IPlayer
    {
        private readonly IUi ui;

        public Player(string nickname, IUi ui) : base(nickname)
        {
            this.ui = ui;
        }

        public Point MakeMove(GameGrid gameGrid, CellInstance instance)
        {
            return ui.GetMove();
        }
    }
}