using System.Collections.Generic;
using Godot;

namespace NewArcana.Scripts.Cards;

public partial class CardDeck : Control
{
	private readonly Queue<CardData> Cards = new Queue<CardData>();
	[Export]private Label _amountLabel;

	public override void _Ready()
	{
		InitializeCards();
	}
	
	public void InitializeCards()
	{
		for (int i = 0; i < 10; i++)
		{
			Cards.Enqueue(ResourceLoader.Load<CardData>("res://Cards/EruptFear.tres"));
		}
		UpdateLabel();
	}
	
	private void UpdateLabel()
	{
		_amountLabel.Text = Cards.Count.ToString();
	}

	public CardData DrawCard()
	{
		var x = Cards.Dequeue();

		UpdateLabel();

		return x;
	}
}
