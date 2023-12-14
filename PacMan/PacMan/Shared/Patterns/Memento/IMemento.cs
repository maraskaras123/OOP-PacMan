using PacMan.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Shared.Patterns.Memento
{
    public interface IMemento
    {
        Dictionary<string, Tile> GetState();
        DateTime GetDate();
    }
}
