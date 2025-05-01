using Godot;
using System;

public partial class StartScreen : Control
{
    [Export] public string Level1ScenePath = "res://Scenes/Main/Level1.tscn";

    public override void _Ready()
    {
        GetNode<TextureButton>("Fondo/Menu/Buttons/PlayButton").Pressed += OnPlayPressed;
        GetNode<TextureButton>("Fondo/Menu/Buttons/ExitButton").Pressed += OnExitPressed;
    }

    private void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile(Level1ScenePath);
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }
}
