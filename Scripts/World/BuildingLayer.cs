using System;
using Godot;

namespace NewArcana.Scripts.World;

public partial class BuildingLayer : LayerBase<BuildingLayerTileType>
{

	protected override (int, Vector2I) GetTileInfo(BuildingLayerTileType tile)
	{
		int sourceId = 0;
		Vector2I tileSetPosition;

		switch (tile)
		{
			case BuildingLayerTileType.City1:
				sourceId = 0;
				tileSetPosition = new Vector2I(0, 0);
				break;
			case BuildingLayerTileType.City2:
				sourceId = 0;
				tileSetPosition = new Vector2I(1, 0);
				break;
			case BuildingLayerTileType.City3:
				sourceId = 0;
				tileSetPosition = new Vector2I(2, 0);
				break;
			case BuildingLayerTileType.City4:
				sourceId = 0;
				tileSetPosition = new Vector2I(3, 0);
				break;
			case BuildingLayerTileType.Forest:
				sourceId = 1;
				tileSetPosition = new Vector2I(GD.RandRange(0, 2), 0);
				break;
			case BuildingLayerTileType.Base:
				sourceId = 0;
				tileSetPosition = new Vector2I(0, 1);
				break;
			case BuildingLayerTileType.Farm:
				sourceId = 0;
				tileSetPosition = new Vector2I(0, 2);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(tile), tile, null);
		}

		return (sourceId, tileSetPosition);
	}
}
