using System;
using System.Collections.Generic;

public class MapaSimple<K, V>
{
    private class Par
    {
        public K Clave;
        public V Valor;

        public Par(K clave, V valor)
        {
            Clave = clave;
            Valor = valor;
        }
    }

    private List<Par> pares = new();

    public void Agregar(K clave, V valor)
    {
        for (int i = 0; i < pares.Count; i++)
        {
            if (Equals(pares[i].Clave, clave))
            {
                pares[i].Valor = valor; // Actualiza si ya existe
                return;
            }
        }
        pares.Add(new Par(clave, valor));
    }

    public V Obtener(K clave)
    {
        foreach (var par in pares)
        {
            if (Equals(par.Clave, clave))
                return par.Valor;
        }
        throw new KeyNotFoundException($"Clave no encontrada: {clave}");
    }

    public bool Contiene(K clave)
    {
        foreach (var par in pares)
        {
            if (Equals(par.Clave, clave))
                return true;
        }
        return false;
    }

    public IEnumerable<(K Clave, V Valor)> ObtenerTodo()
    {
        foreach (var par in pares)
            yield return (par.Clave, par.Valor);
    }
}
