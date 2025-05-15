using System;

public class ListaSimple<T>
{
    private class Nodo
    {
        public T Valor;
        public Nodo Siguiente;

        public Nodo(T valor)
        {
            Valor = valor;
            Siguiente = null;
        }
    }

    private Nodo cabeza;
    private int cantidad;

    public void Agregar(T valor)
    {
        Nodo nuevo = new Nodo(valor);

        if (cabeza == null)
        {
            cabeza = nuevo;
        }
        else
        {
            Nodo actual = cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevo;
        }

        cantidad++;
    }

    public T Obtener(int indice)
    {
        if (indice < 0 || indice >= cantidad)
            throw new IndexOutOfRangeException();

        Nodo actual = cabeza;
        for (int i = 0; i < indice; i++)
        {
            actual = actual.Siguiente;
        }

        return actual.Valor;
    }

    public int Contar()
    {
        return cantidad;
    }

    public bool EstaVacio()
    {
        return cantidad == 0;
    }

    public void Limpiar()
    {
        cabeza = null;
        cantidad = 0;
    }
}
