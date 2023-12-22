using System.Text.Json;
using PacMan.Shared.Models;
using System.Text.Json.Serialization;

namespace PacMan.Shared.Converters
{
    public class TileGridJsonConverter : JsonConverter<TileGrid>
    {
        public override TileGrid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;

            if (!root.TryGetProperty("Width", out var widthElement))
            {
                throw new JsonException("Width property not found.");
            }

            if (!root.TryGetProperty("Height", out var heightElement))
            {
                throw new JsonException("Height property not found.");
            }

            // Check the type property to determine which class to instantiate.
            if (!root.TryGetProperty("Tiles", out var typeElement))
            {
                throw new JsonException("Tiles property not found.");
            }

            return new TileGrid(widthElement.GetInt32(), heightElement.GetInt32(), new());
        }

        public override void Write(Utf8JsonWriter writer, TileGrid value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}