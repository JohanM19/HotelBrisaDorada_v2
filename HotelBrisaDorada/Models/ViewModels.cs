using System.ComponentModel.DataAnnotations;

namespace HotelBrisaDorada.Models;

public class HomeViewModel
{
    public IReadOnlyList<Habitacion> Habitaciones { get; init; } = Array.Empty<Habitacion>();
    public IReadOnlyList<Servicio> Servicios { get; init; } = Array.Empty<Servicio>();
}

public class DisponibilidadViewModel
{
    [DataType(DataType.Date)]
    public DateTime? FechaEntrada { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaSalida { get; set; }

    public int Personas { get; set; } = 2;
    public bool Consultado { get; set; }
    public string? Error { get; set; }
    public List<ResultadoDisponibilidad> Resultados { get; set; } = new();
}

public class ResultadoDisponibilidad
{
    public Habitacion Habitacion { get; init; } = null!;
    public int Disponibles { get; init; }
    public bool CabePersonas { get; init; }
    public Cotizacion? Cotizacion { get; init; }
}

public class ConfirmacionViewModel
{
    public Reserva Reserva { get; init; } = null!;
    public Habitacion Habitacion { get; init; } = null!;
    public Cotizacion Cotizacion { get; init; } = null!;
}
