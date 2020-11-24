using System;
using System.Collections.Generic;
using Domain.Domain;

namespace Domain.Infrastructure
{
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