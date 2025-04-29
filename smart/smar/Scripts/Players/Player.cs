using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int PlayerNumber = 1;
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;

    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Gravedad
        if (!IsOnFloor())
            velocity.Y += _gravity * (float)delta;

        // Movimiento
        Vector2 inputDir = Input.GetVector(
            $"p{PlayerNumber}_move_left",
            $"p{PlayerNumber}_move_right",
            "",
            "");

        velocity.X = inputDir.X * Speed;

        // Salto
        if (Input.IsActionJustPressed($"p{PlayerNumber}_jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        Velocity = velocity;
        MoveAndSlide();
    }
}