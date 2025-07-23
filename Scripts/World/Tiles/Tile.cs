using Godot;

namespace NewArcana.Scripts.World.Tiles;

public abstract partial class Tile : Node
{
    public Vector2I Position { get; set; }
    public abstract BuildingLayerTileType  TileType { get; }

    public abstract void OnMonthPassed(int month, int year);

    public abstract void OnYearPassed(int month);
}