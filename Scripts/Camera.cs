using Godot;

namespace NewArcana.Scripts;

public partial class Camera : Camera2D
{
    private float _speed = 5;

    private Vector2 zoomIncrement = new(0.1f, 0.1f);

    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");

        Position += inputDirection * _speed;
    }

    public void AdjustSetZoom(Vector2 delta)
    {
        if(Zoom.X + delta.X > 0.2f || Zoom.X + delta.X < 2 )
            Zoom += delta;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseButton emb) return;
        if (!emb.IsPressed()) return;

        switch (emb.ButtonIndex)
        {
            case MouseButton.WheelUp:
                AdjustSetZoom(zoomIncrement);
                break;
            case MouseButton.WheelDown:
                AdjustSetZoom(-zoomIncrement);
                break;
        }
    }
}