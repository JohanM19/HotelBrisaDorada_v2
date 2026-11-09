namespace HotelBrisaDorada.Models;

public class Cotizacion
{
    public int Noches { get; init; }
    public decimal Alojamiento { get; init; }
    public decimal PersonasAdicionales { get; init; }
    public decimal Desayuno { get; init; }
    public decimal Total => Alojamiento + PersonasAdicionales + Desayuno;
}
