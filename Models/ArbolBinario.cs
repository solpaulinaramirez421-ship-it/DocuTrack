namespace DocuTrack.Models;

// Indica qué caso de eliminación se ejecutó (la View lo mostrará).
public enum CasoEliminacion
{
    NoEncontrado,
    Hoja,
    UnHijo,
    DosHijos
}

public partial class ArbolBinario
{
    public Nodo? Raiz { get; private set; }

    // Comparaciones hechas en la última operación (para mostrarlas en la View).
    public int UltimasComparaciones { get; private set; }

    // Devuelve false si el nombre ya existe (duplicado rechazado).
    public bool Insertar(string nombre, bool esCarpeta)
    {
        var nuevo = new Nodo(nombre, esCarpeta);
        UltimasComparaciones = 0;

        if (Raiz == null)
        {
            Raiz = nuevo;
            return true;
        }

        Nodo actual = Raiz;
        while (true)
        {
            UltimasComparaciones++;
            int resultado = string.Compare(nuevo.Nombre, actual.Nombre, StringComparison.OrdinalIgnoreCase);

            if (resultado == 0)
                return false;

            if (resultado < 0)
            {
                if (actual.Izquierdo == null)
                {
                    actual.Izquierdo = nuevo;
                    return true;
                }
                actual = actual.Izquierdo;
            }
            else
            {
                if (actual.Derecho == null)
                {
                    actual.Derecho = nuevo;
                    return true;
                }
                actual = actual.Derecho;
            }
        }
    }

    // Devuelve el nodo si existe; null si no se encuentra.
    public Nodo? Buscar(string nombre)
    {
        UltimasComparaciones = 0;

        if (string.IsNullOrWhiteSpace(nombre))
            return null;

        Nodo? actual = Raiz;
        while (actual != null)
        {
            UltimasComparaciones++;
            int resultado = string.Compare(nombre.Trim(), actual.Nombre, StringComparison.OrdinalIgnoreCase);

            if (resultado == 0)
                return actual;

            actual = resultado < 0 ? actual.Izquierdo : actual.Derecho;
        }

        return null;
    }

    // Elimina el nodo y devuelve qué caso se aplicó.
    public CasoEliminacion Eliminar(string nombre)
    {
        UltimasComparaciones = 0;

        if (string.IsNullOrWhiteSpace(nombre))
            return CasoEliminacion.NoEncontrado;

        var caso = CasoEliminacion.NoEncontrado;
        Raiz = EliminarEn(Raiz, nombre.Trim(), ref caso);
        return caso;
    }

    private Nodo? EliminarEn(Nodo? nodo, string nombre, ref CasoEliminacion caso)
    {
        if (nodo == null)
            return null;

        UltimasComparaciones++;
        int resultado = string.Compare(nombre, nodo.Nombre, StringComparison.OrdinalIgnoreCase);

        if (resultado < 0)
        {
            nodo.Izquierdo = EliminarEn(nodo.Izquierdo, nombre, ref caso);
        }
        else if (resultado > 0)
        {
            nodo.Derecho = EliminarEn(nodo.Derecho, nombre, ref caso);
        }
        else
        {
            // Caso 1: hoja, se desenlaza.
            if (nodo.Izquierdo == null && nodo.Derecho == null)
            {
                caso = CasoEliminacion.Hoja;
                return null;
            }

            // Caso 2: un solo hijo, el hijo ocupa su lugar.
            if (nodo.Izquierdo == null)
            {
                caso = CasoEliminacion.UnHijo;
                return nodo.Derecho;
            }
            if (nodo.Derecho == null)
            {
                caso = CasoEliminacion.UnHijo;
                return nodo.Izquierdo;
            }

            // Caso 3: dos hijos, se reemplaza por el sucesor (mínimo del subárbol derecho).
            Nodo sucesor = Minimo(nodo.Derecho);
            nodo.Nombre = sucesor.Nombre;
            nodo.EsCarpeta = sucesor.EsCarpeta;

            var casoInterno = CasoEliminacion.NoEncontrado;
            nodo.Derecho = EliminarEn(nodo.Derecho, sucesor.Nombre, ref casoInterno);
            caso = CasoEliminacion.DosHijos;
        }

        return nodo;
    }

    private static Nodo Minimo(Nodo nodo)
    {
        while (nodo.Izquierdo != null)
            nodo = nodo.Izquierdo;
        return nodo;
    }
}