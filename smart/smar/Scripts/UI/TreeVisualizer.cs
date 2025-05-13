using Godot;
using BinaryTree;
using System;

public partial class TreeVisualizer : Node2D
{
    [Export] public PackedScene NodeScene;

    private const float XSpacing = 80f;
    private const float YSpacing = 50f;

    public void CreateVisualTree(BST bst)
    {
        ClearTree();

        var root = bst.GetRoot();
        if (root != null)
        {
            CreateNodeRecursive(root, 0, 0, 0, null);
        }
    }

    public void CreateVisualTree(AVLTree avl)
    {
        ClearTree();

        var root = avl.GetRoot();
        if (root != null)
        {
            CreateNodeRecursive(avl.GetRoot(), 0, 0, 0, null);
        }
    }

    private void CreateNodeRecursive(BST.Node node, int depth, float x, float y, TreeNode2D parent)
    {
        if (node == null) return;

        var visualNode = NodeScene.Instantiate<TreeNode2D>();
        visualNode.SetValue(node.Value);
        visualNode.Position = new Vector2(x, y);
        AddChild(visualNode);

        if (parent != null)
        {
            // Crear línea de conexión
            var line = new Line2D();
            line.DefaultColor = new Color(0.36f, 0.24f, 0.13f); // Café oscuro
            line.Width = 4.0f; // Más visible
            line.ZIndex = 0;
            line.ZAsRelative = true;

            line.AddPoint(parent.Position);
            line.AddPoint(visualNode.Position);

            // DEBUG: Confirmar conexión y posiciones
            GD.Print($"📌 Conectando nodo {node.Value} con padre en {parent.Position} → {visualNode.Position}");

            AddChild(line);
        }

        float offset = XSpacing / (depth + 1);

        CreateNodeRecursive(node.Left, depth + 1, x - offset, y + YSpacing, visualNode);
        CreateNodeRecursive(node.Right, depth + 1, x + offset, y + YSpacing, visualNode);
    }

    private void CreateNodeRecursive(AVLTree.Node node, int depth, float x, float y, TreeNode2D parent)
    {
        if (node == null) return;

        var visualNode = NodeScene.Instantiate<TreeNode2D>();
        visualNode.SetValue(node.Value);
        visualNode.Position = new Vector2(x, y);
        AddChild(visualNode);

        if (parent != null)
        {
            // Crear línea de conexión
            var line = new Line2D();
            line.DefaultColor = new Color(0.36f, 0.24f, 0.13f); // Café oscuro
            line.Width = 4.0f; // Más visible
            line.ZIndex = 0;
            line.ZAsRelative = true;

            line.AddPoint(parent.Position);
            line.AddPoint(visualNode.Position);

            // DEBUG: Confirmar conexión y posiciones
            GD.Print($"📌 Conectando nodo {node.Value} con padre en {parent.Position} → {visualNode.Position}");

            AddChild(line);
        }

        float offset = XSpacing / (depth + 1);

        CreateNodeRecursive(node.Left, depth + 1, x - offset, y + YSpacing, visualNode);
        CreateNodeRecursive(node.Right, depth + 1, x + offset, y + YSpacing, visualNode);
    }


    private void ClearTree()
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }
    }
}
