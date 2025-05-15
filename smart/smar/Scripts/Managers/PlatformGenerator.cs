using Godot;
using System;
using System.Collections.Generic;

public partial class PlatformGenerator : Node2D
{
    private ListaSimple<PackedScene> platformTypes = new ListaSimple<PackedScene>();

    public override void _Ready()
    {
        platformTypes.Agregar(GD.Load<PackedScene>("res://Scenes/Platform/plataformaPeque.tscn"));
        platformTypes.Agregar(GD.Load<PackedScene>("res://Scenes/Platform/plataformaMediana.tscn"));
        platformTypes.Agregar(GD.Load<PackedScene>("res://Scenes/Platform/plataformaGrande.tscn"));
        platformTypes.Agregar(GD.Load<PackedScene>("res://Scenes/Platform/plataformaEnorme.tscn"));

        // ✅ Esto es lo que faltaba
        GeneratePlatforms(5, 150f);   // Columna izquierda
        GeneratePlatforms(5, 600f);   // Columna central
        GeneratePlatforms(5, 1000f);  // Columna derecha
    }

    private void GeneratePlatforms(int amount, float startX)
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        float x = startX;
        float y = 600f;
        bool goRight = true;

        for (int i = 0; i < amount; i++)
        {
            var scene = platformTypes.Obtener(rng.RandiRange(0, platformTypes.Contar() - 1));
            var platform = scene.Instantiate<Node2D>();
            platform.Scale = new Vector2(0.5f, 0.5f);
            platform.Position = new Vector2(x, y);
            AddChild(platform);

            float direction = goRight ? 1f : -1f;
            goRight = !goRight;

            x += direction * rng.RandfRange(120f, 180f);
            x = Mathf.Clamp(x, 100f, 1180f);

            y -= rng.RandfRange(100f, 140f);
        }
    }
}
