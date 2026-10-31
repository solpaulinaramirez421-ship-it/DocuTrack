namespace DocuTrack.Models;

public class Nodo
{
    public string Nombre { get; set; }
    public bool EsCarpeta { get; set; }
    public Nodo? Izquierdo { get; set; }
    public Nodo? Derecho { get; set; }

    public Nodo(string nombre, bool esCarpeta)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

        Nombre = nombre.Trim();
        EsCarpeta = esCarpeta;
    }
}