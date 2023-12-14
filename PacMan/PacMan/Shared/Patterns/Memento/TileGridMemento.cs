using PacMan.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Shared.Patterns.Memento
{
    class TileGridMemento : IMemento
    {
        private Dictionary<string, Tile> _state;
        private DateTime _date;

        public TileGridMemento(Dictionary<string, Tile> state)
        {
            _state = state;
            _date = DateTime.Now;
        }

        public Dictionary<string, Tile> GetState()
        {
            return _state;
        }

        public DateTime GetDate()
        {
            return _date;
        }
    }
}
