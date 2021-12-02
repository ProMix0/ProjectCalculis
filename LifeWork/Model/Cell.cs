using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeWork.Model
{
    public class Cell
    {
        public bool IsAlive;
        public List<Cell> Neighbors { get; private set; } = new();

        private bool IsAliveNext;
        public void DetermineNextLiveState()
        {
            // Live cells with fewer than two live neighbors die
            // Live cells with more than three live neighbors die
            // Dead cells with three live neighbors comes alive

            int liveNeighbors = Neighbors.Where(x => x.IsAlive).Count();

            if (IsAlive)
                IsAliveNext = liveNeighbors is 2 or 3;
            else
                IsAliveNext = liveNeighbors == 3;
        }
        public Cell(bool alive)
        {
            IsAlive = alive;
        }

        public void Advance()
        {
            IsAlive = IsAliveNext;
        }
    }
}
