public static class RetosPredefinidos
{
    public static ListaSimple<Reto> Todos = CrearLista();

    private static ListaSimple<Reto> CrearLista()
    {
        var lista = new ListaSimple<Reto>();

        lista.Agregar(new Reto(TipoArbol.BST, "Construye un BST con 6 nodos",
            arbol => (arbol as BinaryTree.BST)?.Count == 6));

        lista.Agregar(new Reto(TipoArbol.BST, "Construye un BST con profundidad 4",
            arbol => (arbol as BinaryTree.BST)?.GetDepth() == 4));

        lista.Agregar(new Reto(TipoArbol.AVL, "Construye un AVL con al menos 5 nodos",
            arbol => (arbol as BinaryTree.AVLTree)?.Count >= 5));

        lista.Agregar(new Reto(TipoArbol.AVL, "Construye un AVL con exactamente 7 nodos",
            arbol => (arbol as BinaryTree.AVLTree)?.Count == 7));

        return lista;
    }
}
