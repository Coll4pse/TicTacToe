using System;
using System.Collections.Generic;
using Domain.Domain;

namespace Domain.Infrastructure
{
    /// <summary>
    /// Класс дерева сеток крестиков-ноликов вместе со счетом возможных исходов с текущей сетки
    /// </summary>
    public class GameGridTree
    {
        public GameGridTree Parent { get; private set; }
        
        public Score Score { get; set; }
        
        public GameGrid Grid { get; }

        private readonly HashSet<GameGridTree> children;
        public IEnumerable<GameGridTree> Children => children;

        public GameGridTree(GameGrid value)
        {
            Grid = value;
            Score = new Score();
            children = new HashSet<GameGridTree>();
        }

        public void AddChild(GameGridTree node)
        {
            node.Parent = this;
            children.Add(node);
        }
    }
}