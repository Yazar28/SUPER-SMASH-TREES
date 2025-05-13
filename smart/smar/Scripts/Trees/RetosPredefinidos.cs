using System.Collections.Generic;

public static class RetosPredefinidos
{
    public static List<Reto> Todos = new List<Reto>
    {
        // --- SOLO BST ---
        new Reto(TipoArbol.BST, "Construye un BST con 6 nodos",
            arbol => (arbol as BinaryTree.BST)?.Count == 6),

        new Reto(TipoArbol.BST, "Construye un BST con profundidad 4",
            arbol => (arbol as BinaryTree.BST)?.GetDepth() == 4),

        // --- AVL ---
        new Reto(TipoArbol.AVL, "Construye un AVL con al menos 5 nodos",
            arbol => (arbol as BinaryTree.AVLTree)?.Count >= 5),

        new Reto(TipoArbol.AVL, "Construye un AVL con exactamente 7 nodos",
            arbol => (arbol as BinaryTree.AVLTree)?.Count == 7),
    };


}
