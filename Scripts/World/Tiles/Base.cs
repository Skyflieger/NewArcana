using Godot;

namespace NewArcana.Scripts.World.Tiles;

public partial class Base : Tile
{
	public override BuildingLayerTileType TileType { get; } =  BuildingLayerTileType.Base;
	
	public override void OnMonthPassed(int month, int year)
	{
	}

	public override void OnYearPassed(int month)
	{
	}
}
