using Godot;
using System;

public partial class TreeNode2D : Node2D
{
    public int Value;

    public void SetValue(int value)
    {
        Value = value;
        GetNode<Label>("ValueLabel").Text = value.ToString();
    }
}
