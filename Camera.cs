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
		var mouse_pos = GetGlobalMousePosition();
		Zoom += delta;
		var new_mouse_pos = GetGlobalMousePosition();
		Position += mouse_pos - new_mouse_pos;
	}
	
	public override void _UnhandledInput(InputEvent @event){
		if (@event is InputEventMouseButton){
			InputEventMouseButton emb = (InputEventMouseButton)@event;
			if (emb.IsPressed()){
				if (emb.ButtonIndex == MouseButton.WheelUp){
					GD.Print(emb.AsText());
				}
				if (emb.ButtonIndex == MouseButton.WheelDown){
					GD.Print(emb.AsText());
				}
			}
		}
	}
}
