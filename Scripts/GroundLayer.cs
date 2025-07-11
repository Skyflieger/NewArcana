using Godot;
using System;

public partial class GroundLayer : TileMapLayer
{
	public override void _Ready()
	{
		for (int i = -50; i < 50; i++)
		{
			for (int j = -50; j < 50; j++)
			{
				SetCell(new Vector2I(i, j), 0, new Vector2I(0, 1));
			}
		}
		base._Ready();
	}
}
