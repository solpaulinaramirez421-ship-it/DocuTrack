using DocuTrack.Models;
using DocuTrack.Views;

namespace DocuTrack.Controllers;

public class DocuTrackController
{
    private readonly ArbolBinario _arbol = new();
    private readonly ConsolaView _vista = new();

    // Orden de inserción pensado para que todos los archivos queden como hojas.
    private static readonly (string Nombre, bool EsCarpeta)[] NodosIniciales =
    {
        ("Documentos", true),
        ("Contratos", true),
        ("Proyectos", true),
        ("Balances", true),
        ("cotizaciones.xlsx", false),
        ("Informes", true),
        ("Respaldos", true),
        ("Actas", true),
        ("Facturas", true),
        ("manual.docx", false),
        ("reportes.pdf", false),
        ("Usuarios", true),
        ("acta1.pdf", false),
        ("tareas.txt", false)
    };

    public void Ejecutar()
    {
        ConstruirArbol();
        RealizarBusquedas();
        RealizarActualizaciones();
        RealizarEliminaciones();
        MostrarRecorridos();
        MostrarAlturaFinal();
    }

    private void ConstruirArbol()
    {
        _vista.MostrarEncabezado("CASO 1: CONSTRUCCIÓN DEL ÁRBOL");

        foreach (var (nombre, esCarpeta) in NodosIniciales)
            Insertar(nombre, esCarpeta);

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Prueba de política de duplicados (sin distinguir mayúsculas):");
        Insertar("ACTAS", true);

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Árbol inicial:");
        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void RealizarBusquedas()
    {
        _vista.MostrarEncabezado("CASO 2: BÚSQUEDAS RÁPIDAS");

        _vista.MostrarMensaje("Existentes en el subárbol izquierdo:");
        Buscar("ACTAS");
        Buscar("cotizaciones.xlsx");

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Existentes en el subárbol derecho:");
        Buscar("manual.docx");
        Buscar("tareas.txt");

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Inexistentes:");
        Buscar("presupuesto.xlsx");
        Buscar("Nomina");
    }

    private void RealizarActualizaciones()
    {
        _vista.MostrarEncabezado("CASO 3: ACTUALIZACIONES SELECTIVAS");

        _vista.MostrarMensaje("Actualizar un nodo hoja (archivo):");
        Actualizar("manual.docx", "manual_v2.docx");
        _vista.MostrarArbol(_arbol.Raiz);

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Actualizar un nodo con un solo hijo:");
        Actualizar("Usuarios", "Clientes");
        _vista.MostrarArbol(_arbol.Raiz);

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Actualizar la raíz:");
        if (_arbol.Raiz != null)
            Actualizar(_arbol.Raiz.Nombre, "Gerencia");
        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void RealizarEliminaciones()
    {
        _vista.MostrarEncabezado("CASO 4: ELIMINACIONES SELECTIVAS");

        _vista.MostrarMensaje("Eliminar un nodo hoja:");
        Eliminar("cotizaciones.xlsx");
        _vista.MostrarArbol(_arbol.Raiz);

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Eliminar un nodo con un solo hijo:");
        Eliminar("Actas");
        _vista.MostrarArbol(_arbol.Raiz);

        _vista.MostrarMensaje("");
        _vista.MostrarMensaje("Eliminar la raíz (dos hijos, se usa el sucesor):");
        if (_arbol.Raiz != null)
            Eliminar(_arbol.Raiz.Nombre);
        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void MostrarRecorridos()
    {
        _vista.MostrarEncabezado("CASO 5: RECORRIDOS DE VERIFICACIÓN");

        _vista.MostrarRecorrido("Preorden ", _arbol.Preorden());
        _vista.MostrarRecorrido("Inorden  ", _arbol.Inorden());
        _vista.MostrarRecorrido("Postorden", _arbol.Postorden());
        _vista.MostrarRecorrido("Por niveles", _arbol.PorNiveles());
    }

    private void MostrarAlturaFinal()
    {
        _vista.MostrarEncabezado("CASO 6: ALTURA FINAL");
        _vista.MostrarAltura(_arbol.Altura());
    }

    private void Insertar(string nombre, bool esCarpeta)
    {
        bool exito = _arbol.Insertar(nombre, esCarpeta);
        _vista.MostrarInsercion(nombre, esCarpeta, exito, _arbol.UltimasComparaciones);
    }

    private void Buscar(string nombre)
    {
        Nodo? nodo = _arbol.Buscar(nombre);
        _vista.MostrarBusqueda(nombre, nodo, _arbol.UltimasComparaciones);
    }

    private void Eliminar(string nombre)
    {
        CasoEliminacion caso = _arbol.Eliminar(nombre);
        _vista.MostrarEliminacion(nombre, caso, _arbol.UltimasComparaciones);
    }

    // Actualizar = Eliminar(nombreAntiguo) + Insertar(nombreNuevo), validando antes.
    private void Actualizar(string nombreAntiguo, string nombreNuevo)
    {
        if (string.IsNullOrWhiteSpace(nombreNuevo))
        {
            _vista.MostrarActualizacion(nombreAntiguo, nombreNuevo, false, "El nombre nuevo no puede estar vacío.");
            return;
        }

        Nodo? existente = _arbol.Buscar(nombreAntiguo);
        if (existente == null)
        {
            _vista.MostrarActualizacion(nombreAntiguo, nombreNuevo, false, "El nombre antiguo no existe.");
            return;
        }

        if (_arbol.Buscar(nombreNuevo) != null)
        {
            _vista.MostrarActualizacion(nombreAntiguo, nombreNuevo, false, "El nombre nuevo ya existe.");
            return;
        }

        bool esCarpeta = existente.EsCarpeta;

        CasoEliminacion caso = _arbol.Eliminar(nombreAntiguo);
        _vista.MostrarEliminacion(nombreAntiguo, caso, _arbol.UltimasComparaciones);

        bool insertado = _arbol.Insertar(nombreNuevo, esCarpeta);
        _vista.MostrarInsercion(nombreNuevo, esCarpeta, insertado, _arbol.UltimasComparaciones);

        _vista.MostrarActualizacion(nombreAntiguo, nombreNuevo, insertado, "Eliminar + Insertar completado.");
    }
}