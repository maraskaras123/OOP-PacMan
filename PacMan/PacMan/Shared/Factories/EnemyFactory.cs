using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public abstract class EnemyFactory
    {
        public abstract IEnemy CreateEnemy();
    }
}