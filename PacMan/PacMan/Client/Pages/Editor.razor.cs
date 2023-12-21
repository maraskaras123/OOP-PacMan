using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PacMan.Shared.Enums;
using PacMan.Shared.Factories;
using PacMan.Shared.Models;
using PacMan.Shared.Patterns.Memento;
using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PacMan.Client.Pages
{
    public partial class Editor
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        bool GridOptionsSelected = false;

        bool isPainting = false;

        TileGridBuilderOptions GridOptions = new TileGridBuilderOptions();
        TileGrid Grid = new TileGrid();

        private PelletTileFactory _pelletFactory = new PelletTileFactory();
        private WallTileFactory _wallsFactory = new WallTileFactory();
        private EmptyTileFactory _emptyFactory = new EmptyTileFactory();
        private MegaPelletTileFactory _megapelletFactory = new MegaPelletTileFactory();

        EnumTileType currentBrush = EnumTileType.Wall;

        Caretaker caretaker = new Caretaker(new TileGrid());

        string test = "";

        private void SetGridOptions()
        {
            TileGridBuilder builder = new TileGridBuilder();
            builder.WithHeight(GridOptions.Height)
                .WithWidth(GridOptions.Width)
                .WithRandomTiles(0);
            Grid = builder.Build();
            
            caretaker = new Caretaker(Grid);

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
                            Tile = _megapelletFactory.CreateTile();
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
            using (MemoryStream ms = new MemoryStream())
            {
                // Create Utf8JsonWriter using MemoryStream
                using (Utf8JsonWriter writer = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true }))
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
