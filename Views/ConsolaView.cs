using System.Text;
using DocuTrack.Models;

namespace DocuTrack.Views;

public class ConsolaView
{
    public ConsolaView()
    {
        // Necesario para que la consola muestre ├── └── │ correctamente.
        Console.OutputEncoding = Encoding.UTF8;
    }

    public void MostrarEncabezado(string titulo)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine(titulo);
        Console.WriteLine(new string('=', 60));
    }

    public void MostrarMensaje(string mensaje)
    {
        Console.WriteLine(mensaje);
    }

    public void MostrarInsercion(string nombre, bool esCarpeta, bool exito, int comparaciones)
    {
        string tipo = esCarpeta ? "carpeta" : "archivo";

        if (exito)
            Console.WriteLine($"Insertado: '{nombre}' ({tipo}) - comparaciones: {comparaciones}");
        else
            Console.WriteLine($"DUPLICADO RECHAZADO: '{nombre}' ya existe - comparaciones: {comparaciones}");
    }

    public void MostrarBusqueda(string nombre, Nodo? nodo, int comparaciones)
    {
        if (nodo != null)
        {
            string tipo = nodo.EsCarpeta ? "carpeta" : "archivo";
            Console.WriteLine($"Buscar '{nombre}': HALLADO ({tipo}) - comparaciones: {comparaciones}");
        }
        else
        {
            Console.WriteLine($"Buscar '{nombre}': NO HALLADO - comparaciones: {comparaciones}");
        }
    }

    public void MostrarEliminacion(string nombre, CasoEliminacion caso, int comparaciones)
    {
        string descripcion = caso switch
        {
            CasoEliminacion.Hoja => "caso HOJA (se desenlazó)",
            CasoEliminacion.UnHijo => "caso UN HIJO (el hijo se reconectó con el padre)",
            CasoEliminacion.DosHijos => "caso DOS HIJOS (reemplazado por el sucesor)",
            _ => "NO ENCONTRADO"
        };

        Console.WriteLine($"Eliminar '{nombre}': {descripcion} - comparaciones: {comparaciones}");
    }

    public void MostrarActualizacion(string nombreAntiguo, string nombreNuevo, bool exito, string detalle)
    {
        string estado = exito ? "OK" : "FALLÓ";
        Console.WriteLine($"Actualizar '{nombreAntiguo}' -> '{nombreNuevo}': {estado}. {detalle}");
    }

    public void MostrarArbol(Nodo? raiz)
    {
        Console.WriteLine("Leyenda: [C] carpeta, [A] archivo, (I) hijo izquierdo, (D) hijo derecho");

        if (raiz == null)
        {
            Console.WriteLine("(árbol vacío)");
            return;
        }

        Console.WriteLine(Etiqueta(raiz));
        ImprimirHijos(raiz, "");
    }

    public void MostrarRecorrido(string titulo, List<string> nombres)
    {
        Console.WriteLine($"{titulo}: {string.Join(", ", nombres)}");
    }

    public void MostrarAltura(int altura)
    {
        Console.WriteLine($"Altura del árbol: {altura} niveles");
    }

    private void ImprimirHijos(Nodo nodo, string prefijo)
    {
        var hijos = new List<(Nodo Hijo, string Lado)>();
        if (nodo.Izquierdo != null) hijos.Add((nodo.Izquierdo, "I"));
        if (nodo.Derecho != null) hijos.Add((nodo.Derecho, "D"));

        for (int i = 0; i < hijos.Count; i++)
        {
            bool esUltimo = i == hijos.Count - 1;
            var (hijo, lado) = hijos[i];

            string conector = esUltimo ? "└── " : "├── ";
            Console.WriteLine($"{prefijo}{conector}({lado}) {Etiqueta(hijo)}");

            string nuevoPrefijo = prefijo + (esUltimo ? "    " : "│   ");
            ImprimirHijos(hijo, nuevoPrefijo);
        }
    }

    private static string Etiqueta(Nodo nodo)
    {
        return nodo.EsCarpeta ? $"[C] {nodo.Nombre}" : $"[A] {nodo.Nombre}";
    }
}