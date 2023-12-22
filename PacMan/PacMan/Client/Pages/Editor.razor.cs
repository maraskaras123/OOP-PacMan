using Microsoft.AspNetCore.Components;
using PacMan.Shared.Enums;
using PacMan.Shared.Factories;
using PacMan.Shared.Models;
using PacMan.Shared.Patterns.Memento;
using PacMan.Shared.Patterns.Proxy;
using System.Text;
using System.Text.Json;

namespace PacMan.Client.Pages
{
    public partial class Editor
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        private bool GridOptionsSelected = false;

        private bool isPainting = false;

        private TileGridBuilderOptions GridOptions = new();
        private TileGrid Grid = new();

        private readonly TileFactoryProxy<PelletTileFactory> _pelletFactory = new();
        private readonly TileFactoryProxy<WallTileFactory> _wallsFactory = new();
        private readonly TileFactoryProxy<EmptyTileFactory> _emptyFactory = new();
        private readonly TileFactoryProxy<MegaPelletTileFactory> _megaPelletFactory = new();

        private EnumTileType currentBrush = EnumTileType.Wall;

        private Caretaker caretaker = new(new());

        private string test = "";

        private void SetGridOptions()
        {
            var builder = new TileGridBuilder();
            builder.WithHeight(GridOptions.Height)
                .WithWidth(GridOptions.Width)
                .WithRandomTiles(0);
            Grid = builder.Build();

            caretaker = new(Grid);

            GridOptionsSelected = true;
        }

        private void SetBrush(EnumTileType brush)
        {
            currentBrush = brush;
        }

        private void HandlePainting(int row, int col)
        {
            if (isPainting)
            {
                if (row > 0 && col > 0 && row < Grid.Width - 1 && col < Grid.Height - 1)
                {
                    Tile Tile;

                    switch (currentBrush)
                    {
                        case EnumTileType.Wall:
                            Tile = _wallsFactory.CreateTile();
                            break;
                        case EnumTileType.Pellet:
                            Tile = _pelletFactory.CreateTile();
                            break;
                        case EnumTileType.MegaPellet:
                            Tile = _megaPelletFactory.CreateTile();
                            break;
                        default:
                            Tile = _emptyFactory.CreateTile();
                            break;
                    }

                    Grid.ChangeTile(Tile, row, col);
                    StateHasChanged();
                }
            }
        }

        private bool StartPainting()
        {
            caretaker.Backup();
            isPainting = true;
            return false;
        }

        private void StopPainting()
        {
            isPainting = false;
        }

        private void Undo()
        {
            caretaker.Undo();
            StateHasChanged();
        }

        private void SetEmptyToPellet()
        {
            foreach (Tile tile in Grid)
            {
                tile.Type = EnumTileType.Pellet;
            }

            StateHasChanged();
        }

        private string ConvertToJson()
        {
            // Create a MemoryStream to write JSON data
            using (var ms = new MemoryStream())
            {
                // Create Utf8JsonWriter using MemoryStream
                using (var writer = new Utf8JsonWriter(ms, new() { Indented = true }))
                {
                    // Start writing the JSON object
                    writer.WriteStartObject();

                    // Write Width and Height properties
                    writer.WriteNumber("Width", Grid.Width);
                    writer.WriteNumber("Height", Grid.Height);

                    // Write Tiles dictionary
                    writer.WritePropertyName("Tiles");
                    writer.WriteStartObject();

                    foreach (var tile in Grid.Tiles)
                    {
                        writer.WritePropertyName(tile.Key);
                        writer.WriteStartObject();
                        writer.WriteNumber("Type", (int)tile.Value.Type);
                        writer.WriteEndObject();
                    }

                    // End Tiles object
                    writer.WriteEndObject();

                    // End the main object
                    writer.WriteEndObject();
                }

                test = Encoding.UTF8.GetString(ms.ToArray());
                // Convert the MemoryStream to a string
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private async Task UploadGrid()
        {
            // sorry boss nezinau whata doin
            //
        }
    }
}