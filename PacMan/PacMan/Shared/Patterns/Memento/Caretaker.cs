using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.Memento
{
    public class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private TileGrid _originator = null;

        public Caretaker(TileGrid originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            this._mementos.Add(this._originator.Save());
        }

        public void Undo()
        {
            if (this._mementos.Count == 0)
            {
                return;
            }

            var memento = this._mementos.Last();
            this._mementos.Remove(memento);

            try
            {
                this._originator.Restore(memento);
            }
            catch (Exception ex)
            {
                this.Undo();
            }
        }
    }
}
