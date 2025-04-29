// BST.cs
using System;
namespace BinaryTree
{
    // Binary Search Tree (BST) implementation
    // This class is used to manage the player's score tree
    // and calculate the depth of the tree.
    public class BST
    {
        private class Node
        {
            public int Value;
            public Node Left;
            public Node Right;

            public Node(int value) => Value = value;
        }

        private Node _root;
        public int NodeCount { get; private set; }

        public void Insert(int value)
        {
            _root = InsertRec(_root, value);
            NodeCount++;
        }

        private Node InsertRec(Node node, int value)
        {
            if (node == null) return new Node(value);

            if (value < node.Value)
                node.Left = InsertRec(node.Left, value);
            else if (value > node.Value)
                node.Right = InsertRec(node.Right, value);

            return node;
        }

        public int GetDepth() => CalculateDepth(_root);

        private int CalculateDepth(Node node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(
                CalculateDepth(node.Left),
                CalculateDepth(node.Right)
            );
        }
    }
}