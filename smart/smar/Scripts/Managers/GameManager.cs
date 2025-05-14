using Godot;
using System;
using System.Collections.Generic;
using BinaryTree;

public partial class GameManager : Node
{
    public static GameManager Instance;

    [Export] public PackedScene PlayerScene1;
    [Export] public PackedScene PlayerScene2;
    [Export] public PackedScene PlayerScene3;
    [Export] public PackedScene TokenScene;
    [Export] public float TokenSpawnInterval = 2.0f;
    [Export] public TreeVisualizer VisualJugador1;
    [Export] public TreeVisualizer VisualJugador2;
    [Export] public TreeVisualizer VisualJugador3; 

    private Timer _tokenTimer;

    private Dictionary<Player, JugadorProgreso> _jugadores = new();
    private Dictionary<Player, TreeVisualizer> _visualPorJugador = new();

    private bool _jugador1Activo = false;
    private bool _jugador2Activo = false;
    private bool _jugador3Activo = false;

    private Reto _retoGlobal;
    private int _rondaActual = 1;

    public override void _Ready()
    {
        Instance = this;

        _tokenTimer = new Timer();
        AddChild(_tokenTimer);
        _tokenTimer.WaitTime = TokenSpawnInterval;
        _tokenTimer.Timeout += SpawnToken;
        _tokenTimer.Start();

        _retoGlobal = ObtenerRetoAleatorio();

        CallDeferred(nameof(ActualizarHUDReto));
    }

    private void ActualizarHUDReto()
    {
        var hub = GetTree().Root.GetNode<HubUI>("TestLevel/HUB/HubUI");
        hub.MostrarReto(_retoGlobal);
        hub.ActualizarRonda(_rondaActual);
    }

    public override void _Process(double delta)
    {
        if (!_jugador1Activo && (
            Input.IsActionJustPressed("p1_move_left") ||
            Input.IsActionJustPressed("p1_move_right") ||
            Input.IsActionJustPressed("p1_jump")))
        {
            InstanciarJugador(1, new Vector2(300, -200));
            _jugador1Activo = true;
        }

        if (!_jugador2Activo && (
            Input.IsActionJustPressed("p2_move_left") ||
            Input.IsActionJustPressed("p2_move_right") ||
            Input.IsActionJustPressed("p2_jump")))
        {
            InstanciarJugador(2, new Vector2(900, -200));
            _jugador2Activo = true;
        }

        if (!_jugador3Activo && (
            Input.IsActionJustPressed("p3_move_left") ||
            Input.IsActionJustPressed("p3_move_right") ||
            Input.IsActionJustPressed("p3_jump")))
        {
            InstanciarJugador(3, new Vector2(600, -200)); 
            _jugador3Activo = true;
        }
    }

    private void InstanciarJugador(int numero, Vector2 posicion)
    {
        Player jugador = null;

        switch (numero)
        {
            case 1:
                jugador = PlayerScene1.Instantiate<Player>();
                break;
            case 2:
                jugador = PlayerScene2.Instantiate<Player>();
                break;
            case 3:
                jugador = PlayerScene3.Instantiate<Player>(); 
                break;
        }

        if (jugador == null) return;

        jugador.PlayerNumber = numero;
        jugador.GlobalPosition = posicion;
        jugador.Name = $"Player{numero}";
        jugador.AddToGroup("Players");

        var contenedorJugadores = GetParent().GetNode<Node2D>("Players");
        contenedorJugadores.AddChild(jugador);

        _jugadores[jugador] = new JugadorProgreso(_retoGlobal);
        GD.Print($"👤 Jugador {numero} instanciado con reto: {_retoGlobal.Descripcion}");

        switch (numero)
        {
            case 1: _visualPorJugador[jugador] = VisualJugador1; break;
            case 2: _visualPorJugador[jugador] = VisualJugador2; break;
            case 3: _visualPorJugador[jugador] = VisualJugador3; break;
        }
    }

    private void SpawnToken()
    {
        if (TokenScene == null) return;

        var tokenInstance = TokenScene.Instantiate();

        if (tokenInstance is Area2D token)
        {
            var rng = new RandomNumberGenerator();
            rng.Randomize();

            token.Position = new Vector2(
                rng.RandfRange(100, 1180),
                rng.RandfRange(-300, -100)
            );

            GetParent().GetNode<Node2D>("Tokens").AddChild(token);
        }
    }

    public void PlayerCollectToken(Player player, int value)
    {
        GD.Print($"📌 Jugador registrado en diccionario: {_jugadores.ContainsKey(player)}");
        GD.Print($"🎮 Número del jugador recibido: {player.PlayerNumber}");

        if (!_jugadores.ContainsKey(player)) return;

        var progreso = _jugadores[player];

        progreso.InsertarValor(value);
        player.AddScore(value);

        var hub = GetTree().Root.GetNode<HubUI>("TestLevel/HUB/HubUI");
        hub.ActualizarPuntaje(player.PlayerNumber, player.Score);

        GD.Print($"🎯 Jugador {player.PlayerNumber} recogió un token con valor {value}");

        if (_retoGlobal.Tipo == TipoArbol.BST)
        {
            var bst = progreso.Arbol as BST;
            if (_visualPorJugador.ContainsKey(player))
            {
                GD.Print("🌿 Llamando CreateVisualTree para BST");
                _visualPorJugador[player].CreateVisualTree(bst);
            }
        }
        else if (_retoGlobal.Tipo == TipoArbol.AVL)
        {
            var avl = progreso.Arbol as AVLTree;
            if (_visualPorJugador.ContainsKey(player))
            {
                GD.Print("🌳 Llamando CreateVisualTree para AVL");
                _visualPorJugador[player].CreateVisualTree(avl);
            }
        }

        if (_retoGlobal.Verificar(progreso.Arbol))
        {
            GD.Print($"✅ Jugador {player.PlayerNumber} completó el reto: {_retoGlobal.Descripcion}");

            _rondaActual++;
            hub.ActualizarRonda(_rondaActual);

            _retoGlobal = ObtenerRetoAleatorio();

            foreach (var kv in _jugadores)
            {
                kv.Value.CambiarReto(_retoGlobal);
                GD.Print($"🔁 Reiniciando árbol del jugador {kv.Key.PlayerNumber}");

                if (_retoGlobal.Tipo == TipoArbol.BST)
                    _visualPorJugador[kv.Key].CreateVisualTree(new BST());
                else
                    _visualPorJugador[kv.Key].CreateVisualTree(new AVLTree());
            }

            GD.Print($"🎯 Nuevo reto global: {_retoGlobal.Descripcion}");
            hub.MostrarReto(_retoGlobal);
        }
    }

    private Reto ObtenerRetoAleatorio()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        return RetosPredefinidos.Todos[rng.RandiRange(0, RetosPredefinidos.Todos.Count - 1)];
    }
}
