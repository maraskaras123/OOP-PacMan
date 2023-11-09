using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

public class TileJsonConverter : JsonConverter<Tile>
{
    public override Tile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = document.RootElement;

            // Check the type property to determine which class to instantiate.
            if (!root.TryGetProperty("Type", out var typeElement))
            {
                throw new JsonException("Type property not found.");
            }

            EnumTileType type;
            if (typeElement.ValueKind == JsonValueKind.String)
            {
                // If the type is a string, parse it as an enum
                var typeStr = typeElement.GetString();
                if (!Enum.TryParse(typeStr, out type))
                {
                    throw new JsonException($"Unknown tile type: {typeStr}");
                }
            }
            else if (typeElement.ValueKind == JsonValueKind.Number)
            {
                // If the type is a number, get it as an int and then cast to the enum
                type = (EnumTileType)typeElement.GetInt32();
            }
            else
            {
                throw new JsonException("Invalid type value.");
            }

            Tile tile;
            switch (type)
            {
                case EnumTileType.Wall:
                    tile = new WallTile();
                    break;
                case EnumTileType.Pellet:
                    tile = new PelletTile();
                    break;
                case EnumTileType.MegaPellet:
                    tile = new MegaPelletTile();
                    break;
                case EnumTileType.Empty:
                    tile = new EmptyTile();
                    break;
                default:
                    throw new NotSupportedException($"Unknown tile type: {type}");
            }

            return tile;
        }
    }


    public override void Write(Utf8JsonWriter writer, Tile value, JsonSerializerOptions options)
    {
        // Serialize the Tile object based on its runtime type.
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}