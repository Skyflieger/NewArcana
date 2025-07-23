using Godot;
using NewArcana.Scripts.World;

namespace NewArcana.Scripts;

public partial class GameManager : Node
{
	[Export] private TimeManager _timeManager;
	[Export] private Map _map;

	public override void _Ready()
	{
		Tween tween = CreateTween();
		tween.TweenInterval(0.5f);
		tween.TweenCallback(Callable.From(StartGame));
	}

	private void StartGame()
	{
		_map.InitializeMap();
	}
}
