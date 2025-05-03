// TreeUI.cs
using Godot;
using System;
using System.Collections.Generic;

public partial class TreeUI : Control
{
    public void DrawTree(Node root, Vector2 startPosition)
    {
        Queue<Node> nodes = new Queue<Node>();
        nodes.Enqueue(root);

        float yOffset = 50f;
        float xSpacing = 200f;

        while (nodes.Count > 0)
        {
            var current = nodes.Dequeue();
            // Dibujar nodo
            DrawCircle(startPosition, 20f, Colors.White);

            if (!EqualityComparer<Variant>.Default.Equals(current.Get("Left"), default(Variant))) // Fix: Use Get method to access properties dynamically
            {
                DrawLine(startPosition, startPosition + new Vector2(-xSpacing, yOffset), Colors.White, 2f);
                nodes.Enqueue((Node)current.Get("Left")); // Cast to Node
            }

            if (!EqualityComparer<Variant>.Default.Equals(current.Get("Right"), default(Variant))) // Fix: Use Get method to access properties dynamically
            {
                DrawLine(startPosition, startPosition + new Vector2(xSpacing, yOffset), Colors.White, 2f);
                nodes.Enqueue((Node)current.Get("Right")); // Cast to Node
            }

            startPosition.Y += yOffset;
            xSpacing *= 0.5f;
        }
    }
}
