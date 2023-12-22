using System.Text.Json;
using System.Text.Json.Serialization;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Shared.Converters
{
    public class TileJsonConverter : JsonConverter<Tile>
    {
        public override Tile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;

            // Check the type property to determine which class to instantiate.
            if (!root.TryGetProperty("Type", out var typeElement))
            {
                throw new JsonException("Type property not found.");
            }

            EnumTileType type;
            switch (typeElement.ValueKind)
            {
                case JsonValueKind.String:
                {
                    // If the type is a string, parse it as an enum
                    var typeStr = typeElement.GetString();
                    if (!Enum.TryParse(typeStr, out type))
                    {
                        throw new JsonException($"Unknown tile type: {typeStr}");
                    }

                    break;
                }
                case JsonValueKind.Number:
                    // If the type is a number, get it as an int and then cast to the enum
                    type = (EnumTileType)typeElement.GetInt32();
                    break;
                case JsonValueKind.Undefined:
                case JsonValueKind.Object:
                case JsonValueKind.Array:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                default:
                    throw new JsonException("Invalid type value.");
            }

            Tile tile = type switch
            {
                EnumTileType.Wall => new WallTile(),
                EnumTileType.Pellet => new PelletTile(),
                EnumTileType.MegaPellet => new MegaPelletTile(),
                EnumTileType.Empty => new EmptyTile(),
                _ => throw new NotSupportedException($"Unknown tile type: {type}"),
            };
            return tile;
        }


        public override void Write(Utf8JsonWriter writer, Tile value, JsonSerializerOptions options)
        {
            // Serialize the Tile object based on its runtime type.
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}