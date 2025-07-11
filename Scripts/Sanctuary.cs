using System.Collections.Generic;
using Godot;

namespace NewArcana.Scripts;

public class Sanctuary
{
    private readonly TopLayer _topLayer;
    public Dictionary<Vector2I, Tree> Trees = new();

    public Sanctuary(TopLayer topLayer)
    {
        _topLayer = topLayer;
    }

    public void SpawnSanctuary(Vector2I position)
    {
        for (int i = 0; i < 25; i++)
        {
            int x = GD.RandRange(-7, 7);
            int y = GD.RandRange(-7, 7);
            CreateTree(position + new Vector2I(x, y));
        }
    }

    private void CreateTree(Vector2I position)
    {
        if (Trees.ContainsKey(position))
            return;
        Tree tree = new();
        Trees.Add(position, tree);
        _topLayer.SetCell(position, TopLayerTiles.Tree);
    }
}