using Godot;

namespace NewArcana.Scripts;

public partial class Clock : Control
{

	private Control _handle;
	private RichTextLabel _label;

	[Export] private Control _playButton;
	[Export] private Control _stopButton;
	[Export] private TimeManager _timeManager;


	public override void _Ready()
	{
		_handle = GetNode<Control>("Handle");
		_label = GetNode<RichTextLabel>("Year");
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("game_time"))
		{
			if (_timeManager.Paused)
			{
				Resume();
			}
			else
			{
				Pause();	
			}
		}
		base._Process(delta);
	}

	public void Resume()
	{
		_timeManager.Resume();
		_playButton.Visible = false;
		_stopButton.Visible = true;
	}

	public void Pause()
	{
		_timeManager.Pause();
		_playButton.Visible = true;
		_stopButton.Visible = false;
	}

	public void OnMonthPassed(int month, int year)
	{
		_handle.RotationDegrees = (360 / 12) * month;
	}
	
	public void OnYearPassed(int year)
	{
		_label.Text = year.ToString();
	}
}
