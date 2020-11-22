using System;
using System.Collections.Generic;
using Domain.Infrastructure;

namespace Domain.Domain
{
    public class Game : Entity<int>
    {
        private readonly IUi ui;

        private readonly Dictionary<IPlayer, CellInstance> players;
        public GameGrid GameGrid { get; private set; }
        
        public IPlayer CurrentPlayer { get; private set; }

        public IPlayer CrossesPlayer { get; }

        public IPlayer NoughtsPlayer { get; }
        
        public GameStatus Status { get; private set; }
        
        public GameWinner Winner { get; private set; }
        
        public List<GameGrid> StepsHistory { get; }

        public Game(int id, int girdSize, IPlayer crossesPlayer, IPlayer noughtsPlayer, IUi ui) : base(id)
        {
            GameGrid = new GameGrid(girdSize);
            StepsHistory.Add(GameGrid);
            players = new Dictionary<IPlayer, CellInstance>(2)
            {
                [crossesPlayer] = CellInstance.Cross, [noughtsPlayer] = CellInstance.Nought
            };
            CrossesPlayer = crossesPlayer;
            NoughtsPlayer = noughtsPlayer;
            this.ui = ui;
        }

        public void Start()
        {
            if (Status != GameStatus.ReadyToStart)
                throw new InvalidOperationException($"Game isn't ready to start. Status: {Status}");
            
            CurrentPlayer = CrossesPlayer;
            Status = GameStatus.InProcess;
            
            while (Status != GameStatus.Ended || Status != GameStatus.Aborted)
            {
                var point = CurrentPlayer.MakeMove(GameGrid);
                GameGrid = GameGrid.SetCellInstance(point ,players[CurrentPlayer]);
                ui.DrawInstance(point, players[CurrentPlayer]);
                StepsHistory.Add(GameGrid);
                NextCurrentPlayer();
                TryChangeStatus();
            }
        }

        public void Abort()
        {
            Status = GameStatus.Aborted;
        }
        
        
        private void NextCurrentPlayer()
        {
            CurrentPlayer = CurrentPlayer == CrossesPlayer ? NoughtsPlayer : CrossesPlayer;
        }

        private void TryChangeStatus()
        {
            var winner = CheckWinner(GameGrid);
            if (winner == GameWinner.None) return;
            Winner = winner;
            Status = GameStatus.Ended;
        }

        #region Static Methods
        
        public static GameWinner CheckWinner(GameGrid grid)
        {
            if (TryGetWinnerFromMainDiagonal(grid, out var winner))
                return winner;
            if (TryGetWinnerFromSecondaryDiagonal(grid, out winner))
                return winner;
            if (TryGetWinnerFromRows(grid, out winner))
                return winner;
            if (TryGetWinnerFromColumns(grid, out winner))
                return winner;
            return IsGameGridFilled(grid) ? GameWinner.Draw : GameWinner.None;
        }

        private static bool TryGetWinnerFromMainDiagonal(GameGrid grid, out GameWinner winner)
        {
            winner = GameWinner.None;
            
            var arr = grid.Grid;
            var possibleWinner = arr[0, 0];
            
            if (possibleWinner == CellInstance.Empty) return false;
            
            for (var i = 1; i < grid.Size; i++)
            {
                if (arr[i, i] != possibleWinner)
                    return false;
            }

            winner = possibleWinner == CellInstance.Cross ? GameWinner.Crosses : GameWinner.Noughts;
            return true;
        }

        private static bool TryGetWinnerFromSecondaryDiagonal(GameGrid grid, out GameWinner winner)
        {
            winner = GameWinner.None;
            
            var arr = grid.Grid;
            var possibleWinner = arr[grid.Size - 1, 0];
            
            if (possibleWinner == CellInstance.Empty) return false;
            
            for (var i = 1; i < grid.Size; i++)
            {
                if (arr[i, grid.Size - i - 1] != possibleWinner)
                    return false;
            }

            winner = possibleWinner == CellInstance.Cross ? GameWinner.Crosses : GameWinner.Noughts;
            return true;
        }

        private static bool TryGetWinnerFromRows(GameGrid grid, out GameWinner winner)
        {
            winner = GameWinner.None;

            var arr = grid.Grid;
            for (int i = 0; i < grid.Size; i++)
            {
                var possibleWinner = arr[i, 0];
                for (int j = 1; j < grid.Size; j++)
                {
                    if (possibleWinner != arr[i, j])
                        break;
                    if (j == grid.Size - 1)
                    {
                        winner = possibleWinner == CellInstance.Cross ? GameWinner.Crosses : GameWinner.Noughts;
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool TryGetWinnerFromColumns(GameGrid grid, out GameWinner winner)
        {
            winner = GameWinner.None;

            var arr = grid.Grid;
            for (int i = 0; i < grid.Size; i++)
            {
                var possibleWinner = arr[0, i];
                for (int j = 1; j < grid.Size; j++)
                {
                    if (possibleWinner != arr[j, i])
                        break;
                    if (j == grid.Size - 1)
                    {
                        winner = possibleWinner == CellInstance.Cross ? GameWinner.Crosses : GameWinner.Noughts;
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsGameGridFilled(GameGrid grid)
        {
            var arr = grid.Grid;
            for (int i = 0; i < grid.Size; i++)
            {
                for (int j = 0; j < grid.Size; j++)
                {
                    if (arr[i, j] == CellInstance.Empty) return false;
                }
            }

            return true;
        }
        #endregion
    }
}