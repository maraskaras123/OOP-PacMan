using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.Memento
{
    public class Caretaker
    {
        private readonly List<IMemento> _mementos = new();

        private readonly TileGrid _originator;

        public Caretaker(TileGrid originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            _mementos.Add(_originator.Save());
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                return;
            }

            var memento = _mementos.Last();
            _mementos.Remove(memento);
            try
            {
                _originator.Restore(memento);
            }
            catch (Exception)
            {
                Undo();
            }
        }
    }
}