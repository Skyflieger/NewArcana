using Godot;
using System;

public partial class Camera : Camera2D
{
	private float _speed = 5;
	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");

		Position += inputDirection * _speed;
	}

	public void SetZoom(Vector2 delta)
	{
		Zoom += delta;
	}

	private Vector2 zoomIncrement = new Vector2(0.1f, 0.1f);
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is not InputEventMouseButton emb) return;
		if (!emb.IsPressed()) return;
		
		switch (emb.ButtonIndex)
		{
			case MouseButton.WheelUp:
				SetZoom(zoomIncrement);
				break;
			case MouseButton.WheelDown:
				SetZoom(-zoomIncrement);
				break;
		}
	}
}
