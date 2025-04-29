using Godot;
using System;

public partial class PlatformGenerator : Node2D
{
    [Export] public PackedScene PlatformScene;
    [Export] public int MaxPlatforms = 20;
    [Export] public Vector2 SpawnRange = new Vector2(1000, 600);

    public override void _Ready()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        for (int i = 0; i < MaxPlatforms; i++)
        {
            var platform = PlatformScene.Instantiate() as StaticBody2D;
            platform.Position = new Vector2(
                rng.RandfRange(-SpawnRange.X, SpawnRange.X),
                rng.RandfRange(-SpawnRange.Y, SpawnRange.Y)
            );
            AddChild(platform);
        }
    }
}