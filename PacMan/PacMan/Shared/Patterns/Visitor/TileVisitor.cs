using PacMan.Shared.Factories;
using PacMan.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace PacMan.Shared.Patterns.Visitor
{
    public class TileVisitor : IVisitor
    {
        private int desiredX;
        private int desiredY;
        private PlayerStateModel player;
        private GameStateModel gameState;

        public TileVisitor(int desiredX, int desiredY, PlayerStateModel player, GameStateModel gameState)
        {
            this.player = player;
            this.desiredX = desiredX;
            this.desiredY = desiredY;
            this.gameState = gameState;
        }

        public void VisitAllCureTile()
        {
            gameState.Grid.ChangeTile(new EmptyTile(), desiredX, desiredY);
            player.SetState(new NormalPacmanState(player));
            player.EatPellet();
            MovePlayer();
        }

        public void VisitEmptyTile()
        {
            MovePlayer();
        }

        public void VisitMegaPelletTile()
        {
            if (Eat(gameState, desiredX, desiredY, player))
            {
                player.SetState(new SuperPacmanState(player, 12));
            }
            MovePlayer();
        }

        public void VisitPelletTile()
        {
            Eat(gameState, desiredX, desiredY, player);
            player.EatPellet();
            MovePlayer();
        }

        public void VisitPoisonTile(IPoison poison)
        {            
            gameState.Grid.ChangeTile(new EmptyTile(), desiredX, desiredY);
            player.AddPoison(poison);
            MovePlayer();
        }

        public void VisitWallTile()
        {
            // do fuck all
        }

        private bool Eat(GameStateModel session, int desiredX, int desiredY, PlayerStateModel player)
        {
            if (player.CanEat())
            {
                session.Grid.ChangeTile(new EmptyTile(), desiredX, desiredY);
                return true;
            }
            return false;
        }

        private void MovePlayer()
        {
            player.Coordinates = new(desiredX, desiredY);
        }
    }
}
