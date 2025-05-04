using Godot;
using System;

public partial class Player : CharacterBody2D
{
    // --- Exported vars ---
    [Export] public int PlayerNumber = 1;
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;
    [Export] public float KnockbackForce = 800f;
    [Export] public float AttackCooldown = 0.5f;
    [Export] public float AttackMovementMultiplier = 0.3f;
    [Export] public float ComboWindow = 0.2f;

    // Puntos y poderes
    private int _score = 0;
    [Export] public int ForcePushCost = 5;
    [Export] public int ShieldCost = 3;
    [Export] public int AirJumpCost = 4;

    // Parámetros de poderes
    [Export] public float ForcePushStrength = 1200f;
    [Export] public float ShieldDuration = 2f;
    [Export] public float AirJumpVelocity = -600f;

    // Estados
    private bool _shieldActive = false;
    private Timer _shieldTimer;
    private float _gravity;
    private AnimatedSprite2D _sprite;
    private Area2D _attackArea;
    private Timer _cooldownTimer;
    private Timer _comboTimer;
    private Vector2 _attackAreaBasePos;
    private Player _lastAttacker;
    private bool _isAttacking = false;
    private bool _canAttack = true;

    public override void _Ready()
    {
        // Init
        _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _attackArea = GetNode<Area2D>("AttackArea");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");

        // Connect cooldown timer to reset attack
        _cooldownTimer.Timeout += () =>
        {
            _canAttack = true;
            _isAttacking = false;
        };

        // Attack area
        _attackAreaBasePos = _attackArea.Position;
        _attackArea.Monitoring = false;
        _attackArea.BodyEntered += OnAttackHit;

        // Combo timer
        _comboTimer = new Timer { OneShot = true, WaitTime = ComboWindow };
        AddChild(_comboTimer);

        // Shield timer
        _shieldTimer = new Timer { OneShot = true, WaitTime = ShieldDuration };
        AddChild(_shieldTimer);
        _shieldTimer.Timeout += () => _shieldActive = false;

        // Señal animación (fix signature)
        _sprite.AnimationFinished += OnAnimationFinished;
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
        velocity.X = inputDir.X * Speed * (_isAttacking ? AttackMovementMultiplier : 1);

        // Salto
        if (Input.IsActionJustPressed($"p{PlayerNumber}_jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            CancelAttack();
            
        }

        // Ataque (re‑añadido)
        if (Input.IsActionJustPressed($"p{PlayerNumber}_attack") && _canAttack)
        {
            StartAttack(inputDir);
        }

        Velocity = velocity;
        MoveAndSlide();

        // Lógica poderes
        if (Input.IsActionJustPressed($"p{PlayerNumber}_force_push")) TryForcePush();
        if (Input.IsActionJustPressed($"p{PlayerNumber}_shield")) TryShield();
        if (Input.IsActionJustPressed($"p{PlayerNumber}_air_jump")) TryAirJump();

        // Out of bounds
        if (GlobalPosition.Y > 1000) DieAndRespawn();

        // Actualizar animación y estado
        HandleAttackState();
        UpdateAnimation(inputDir);
    }

    // --- Ataque ---
    private void StartAttack(Vector2 inputDir)
    {
        _isAttacking = true;
        _canAttack = false;
        _cooldownTimer.Start(AttackCooldown);

        if (inputDir.X == 0)
            inputDir.X = _sprite.FlipH ? -1 : 1;
        else
            _sprite.FlipH = inputDir.X < 0;

        _attackArea.Position = new Vector2(
            _attackAreaBasePos.X * (_sprite.FlipH ? -1 : 1),
            _attackAreaBasePos.Y);
        _attackArea.Monitoring = true;

        string nextAnim = _comboTimer.TimeLeft > 0 && _sprite.Animation == "Attack"
            ? "Attack2" : "Attack";
        _sprite.Play(nextAnim);
        _comboTimer.Start();
    }

    private void CancelAttack()
    {
        _isAttacking = false;
        _canAttack = true;
        _cooldownTimer.Stop();
        _comboTimer.Stop();
        _sprite.Play("Reposo");
        _attackArea.Monitoring = false;
        _attackArea.Position = _attackAreaBasePos; // reset posición
    }

    // Fix: include parameter so signal binds
    private void OnAnimationFinished()
    {
        StringName anim = _sprite.Animation;
        if (anim == "Attack" || anim == "Attack2")
        {
            _isAttacking = false;
            _attackArea.Monitoring = false;

            if (_comboTimer.TimeLeft > 0 && Input.IsActionPressed($"p{PlayerNumber}_attack"))
            {
                StartAttack(Input.GetVector(
                    $"p{PlayerNumber}_move_left",
                    $"p{PlayerNumber}_move_right",
                    "",
                    ""));
            }
            else
            {
                _canAttack = true;
            }
        }
    }

    private void HandleAttackState()
    {
        if (!_sprite.IsPlaying() ||
           (_sprite.Animation != "Attack" && _sprite.Animation != "Attack2"))
        {
            _attackArea.Monitoring = false;
            _attackArea.Position = _attackAreaBasePos;
        }
    }

    // --- Knockback & Score ---
    private void OnAttackHit(Node body)
    {
        if (body is Player target && target != this)
        {
            _lastAttacker = this;
            Vector2 dir = (target.GlobalPosition - GlobalPosition).Normalized();
            target.ReceiveKnockback(dir * KnockbackForce);
        }
    }

    public void ReceiveKnockback(Vector2 force)
    {
        if (_shieldActive) return;
        Velocity = force;
        MoveAndSlide();
    }

    private void DieAndRespawn()
    {
        if (_lastAttacker != null) _lastAttacker.AddScore(1);
        Velocity = Vector2.Zero;
        GlobalPosition = new Vector2(650, -300);
    }

    public void AddScore(int pts)
    {
        _score += pts;
        GD.Print($"Jugador {PlayerNumber} tiene {_score} puntos");
    }

    // --- Powers ---
    private void TryForcePush()
    {
        if (_score < ForcePushCost) return;
        _score -= ForcePushCost;
        foreach (var b in _attackArea.GetOverlappingBodies())
            if (b is Player t && t != this)
                t.ReceiveKnockback((t.GlobalPosition - GlobalPosition).Normalized() * ForcePushStrength);
    }

    private void TryShield()
    {
        if (_score < ShieldCost) return;
        _score -= ShieldCost;
        _shieldActive = true;
        _shieldTimer.Start();
    }

    private void TryAirJump()
    {
        if (_score < AirJumpCost) return;
        if (!IsOnFloor() && Velocity.Y > 0)
        {
            Vector2 v = Velocity;
            _score -= AirJumpCost;
            v.Y = AirJumpVelocity;
            Velocity = v;
        }
    }

    // --- Animations ---
    private void UpdateAnimation(Vector2 inputDir)
    {
        if (!IsOnFloor())
            _sprite.Play("Salto");
        else if (_isAttacking)
        {
            // la animación de ataque ya está en curso
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

        if (!_isAttacking)
            _attackArea.Position = new Vector2(
                _attackAreaBasePos.X * (_sprite.FlipH ? -1 : 1),
                _attackAreaBasePos.Y);
    }
}
