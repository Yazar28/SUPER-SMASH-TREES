using Godot;
using System;
using System.Collections.Generic;

public partial class BackgroundManager : Node
{
    [Export] public Sprite2D BackgroundSprite;

    private List<string> backgroundPaths = new List<string>
    {
        "res://Assets/Backgrounds/Level1.png",
        "res://Assets/Backgrounds/Level2.png",
        "res://Assets/Backgrounds/Level3.png",
        "res://Assets/Backgrounds/Level4.png"
    };

    public override void _Ready()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        int index = rng.RandiRange(0, backgroundPaths.Count - 1);
        Texture2D selectedTexture = GD.Load<Texture2D>(backgroundPaths[index]);

        if (BackgroundSprite != null && selectedTexture != null)
        {
            BackgroundSprite.Texture = selectedTexture;

            BackgroundSprite.Scale = new Vector2(
                1280f / BackgroundSprite.Texture.GetWidth(),
                680f / BackgroundSprite.Texture.GetHeight()
            );
        }
    }
}
