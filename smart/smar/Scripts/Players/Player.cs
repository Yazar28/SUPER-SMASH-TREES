using Godot;
using System;
public partial class Player : CharacterBody2D
{
    // Mantenemos las mismas variables Export
    [Export] public int PlayerNumber = 1;
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;
    [Export] public float KnockbackForce = 800f;
    [Export] public float AttackCooldown = 0.5f;
    [Export] public float AttackMovementMultiplier = 0.3f;
    [Export] public float ComboWindow = 0.2f;

    private float _gravity;
    private AnimatedSprite2D _sprite;
    private Area2D _attackArea;
    private Timer _cooldownTimer;
    private bool _isAttacking = false;
    private bool _canAttack = true;
    private Vector2 _preAttackVelocity;
    private float _currentAttackCooldown;

    public override void _Ready()
    {
        _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _attackArea = GetNode<Area2D>("AttackArea");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");

        _attackArea.BodyEntered += OnAttackHit;
        //_sprite.AnimationFinished += OnAnimationFinished;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Gravedad
        if (!IsOnFloor())
            velocity.Y += _gravity * (float)delta;
        else
            velocity.Y = 0;

        // Movimiento
        Vector2 inputDir = Input.GetVector(
            $"p{PlayerNumber}_move_left",
            $"p{PlayerNumber}_move_right",
            "",
            "");

        // Manejar movimiento durante ataque
        if (_isAttacking)
        {
            velocity.X = inputDir.X * Speed * AttackMovementMultiplier;
        }
        else
        {
            velocity.X = inputDir.X * Speed;
        }

        // Salto
        if (Input.IsActionJustPressed($"p{PlayerNumber}_jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            CancelAttack(); // Cancelar cualquier ataque al saltar
        }

        // Ataque
        if (Input.IsActionJustPressed($"p{PlayerNumber}_attack") && _canAttack && IsOnFloor())
        {
            StartAttack(inputDir);
            _preAttackVelocity = velocity;
        }

        Velocity = velocity;
        MoveAndSlide();

        UpdateAnimation(inputDir);
    }

    private void StartAttack(Vector2 inputDir)
    {
        _isAttacking = true;
        _canAttack = false;
        _cooldownTimer.Start(AttackCooldown);

        // Determinar dirección del ataque
        if (inputDir.X != 0)
        {
            _sprite.FlipH = inputDir.X < 0;
        }

        // Selección de animación de ataque
        if (_sprite.Animation == "Attack" && _sprite.IsPlaying())
        {
            _sprite.Play("Attack2");
        }
        else
        {
            _sprite.Play("Attack");
        }
    }

    private void CancelAttack()
    {
        _isAttacking = false;
        _canAttack = true;
        _cooldownTimer.Stop();
    }

    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "Attack" || animName == "Attack2")
        {
            _isAttacking = false;

            // Permitir combo si se mantiene presionado el botón
            if (Input.IsActionPressed($"p{PlayerNumber}_attack") && _canAttack)
            {
                StartAttack(Input.GetVector(
                    $"p{PlayerNumber}_move_left",
                    $"p{PlayerNumber}_move_right",
                    "",
                    ""));
            }
        }
    }

    private void UpdateAnimation(Vector2 inputDir)
    {
        // Prioridad de animaciones
        if (!IsOnFloor())
        {
            _sprite.Play("Salto");
        }
        else if (_isAttacking)
        {
            // La animación de ataque ya está siendo manejada
        }
        else if (Mathf.Abs(Velocity.X) > 1f)
        {
            _sprite.Play("Run");
            _sprite.FlipH = inputDir.X < 0;
        }
        else
        {
            _sprite.Play("Reposo");
        }
    }

    private void OnAttackHit(Node body)
    {
        if (body is Player targetPlayer && targetPlayer != this)
        {
            Vector2 knockbackDirection = (targetPlayer.GlobalPosition - GlobalPosition).Normalized();
            targetPlayer.ApplyKnockback(knockbackDirection * KnockbackForce);
        }
    }

    public void ApplyKnockback(Vector2 force)
    {
        Velocity = force;
        MoveAndSlide();
    }

    private void OnCooldownTimerTimeout()
    {
        _canAttack = true;
        _isAttacking = false;
    }

}