using System;
using Godot;

namespace NewArcana.Scripts;

public partial class TopLayer : TileMapLayer
{
    private const float MinDistance = 20.0f;
    
    public override void _Ready()
    {
        CreateSettlement();
        CreateSanctuary();
        CreateSettlement();
        CreateSanctuary();
        CreateSettlement();
        CreateSanctuary();
    }
    
    public void SetCell(Vector2I position, TopLayerTiles tile)
    {
        (int, Vector2I) info = GetTileInfo(tile);

        SetCell(position, info.Item1, info.Item2);
    }

    private (int, Vector2I) GetTileInfo(TopLayerTiles tile)
    {
        int sourceId = 0;
        Vector2I tileSetPosition;

        switch (tile)
        {
            case TopLayerTiles.House:
                sourceId = 0;
                tileSetPosition = new Vector2I(0, 0);
                break;
            case TopLayerTiles.Street:
                sourceId = 0;
                tileSetPosition = new Vector2I(0, 1);
                break;
            case TopLayerTiles.Tree:
                sourceId = 1;
                tileSetPosition = new Vector2I(GD.RandRange(0, 2), 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tile), tile, null);
        }
        
        return (sourceId, tileSetPosition);
    }
    public bool IsCellOfType(Vector2I position, TopLayerTiles tileType)
    {
        (int id, Vector2I tileSetPosition) = GetTileInfo(tileType);
        return GetCellSourceId(position) == id && GetCellAtlasCoords(position) == tileSetPosition;
    }

    private int ManhattanDistance(Vector2I a, Vector2I b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }
    public TopLayerTiles? GetCellTileType(Vector2I position)
    {
        int cellSourceId = GetCellSourceId(position);
        Vector2I cellTileSetPosition = GetCellAtlasCoords(position);

        foreach (TopLayerTiles tile in Enum.GetValues(typeof(TopLayerTiles)))
        {
            (int id, Vector2I tileSetPosition) = GetTileInfo(tile);

            if (cellSourceId == id && cellTileSetPosition == tileSetPosition)
            {
                return tile;
            }
        }
        
        return null;
    }

    public void OnMonthPassed(int month, int year)
    {
        foreach (Settlement settlement in Settlements)
        {
            settlement.EvolveStreet(Vector2I.Zero);
        }
    }
    
    public void OnYearPassed(int year)
    {
        foreach (Settlement settlement in Settlements)
        {
            settlement.EvolveHouse();
        }
    }
}