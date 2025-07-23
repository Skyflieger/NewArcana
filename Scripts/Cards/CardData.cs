using Godot;

namespace NewArcana.Scripts.Cards;

[GlobalClass]
public partial class CardData : Resource
{
    [Export]
    public string CardName { get; set; }

    [Export] public CSharpScript Script;
}