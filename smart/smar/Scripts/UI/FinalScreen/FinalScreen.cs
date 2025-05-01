using Godot;
using System;
using System.Collections.Generic;

public partial class FinalScreen : Control
{
    public class JugadorInfo
    {
        public string Nombre { get; set; }
        public int Puntaje { get; set; }

        public JugadorInfo(string nombre, int puntaje)
        {
            Nombre = nombre;
            Puntaje = puntaje;
        }
    }

    public List<JugadorInfo> Puntajes { get; set; } = new();

    public override void _Ready()
    {
        Puntajes.Add(new JugadorInfo("Jugador A", 250));
        Puntajes.Add(new JugadorInfo("Jugador B", 120));
        Puntajes.Add(new JugadorInfo("Jugador C", 180));
        Puntajes.Add(new JugadorInfo("Jugador D", 90));

        Puntajes.Sort((a, b) => b.Puntaje.CompareTo(a.Puntaje));

        for (int i = 0; i < Puntajes.Count && i < 4; i++)
        {
            var nombreLabel = GetNode<Label>($"Contenido/Estadísticas/Fila{i + 1}/Nombre{i + 1}");
            var puntosLabel = GetNode<Label>($"Contenido/Estadísticas/Fila{i + 1}/Puntos{i + 1}");

            nombreLabel.Text = Puntajes[i].Nombre;
            puntosLabel.Text = Puntajes[i].Puntaje.ToString();
        }

        var botonSalir = GetNode<TextureButton>("Boton");
        botonSalir.Pressed += () => GetTree().Quit();
    }
}
