namespace DocuTrack.Models;

public partial class ArbolBinario
{
    // Raíz, izquierdo, derecho.
    public List<string> Preorden()
    {
        var resultado = new List<string>();
        Preorden(Raiz, resultado);
        return resultado;
    }

    private static void Preorden(Nodo? nodo, List<string> resultado)
    {
        if (nodo == null) return;
        resultado.Add(nodo.Nombre);
        Preorden(nodo.Izquierdo, resultado);
        Preorden(nodo.Derecho, resultado);
    }

    // Izquierdo, raíz, derecho. En un BST entrega los nombres ordenados.
    public List<string> Inorden()
    {
        var resultado = new List<string>();
        Inorden(Raiz, resultado);
        return resultado;
    }

    private static void Inorden(Nodo? nodo, List<string> resultado)
    {
        if (nodo == null) return;
        Inorden(nodo.Izquierdo, resultado);
        resultado.Add(nodo.Nombre);
        Inorden(nodo.Derecho, resultado);
    }

    // Izquierdo, derecho, raíz.
    public List<string> Postorden()
    {
        var resultado = new List<string>();
        Postorden(Raiz, resultado);
        return resultado;
    }

    private static void Postorden(Nodo? nodo, List<string> resultado)
    {
        if (nodo == null) return;
        Postorden(nodo.Izquierdo, resultado);
        Postorden(nodo.Derecho, resultado);
        resultado.Add(nodo.Nombre);
    }

    // Nivel por nivel usando una cola.
    public List<string> PorNiveles()
    {
        var resultado = new List<string>();
        if (Raiz == null) return resultado;

        var cola = new Queue<Nodo>();
        cola.Enqueue(Raiz);

        while (cola.Count > 0)
        {
            Nodo actual = cola.Dequeue();
            resultado.Add(actual.Nombre);

            if (actual.Izquierdo != null) cola.Enqueue(actual.Izquierdo);
            if (actual.Derecho != null) cola.Enqueue(actual.Derecho);
        }

        return resultado;
    }

    // Altura en niveles: árbol vacío = 0, solo raíz = 1.
    public int Altura() => Altura(Raiz);

    private static int Altura(Nodo? nodo)
    {
        if (nodo == null) return 0;
        return 1 + Math.Max(Altura(nodo.Izquierdo), Altura(nodo.Derecho));
    }
}