using Godot;
using System;

public partial class Clock : Control
{

	private Control _handle;
	private RichTextLabel _label;
	public override void _Ready()
	{
		_handle = GetNode<Control>("Handle");
		_label = GetNode<RichTextLabel>("Year");
		base._Ready();
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
