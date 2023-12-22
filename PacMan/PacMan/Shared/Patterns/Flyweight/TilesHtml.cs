using PacMan.Shared.Enums;

namespace PacMan.Shared.Patterns.Flyweight
{
    public class TilesHtml
    {
        public static Dictionary<EnumTileType, string> TilesHtmlStrings = new()
        {
            { EnumTileType.Empty, "<td class='grid-cell'></td>" },
            { EnumTileType.Wall, "<td class='grid-cell wall'></td>" },
            {
                EnumTileType.Pellet, "<td class='grid-cell'><center><div class='pellet'></center></div></td>"
            },
            {
                EnumTileType.MegaPellet,
                "<td class='grid-cell'><center><div class='megapellet'></center></div></td>"
            },
            {
                EnumTileType.ImobilePoison,
                "<td class='grid-cell'><center><div class='ImobilizePoison'></center></div></td>"
            },
            {
                EnumTileType.SlowPoison,
                "<td class='grid-cell'><center><div class='SlowPoison'></center></div></td>"
            },
            {
                EnumTileType.SlowPoisonAntidote,
                "<td class='grid-cell'><center><div class='SlowPoisonAntidote'></center></div></td>"
            },
            {
                EnumTileType.FoodPoison,
                "<td class='grid-cell'><center><div class='FoodPoison'></center></div></td>"
            },
            {
                EnumTileType.FoodPoisonAntidote,
                "<td class='grid-cell'><center><div class='FoodPoisonAntidote'></center></div></td>"
            },
            {
                EnumTileType.PointsPoison,
                "<td class='grid-cell'><center><div class='PointsPoison'></center></div></td>"
            },
            {
                EnumTileType.PointsPoisonAntidote,
                "<td class='grid-cell'><center><div class='PointsPoisonAntidote'></center></div></td>"
            },
            {
                EnumTileType.AllCureTile,
                "<td class='grid-cell'><center><div class='AllCure'></center></div></td>"
            }
        };
    }
}