using System;

namespace BinaryTree
{
    public class BST
    {
        public class Node
        {
            public int Value;
            public Node Left;
            public Node Right;

            public Node(int value) => Value = value;
        }

        private Node _root;
        private int _nodeCount = 0;

        public int Count => _nodeCount;

        public Node GetRoot()
        {
            return _root;
        }

        public void Insert(int value)
        {
            _root = InsertRec(_root, value);
        }

        private Node InsertRec(Node node, int value)
        {
            if (node == null)
            {
                _nodeCount++;
                return new Node(value);
            }

            if (value < node.Value)
                node.Left = InsertRec(node.Left, value);
            else if (value > node.Value)
                node.Right = InsertRec(node.Right, value);

            return node;
        }

        public int GetDepth()
        {
            return CalculateDepth(_root);
        }

        private int CalculateDepth(Node node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(
                CalculateDepth(node.Left),
                CalculateDepth(node.Right)
            );
        }

        public bool Contains(int value)
        {
            return ContainsRec(_root, value);
        }

        private bool ContainsRec(Node node, int value)
        {
            if (node == null) return false;
            if (value == node.Value) return true;
            if (value < node.Value) return ContainsRec(node.Left, value);
            return ContainsRec(node.Right, value);
        }
    }
}
