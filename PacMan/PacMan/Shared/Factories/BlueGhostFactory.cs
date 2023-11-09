using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class BlueGhostFactory : EnemyFactory
    {
        public override IEnemy CreateEnemy()
        {
            return new BlueGhost();
        }
    }
}