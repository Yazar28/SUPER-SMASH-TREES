using BinaryTree;

public class JugadorProgreso
{
    public Reto RetoActual { get; private set; }
    public object Arbol { get; private set; }

    public JugadorProgreso(Reto retoInicial)
    {
        CambiarReto(retoInicial);
    }

    public void CambiarReto(Reto nuevoReto)
    {
        RetoActual = nuevoReto;

        switch (nuevoReto.Tipo)
        {
            case TipoArbol.BST:
                Arbol = new BST();
                break;

            case TipoArbol.AVL:
                Arbol = new AVLTree();
                break;
        }
    }

    public void InsertarValor(int valor)
    {
        switch (RetoActual.Tipo)
        {
            case TipoArbol.BST:
                (Arbol as BST)?.Insert(valor);
                break;

            case TipoArbol.AVL:
                (Arbol as AVLTree)?.Insert(valor);
                break;
        }
    }
}
