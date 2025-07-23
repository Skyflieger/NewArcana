using Godot;
using NewArcana.Scripts.World;
using NewArcana.Scripts.World.Tiles;

namespace NewArcana.Scripts.Cards.CardScripts;

[GlobalClass]
public partial class EruptFear : CardScriptBase
{
    public override bool ExecuteCard(Tile tile)
    {
        switch (tile)
        {
            case City city:
                city.ChangeFear(10);
                return true;
                break;
            default:
                return false;
        }

    }
}