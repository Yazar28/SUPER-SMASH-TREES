using Godot;
using System;

public partial class Token : Area2D
{
    [Export] public int MinValue = 1;
    [Export] public int MaxValue = 99;
    public int Value;

    private float _fallSpeed = 100f;

    public override void _Ready()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        Value = rng.RandiRange(MinValue, MaxValue);

        var label = GetNode<Label>("ValueLabel");
        label.Text = Value.ToString();
        label.AddThemeColorOverride("font_color", new Color(0.75f, 0.6f, 0.1f));

        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, _fallSpeed * (float)delta);

        if (GlobalPosition.Y > 800)  
        {
            QueueFree();
        }
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
