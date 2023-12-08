using PacMan.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Shared.Patterns.Iterator
{
    public class TileGridIterator : Iterator
    {
        private TileGrid _tileGrid;

        private int _position = 0;
        private int _x = 0;
        private int _y = 0;

        public TileGridIterator(TileGrid tileGrid)
        {
            _tileGrid = tileGrid;
        }

        public override object Current()
        {
            return this._tileGrid.GetTile(_x, _y);
        }

        public override int Key()
        {
            return _position;
        }

        public override void Reset()
        {
            this._position = 0;
        }

        public override bool MoveNext()
        {
            int updatedPosition = this._position + 1;
            if (updatedPosition < _tileGrid.Tiles.Count)
            {
                this._position += 1;
                if (_y < _tileGrid.Width)
                {
                    _x += 1;
                }
                else
                {
                    _y += 1;
                    _x = 0;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
