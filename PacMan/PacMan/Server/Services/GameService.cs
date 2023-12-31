﻿using Microsoft.AspNetCore.SignalR;
using PacMan.Server.DbSchema;
using PacMan.Server.Hubs;
using PacMan.Shared.Enums;
using PacMan.Shared.Factories;
using PacMan.Shared.Models;
using PacMan.Shared.Patterns.Visitor;
using System.Drawing;
using System.Text.Json;

namespace PacMan.Server.Services
{
    public interface IGameService
    {
        void Reset(GameStateModel session);
        void Start(GameStateModel session, TileGridBuilderOptions gridOptions, int endPoints);
        Task Init(string sessionId, GameStateModel session);
        Task PlayerUpdate(string connectionId, int index, string name);
    }

    public class GameService : IGameService
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;
        private readonly ApplicationDbContext _dbContext;

        private readonly EnemyFactory _redGhostFactory;
        private readonly EnemyFactory _blueGhostFactory;
        private readonly TileFactory _emptyTileFactory;
        private readonly TileFactory _megaPelletTileFactory;
        private readonly TileFactory _immobilizePoisonFactory;
        private readonly TileFactory _slowPoisonFactory;
        private readonly TileFactory _slowPoisonAntidoteFactory;
        private readonly TileFactory _foodPoisonFactory;
        private readonly TileFactory _foodPoisonAntidoteFactory;

        private readonly TileFactory _pointsPoisonFactory;
        private readonly TileFactory _pointsPoisonAntidoteFactory;
        private readonly TileFactory _allCureTileFactory;


        private int _endPoints;

        public GameService(IHubContext<GameHub, IGameHubClient> hubContext, ApplicationDbContext dbContext)
        {
            _redGhostFactory = new RedGhostFactory();
            _blueGhostFactory = new BlueGhostFactory();
            _emptyTileFactory = new EmptyTileFactory();
            _megaPelletTileFactory = new MegaPelletTileFactory();
            _immobilizePoisonFactory = new ImmobilePoisonTileFactory();
            _slowPoisonFactory = new SlowPoisonTileFactory();
            _slowPoisonAntidoteFactory = new SlowPoisonAntidoteFactory();
            _foodPoisonFactory = new FoodPoisonFactory();
            _foodPoisonAntidoteFactory = new FoodPoisonAntidoteFactory();
            _pointsPoisonFactory = new PointsPoisonTileFactory();
            _pointsPoisonAntidoteFactory = new PointPoisonAntidoteFactory();
            _allCureTileFactory = new AllCureTileFactory();
            _hubContext = hubContext;
            _dbContext = dbContext;
            _endPoints = 100;
        }

        public void Reset(GameStateModel session)
        {
            session.GameState = EnumGameState.Initializing;
            session.Enemies = new();
            session.Grid = new();
            session.Ticks = 0;
            session.State = new();
        }

        public void Start(GameStateModel session, TileGridBuilderOptions gridOptions, int points)
        {
            CreateEnemies(session);
            session.GameState = EnumGameState.Starting;
            var gridJson = gridOptions.SelectedGridId is not null
                ? _dbContext.Grids.Find(gridOptions.SelectedGridId)?.GridJson
                : null;
            var grid = gridJson is not null ? JsonSerializer.Deserialize<TileGrid>(gridJson) : new TileGridBuilder()
                .WithWidth(gridOptions.Width)
                .WithHeight(gridOptions.Height)
                .WithRandomTiles(gridOptions.RandomTileCount)
                .Build();
            session.Grid = grid;
            _endPoints = points;
        }

        private void CreateEnemies(GameStateModel session)
        {
            var redGhost = _redGhostFactory.CreateEnemy();
            var blueGhost = _blueGhostFactory.CreateEnemy();
            redGhost.Position = new(10, 10);
            blueGhost.Position = new(9, 9);
            session.Enemies.Add(redGhost);
            session.Enemies.Add(blueGhost);
        }

        public async Task Init(string sessionId, GameStateModel session)
        {
            session.GameState = EnumGameState.Running;
            session.Ticks = 0;
            var rnd = new Random();
            foreach (var connection in session.Connections)
            {
                var stateModel = new PlayerStateModel
                    { Name = connection.Key, Direction = (EnumDirection)rnd.Next(0, 4) };
                var coordinates = new Point(rnd.Next(session.Grid.Width), rnd.Next(session.Grid.Height));
                while (session.Grid.Tiles[$"{coordinates.X}_{coordinates.Y}"].Type == EnumTileType.Wall)
                {
                    coordinates = new(rnd.Next(session.Grid.Width), rnd.Next(session.Grid.Height));
                }

                stateModel.Coordinates = coordinates;
                session.State.Add(connection.Key, stateModel);

                await _hubContext.Clients.Group(sessionId).ReceiveGrid(session.Grid.ConvertForSending());
            }

            await _hubContext.Clients.Group(sessionId).StateChange(session.GameState);

            while (session.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(500), Tick(sessionId, session));
                await _hubContext.Clients.Group(sessionId).ReceiveGrid(session.Grid.ConvertForSending());
                await _hubContext.Clients.Group(sessionId).Tick(new(session.GameState,
                    session.State.Select((x, index) =>
                            $"{index},{x.Value.Coordinates.X},{x.Value.Coordinates.Y},{(int)x.Value.Direction}")
                        .ToList(),
                    session.State.Select(x => x.Value.Points).ToList()));
            }
        }

        public async Task PlayerUpdate(string connectionId, int index, string name)
        {
            await _hubContext.Clients.Client(connectionId).PlayerUpdate(index, name);
        }

        private void TouchingEnemy(GameStateModel session, PlayerStateModel state)
        {
            foreach (var enemy in session.Enemies.Where(enemy => state.Coordinates == enemy.Position))
            {
                state.CollideWithGhost();
                enemy.Respawn(session);
            }
        }

        // private bool IsOcupiedByPacman(GameStateModel session, int desiredX, int desiredY)
        // {
        //     Point desiredPoint = new Point(desiredX, desiredY);
        //     foreach (var pacman in session.State)
        //     {
        //         if (pacman.Value.Coordinates == desiredPoint)
        //         {
        //             return true;
        //         }
        //     }
        //     return false;
        // }

        private void HandlePoison(IPoison poison, PlayerStateModel player)
        {
            if (player.GetState() != typeof(PoisonedPacmanState))
            {
                player.SetState(new PoisonedPacmanState(player));
            }

            player.AddPoison(poison);
        }

        // Game Logic
        private async Task Tick(string sessionId, GameStateModel session)
        {
            await _hubContext.Clients.Group(sessionId).ReceiveGrid(session.Grid.ConvertForSending());
            foreach (var enemy in session.Enemies)
            {
                enemy.Move(session);
            }

            foreach (var state in session.State)
            {
                TouchingEnemy(session, state.Value);
                var currentX = state.Value.Coordinates.X;
                var currentY = state.Value.Coordinates.Y;
                var desiredX = currentX;
                var desiredY = currentY;

                switch (state.Value.Direction)
                {
                    case EnumDirection.Up:
                        desiredY--;
                        break;
                    case EnumDirection.Down:
                        desiredY++;
                        break;
                    case EnumDirection.Left:
                        desiredX--;
                        break;
                    case EnumDirection.Right:
                        desiredX++;
                        break;
                }

                if (state.Value.CanMove())
                {
                    var visitor = new TileVisitor(desiredX, desiredY, state.Value, session);

                    var desiredTile = session.Grid.GetTile(desiredX, desiredY);

                    desiredTile.AcceptVisitor(visitor);
                }

                TouchingEnemy(session, state.Value);

                if (state.Value.Points >= _endPoints)
                {
                    session.GameState = EnumGameState.Finished;
                }

                state.Value.Tick();
            }

            var enemyData = session.Enemies
                .Select(e => new EnemyModel
                    { Position = new() { X = e.Position.X, Y = e.Position.Y }, Character = e.Character })
                .ToList();
            await _hubContext.Clients.Group(sessionId).ReceiveEnemies(enemyData);
            SpawnPowerUp(session, _pointsPoisonFactory, 40);
            SpawnPowerUp(session, _foodPoisonFactory, 40);
            SpawnPowerUp(session, _slowPoisonFactory, 40);
            SpawnPowerUp(session, _immobilizePoisonFactory, 40);
            SpawnPowerUp(session, _allCureTileFactory, 20);
            session.Ticks += 1;
        }


        private void SpawnPowerUp(GameStateModel session, TileFactory tileFactory, int ticks)
        {
            if (session.Ticks % ticks == 0)
            {
                var rnd = new Random();
                var placed = false;
                while (!placed)
                {
                    var x = rnd.Next(session.Grid.Width);
                    var y = rnd.Next(session.Grid.Height);
                    if (session.Grid.GetTile(x, y).Type != EnumTileType.Wall)
                    {
                        session.Grid.ChangeTile(tileFactory.CreateTile(), x, y);
                        placed = true;
                    }
                }
            }
        }
    }
}