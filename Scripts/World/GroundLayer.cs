using System;
using Godot;

namespace NewArcana.Scripts.World;

public partial class GroundLayer : LayerBase<GroundLayerTileType>
{

	protected override (int, Vector2I) GetTileInfo(GroundLayerTileType tile)
	{
		int sourceId = 0;
		Vector2I tileSetPosition;

		switch (tile)
		{
			case GroundLayerTileType.Grass:
				sourceId = 0;
				tileSetPosition = new Vector2I(GD.RandRange(0,2), 1);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(tile), tile, null);
		}

		return (sourceId, tileSetPosition);
	}
}
