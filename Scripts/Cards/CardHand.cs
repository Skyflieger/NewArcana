using Godot;
using NewArcana.Scripts.Cards.CardScripts;

namespace NewArcana.Scripts.Cards;

public partial class CardHand : Control
{
	[Export]private PackedScene _cardScene;
	private float _spread = 100;
	[Export] private CardDeck _cardDeck;
	public Card SelectedCard;

	public override void _Ready()
	{
		for (int i = 0; i < 5; i++)
		{
			DrawCard();
		}
	}

	public void DrawCard()
	{
		Card card = _cardScene.Instantiate<Card>();
		card.CardData = _cardDeck.DrawCard();
		card.CardClicked += OnCardClicked;
		AddChild(card);
	}

	private void OnCardClicked(Card clickedCard)
	{
		if (SelectedCard != null)
		{
			SelectedCard.SetHighlighted(false);
			
			if (SelectedCard == clickedCard)
			{
				SelectedCard = null;
				return;
			}
		}
		
		SelectedCard = clickedCard;
		SelectedCard.SetHighlighted(true);
	}

	public void OnYearPassed(int year)
	{
		DrawCard();
	}

	public void ExecuteCard(World.Tiles.Tile tile)
	{
		if(SelectedCard == null)
			return;

		if (((CardScriptBase)SelectedCard.CardData.Script.New()).ExecuteCard(tile))
		{
			RemoveChild(SelectedCard);
			SelectedCard = null;
		}
	}
}
