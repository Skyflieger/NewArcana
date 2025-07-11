using System.Collections.Generic;
using Godot;

namespace NewArcana;

public class Sanctuary
{
    public Dictionary<Vector2I, Tree> Trees = new Dictionary<Vector2I, Tree>();
    private TopLayer _topLayer;
    public Sanctuary(TopLayer topLayer)
    {
        _topLayer = topLayer;
    }

    public void SpawnSanctuary(Vector2I position)
    {
        for (int i = 0; i < 5; i++)
        {
            int x = GD.RandRange(-3, 3);
            int y = GD.RandRange(-3, 3);
            CreateTree(position + new Vector2I(x, y));
        }
    }

    private void CreateTree(Vector2I position)
    {
        if(Trees.ContainsKey(position))
            return;
        Tree tree = new Tree();
        Trees.Add(position, tree);
        _topLayer.SetCell(position, TopLayerTiles.Tree);
    }
}