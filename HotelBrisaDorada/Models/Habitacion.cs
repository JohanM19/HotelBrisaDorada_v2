namespace HotelBrisaDorada.Models;

public class Habitacion
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Resumen { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string ImagenUrl { get; init; } = string.Empty;
    public int CapacidadBase { get; init; }
    public int CapacidadMaxima { get; init; }
    public int Unidades { get; init; }
    public int Metros { get; init; }
    public string Camas { get; init; } = string.Empty;
    public decimal PrecioTemporadaBaja { get; init; }
    public decimal PrecioTemporadaAlta { get; init; }
    public IReadOnlyList<string> Comodidades { get; init; } = Array.Empty<string>();
}
