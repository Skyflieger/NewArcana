using Godot;

namespace NewArcana.Scripts.World.Tiles;

public partial class City : Tile
{
    public int CitySize { get; set; } = 1;
    public int Fear { get; private set; } = 0;
    public int Trust { get; private set; } = 0;
    public int Population { get; private set; } = 10;
    public int Food { get; private set; } = 10;

    public override BuildingLayerTileType TileType =>
        CitySize switch
        {
            1 => BuildingLayerTileType.City1,
            2 => BuildingLayerTileType.City2,
            3 => BuildingLayerTileType.City3,
            _ => BuildingLayerTileType.City1  // Default to City1 for invalid or 0 values
        };

    public override void OnMonthPassed(int month, int year)
    {
    }

    public override void OnYearPassed(int month)
    {
    }

    public void ChangeFear(int amount)
    {
        Fear += amount;
    }
}