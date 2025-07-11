using System.Collections.Generic;
using Godot;

namespace NewArcana;

public class Settlement
{
    public Dictionary<Vector2I, House> Houses = new Dictionary<Vector2I, House>();
    private CityLayer _cityLayer;
    public Settlement(CityLayer cityLayer)
    {
        _cityLayer = cityLayer;
    }

    public void SpawnSettlement(Vector2I position)
    {
        for (int i = 0; i < 5; i++)
        {
            int x = GD.RandRange(-3, 3);
            int y = GD.RandRange(-3, 3);
            CreateHouse(position + new Vector2I(x, y));
        }
    }

    private void CreateHouse(Vector2I position)
    {
        if(Houses.ContainsKey(position))
            return;
        House house = new House();
        Houses.Add(position, house);
        _cityLayer.SetHouse(position);
    }
}