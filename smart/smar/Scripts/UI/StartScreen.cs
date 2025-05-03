using Godot;
using System;

public partial class StartScreen : Control
{
    public override void _Ready()
    {
        var botonJugar = GetNode<TextureButton>("Fondo/Menu/Buttons/PlayButton");
        var botonSalir = GetNode<TextureButton>("Fondo/Menu/Buttons/ExitButton");

        botonJugar.Pressed += OnPlayPressed;
        botonSalir.Pressed += OnExitPressed;
    }

    private void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Main/Level1.tscn");
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }
}
