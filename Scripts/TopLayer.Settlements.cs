using System.Collections.Generic;
using System.Linq;
using Godot;

namespace NewArcana.Scripts;

public partial class TopLayer
{
    public List<Settlement> Settlements { get; set; } = new();

    public Settlement CreateSettlement()
    {
        Settlement settlement = new(this);
        Vector2I position;

        position = new Vector2I(GD.RandRange(-30, 30), GD.RandRange(-30, 30));
        
        settlement.SpawnSettlement(position);
        Settlements.Add(settlement);

        return settlement;
    }
    
}