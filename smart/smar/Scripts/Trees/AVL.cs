using System;

namespace BinaryTree
{
    public class AVLTree
    {
        public class Node
        {
            public int Value;
            public Node Left;
            public Node Right;
            public int Height;

            public Node(int value)
            {
                Value = value;
                Height = 1;
            }
        }

        private Node _root;
        public int Count { get; private set; }

        public Node GetRoot()
        {
            return _root;
        }

        public void Insert(int value)
        {
            _root = Insert(_root, value);
            Count++;
        }

        private Node Insert(Node node, int value)
        {
            if (node == null)
                return new Node(value);

            if (value < node.Value)
                node.Left = Insert(node.Left, value);
            else if (value > node.Value)
                node.Right = Insert(node.Right, value);
            else
                return node;

            UpdateHeight(node);
            return Balance(node);
        }

        private void UpdateHeight(Node node)
        {
            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
        }

        private int Height(Node node) => node?.Height ?? 0;

        private int GetBalance(Node node) =>
            node == null ? 0 : Height(node.Left) - Height(node.Right);

        private Node Balance(Node node)
        {
            int balance = GetBalance(node);

            if (balance < -1 && GetBalance(node.Right) <= 0)
                return RotateLeft(node);

            if (balance > 1 && GetBalance(node.Left) >= 0)
                return RotateRight(node);

            if (balance < -1 && GetBalance(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            if (balance > 1 && GetBalance(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            return node;
        }

        private Node RotateLeft(Node z)
        {
            Node y = z.Right;
            Node T2 = y.Left;

            y.Left = z;
            z.Right = T2;

            UpdateHeight(z);
            UpdateHeight(y);

            return y;
        }

        private Node RotateRight(Node z)
        {
            Node y = z.Left;
            Node T3 = y.Right;

            y.Right = z;
            z.Left = T3;

            UpdateHeight(z);
            UpdateHeight(y);

            return y;
        }
    }
}
