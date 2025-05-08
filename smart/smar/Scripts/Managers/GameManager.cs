using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance;

    [Export] public PackedScene TokenScene;
    [Export] public float TokenSpawnInterval = 2.0f;

    private Timer _tokenTimer;

    public override void _Ready()
    {
        Instance = this;

        _tokenTimer = new Timer();
        AddChild(_tokenTimer);
        _tokenTimer.WaitTime = TokenSpawnInterval;
        _tokenTimer.Timeout += SpawnToken;
        _tokenTimer.Start();
    }

    private void SpawnToken()
    {
        if (TokenScene == null)
            return;

        var tokenInstance = TokenScene.Instantiate();

        if (tokenInstance is Area2D token)
        {
            var rng = new RandomNumberGenerator();
            rng.Randomize();

            token.Position = new Vector2(
                rng.RandfRange(100, 1180),
                rng.RandfRange(-300, -100)
            );

            GetParent().GetNode<Node2D>("Tokens").AddChild(token);
        }
    }

    public void PlayerCollectToken(Player player, int value)
    {
        player.AddScore(value);
    }
}
