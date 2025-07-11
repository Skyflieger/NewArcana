using Godot;
using System;
using System.Collections.Generic;
using NewArcana;

public partial class CityLayer : TileMapLayer
{
	public List<Settlement> Settlements { get; set; } = new List<Settlement>();
	public override void _Ready()
	{
		CreateSettlement();
		CreateSettlement();
		CreateSettlement();
	}

	public Settlement CreateSettlement()
	{
		Settlement settlement = new Settlement(this);

		Vector2I position = new Vector2I(GD.RandRange(-50, 50), GD.RandRange(-50, 50));
		
		settlement.SpawnSettlement(position);
		
		Settlements.Add(settlement);
		
		return settlement;
	}

	public void SetHouse(Vector2I position)
	{
		SetCell(position, 0, new Vector2I(0, 0));
	}
}
