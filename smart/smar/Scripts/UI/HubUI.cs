using Godot;
using System;
using BinaryTree; // Asegurate de tener acceso a la clase `Reto`

public partial class HubUI : CanvasLayer
{
    private Label _timerLabel;
    private Label _scoreJ1;
    private Label _scoreJ2;
    private Label _scoreJ3;
    private Label _tipoArbolLabel;
    private Label _descripcionRetoLabel;
    private Label _roundsLabel;

    private float _tiempoRestante = 120f; // 2 minutos
    private bool _partidaFinalizada = false;

    public override void _Ready()
    {
        _timerLabel = GetNode<Label>("TimerLabel");
        _scoreJ1 = GetNode<Label>("ScoreJugador1");
        _scoreJ2 = GetNode<Label>("ScoreJugador2");
        _scoreJ3 = GetNode<Label>("ScoreJugador3");
        _tipoArbolLabel = GetNode<Label>("TypeTree");
        _descripcionRetoLabel = GetNode<Label>("DescriptionTree");
        _roundsLabel = GetNode<Label>("RoundsTree");

        ActualizarTimerLabel();
    }

    public override void _Process(double delta)
    {
        if (_partidaFinalizada)
            return;

        _tiempoRestante -= (float)delta;

        if (_tiempoRestante <= 0)
        {
            _tiempoRestante = 0;
            _partidaFinalizada = true;
            CambiarAEscenaFinal();
        }

        ActualizarTimerLabel();
    }

    private void ActualizarTimerLabel()
    {
        int minutos = (int)(_tiempoRestante / 60);
        int segundos = (int)(_tiempoRestante % 60);
        _timerLabel.Text = $"{minutos:D2}:{segundos:D2}";
    }

    public void ActualizarPuntaje(int jugador, int puntos)
    {
        switch (jugador)
        {
            case 1:
                _scoreJ1.Text = $"{puntos} PTS";
                break;
            case 2:
                _scoreJ2.Text = $"{puntos} PTS";
                break;
            case 3:
                _scoreJ3.Text = $"{puntos} PTS";
                break;
        }
    }

    public void MostrarReto(Reto reto)
    {
        GD.Print($"👀 Mostrando en HUD: {_tipoArbolLabel.Name} = {reto.Tipo}, {_descripcionRetoLabel.Name} = {reto.Descripcion}");
        _tipoArbolLabel.Text = reto.Tipo.ToString().ToUpper();
        _descripcionRetoLabel.Text = reto.Descripcion;
    }


    private void CambiarAEscenaFinal()
    {

        var gd = GetNode<GlobalData>("/root/GlobalData");
        gd.ResultadosFinales.Clear();

        foreach (Player jugador in GetTree().GetNodesInGroup("Players"))
        {
            gd.ResultadosFinales.Add(($"Jugador {jugador.PlayerNumber}", jugador.Score));
        }

        GetTree().ChangeSceneToFile("res://Scenes/UI/FinalScreen/FinalScreen.tscn");
    }

    public void ActualizarRonda(int numero)
    {
        _roundsLabel.Text = numero.ToString();
    }

}
