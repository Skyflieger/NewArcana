using System.Collections.Generic;
using System.Linq;
using Godot;

namespace NewArcana.Scripts;

public partial class TopLayer
{
    public List<Sanctuary> Sanctuaries { get; set; } = new();

    public Sanctuary CreateSanctuary()
    {
        Sanctuary sanctuary = new(this);
        Vector2I position;
        
        position = new Vector2I(GD.RandRange(-30, 30), GD.RandRange(-30, 30));


        sanctuary.SpawnSanctuary(position);
        Sanctuaries.Add(sanctuary);

        return sanctuary;
    }

}