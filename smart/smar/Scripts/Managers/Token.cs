using Godot;

public partial class Token : Area2D
{
    [Export] public int Value;

    public override void _Ready()
    {
        // Animación de caída
        var tween = CreateTween();
        tween.TweenProperty(this, "position", Position + new Vector2(0, 1000), 3.0f);
        tween.TweenCallback(Callable.From(QueueFree));

        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            GameManager.Instance.PlayerCollectToken(player, Value);
            QueueFree();
        }
    }
}