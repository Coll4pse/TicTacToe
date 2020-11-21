using System;
using System.Collections.Generic;
using Domain.Infrastructure;

namespace Domain.Domain
{
    public class Game : Entity<int>
    {
        private IUi ui;
        
        public GameGrid GameGrid { get; }

        public IPlayer CurrentPlayer { get; private set; }
        
        public IPlayer CrossesPlayer { get; }
        
        public  IPlayer NoughtsPlayer { get; }
        
        public GameStatus Status { get; private set; }
        
        public GameWinner Winner { get; private set; }
        
        public List<GameGrid> StepsHistory { get; }

        public Game(int id, GameGrid gameGrid, IPlayer crossesPlayer, IPlayer noughtsPlayer, IUi ui) : base(id)
        {
            GameGrid = gameGrid;
            CrossesPlayer = crossesPlayer;
            NoughtsPlayer = noughtsPlayer;
            this.ui = ui;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}