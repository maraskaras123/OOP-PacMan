using PacMan.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Shared.Patterns.Visitor
{
    public interface IVisitor
    {
        void VisitPoisonTile(IPoison poison);
        void VisitWallTile();
        void VisitEmptyTile();
        void VisitMegaPelletTile();
        void VisitAllCureTile();
        void VisitPelletTile();
    }
}
