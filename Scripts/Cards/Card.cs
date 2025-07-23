using Godot;

namespace NewArcana.Scripts.Cards;

public partial class Card : Control
{
	public CardData CardData { get; set; }
	[Export] private Label _cardName;

	public override void _Ready()
	{
		_cardName.Text = CardData.CardName;
		base._Ready();
	}
	
	[Signal]
	public delegate void CardClickedEventHandler(Card card);
	
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				EmitSignal(SignalName.CardClicked, this);
			}
		}
	}
	
	public void SetHighlighted(bool highlighted)
	{
		if (highlighted)
		{
			Modulate = Colors.Red;
		}
		else
		{
			Modulate = Colors.White;
		}
	}
}
