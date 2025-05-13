using System;

public enum TipoArbol { BST, AVL }

public class Reto
{
    public TipoArbol Tipo { get; }
    public string Descripcion { get; }
    private Func<object, bool> _verificador;

    public Reto(TipoArbol tipo, string descripcion, Func<object, bool> verificador)
    {
        Tipo = tipo;
        Descripcion = descripcion;
        _verificador = verificador;
    }

    public bool Verificar(object arbol)
    {
        return _verificador(arbol);
    }
}
