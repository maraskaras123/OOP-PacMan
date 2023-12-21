using PacMan.Shared.Enums;
using PacMan.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Shared.Patterns.Iterator
{
    public class EmptyTileIterator : Iterator
    {
        private TileGrid _tileGrid;

        private List<string> keys = new List<string>();

        private int _position = -1;

        private int count = 0;

        public EmptyTileIterator(TileGrid tileGrid)
        {
            _tileGrid = tileGrid;

            keys = _tileGrid.Tiles
                .Where(pair => pair.Value.Type == EnumTileType.Empty)
                .Select(pair => pair.Key)
                .ToList();
        }

        public override Tile Current()
        {
            return this._tileGrid.Tiles[keys[_position]];
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
            if (updatedPosition < keys.Count)
            {
                _position = updatedPosition;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
