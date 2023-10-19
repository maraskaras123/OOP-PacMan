using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class RedGhostFactory : EnemyFactory
    {
        public override IEnemy CreateEnemy()
        {
            return new RedGhost();
        }
    }
}