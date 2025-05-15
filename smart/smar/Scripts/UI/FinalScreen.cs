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
        var resultados = GetNode<GlobalData>("/root/GlobalData").ResultadosFinales;

        var listaOrdenada = new List<(string nombre, int puntaje)>();
        for (int i = 0; i < resultados.Contar(); i++)
            listaOrdenada.Add(resultados.Obtener(i));

        listaOrdenada.Sort((a, b) => b.puntaje.CompareTo(a.puntaje));

        for (int i = 0; i < listaOrdenada.Count && i < 3; i++)
        {
            var nombreLabel = GetNode<Label>($"Contenido/Estadísticas/Fila{i + 1}/Nombre{i + 1}");
            var puntosLabel = GetNode<Label>($"Contenido/Estadísticas/Fila{i + 1}/Puntos{i + 1}");

            nombreLabel.Text = listaOrdenada[i].nombre;
            puntosLabel.Text = $"{listaOrdenada[i].puntaje} pts";
        }
    }

}
