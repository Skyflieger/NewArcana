namespace NewArcana.Scripts.World.Tiles;

public partial class Farm : Tile
{
    public override BuildingLayerTileType TileType { get; } = BuildingLayerTileType.Farm;
    public override void OnMonthPassed(int month, int year)
    {
    }

    public override void OnYearPassed(int month)
    {
    }
}