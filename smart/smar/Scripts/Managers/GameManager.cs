using Godot;
using System.Collections.Generic;
using BinaryTree;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance;

    [Export] public PackedScene TokenScene;
    [Export] public float TokenSpawnInterval = 2.0f;

    private Timer _tokenTimer;
    private Dictionary<Player, BST> _playerTrees = new Dictionary<Player, BST>();

    public override void _Ready()
    {
        Instance = this;
        InitializePlayers();
        StartTokenSpawn();
    }

    private void InitializePlayers()
    {
        foreach (Player player in GetTree().GetNodesInGroup("Players"))
        {
            _playerTrees.Add(
                player,
                new BST());
        }
    }

    private void StartTokenSpawn()
    {
        _tokenTimer = new Timer();
        AddChild(_tokenTimer);
        _tokenTimer.WaitTime = TokenSpawnInterval;
        _tokenTimer.Timeout += SpawnToken;
        _tokenTimer.Start();
    }

    private void SpawnToken()
    {
        var token = TokenScene.Instantiate() as Token;
        token.Position = new Vector2(
            new RandomNumberGenerator().RandfRange(-500, 500),
            -300
        );
        GetNode<Node2D>("/root/Main/Tokens").AddChild(token);
    }

    public void PlayerCollectToken(Player player, int value)
    {
        _playerTrees[player].Insert(value);
        CheckChallenge(player);
    }

    private void CheckChallenge(Player player)
    {
        throw new NotImplementedException();
    }
}
