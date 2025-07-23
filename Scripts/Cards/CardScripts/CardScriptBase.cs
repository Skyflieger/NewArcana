using Godot;

namespace NewArcana.Scripts.Cards.CardScripts;

[GlobalClass]
public abstract partial class CardScriptBase : Resource
{
    public abstract bool ExecuteCard(World.Tiles.Tile tile);
}