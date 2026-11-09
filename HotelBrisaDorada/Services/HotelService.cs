using HotelBrisaDorada.Models;

namespace HotelBrisaDorada.Services;

/// <summary>
/// Catálogo del hotel y reglas de negocio (disponibilidad, tarifas y registro de reservas).
/// Se registra como Singleton y guarda las reservas en memoria.
/// </summary>
public class HotelService
{
    public const decimal TarifaPersonaAdicional = 60000m;
    public const decimal TarifaDesayuno = 25000m;

    private readonly object _candado = new();
    private readonly List<Reserva> _reservas = new();

    public IReadOnlyList<Habitacion> Habitaciones { get; } = new List<Habitacion>
    {
        new Habitacion
        {
            Id = 1,
            Nombre = "Habitación estándar",
            Resumen = "Un ambiente acogedor para descansar después de un día de actividades.",
            Descripcion = "Pensada para quienes quieren descansar bien sin complicaciones: cama cómoda, baño privado y todo lo necesario para recorrer Guatapé durante el día y volver a un espacio tranquilo.",
            ImagenUrl = "https://images.unsplash.com/photo-1611892440504-42a792e24d32?auto=format&fit=crop&w=1000&q=80",
            CapacidadBase = 2,
            CapacidadMaxima = 3,
            Unidades = 6,
            Metros = 22,
            Camas = "1 cama doble",
            PrecioTemporadaBaja = 180000m,
            PrecioTemporadaAlta = 220000m,
            Comodidades = new[] { "Baño privado con agua caliente", "Wi-Fi", "Televisión", "Ventilador de techo" }
        },
        new Habitacion
        {
            Id = 2,
            Nombre = "Habitación superior",
            Resumen = "Más amplitud y comodidad para disfrutar una estadía especial.",
            Descripcion = "Un espacio más amplio y luminoso, con zona de descanso y una distribución que invita a quedarse. Ideal para parejas o para quienes buscan algo más de comodidad.",
            ImagenUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427?auto=format&fit=crop&w=1000&q=80",
            CapacidadBase = 2,
            CapacidadMaxima = 3,
            Unidades = 4,
            Metros = 30,
            Camas = "1 cama queen",
            PrecioTemporadaBaja = 250000m,
            PrecioTemporadaAlta = 300000m,
            Comodidades = new[] { "Baño privado con agua caliente", "Wi-Fi", "Televisión", "Zona de estar", "Escritorio" }
        },
        new Habitacion
        {
            Id = 3,
            Nombre = "Suite familiar",
            Resumen = "Ideal para compartir en familia, con espacios pensados para todos.",
            Descripcion = "La opción para viajar en grupo: ambientes separados, camas para todos y espacio de sobra para guardar el equipaje y compartir después de un día de paseo por el embalse.",
            ImagenUrl = "https://images.unsplash.com/photo-1616486338812-3dadae4b4ace?auto=format&fit=crop&w=1000&q=80",
            CapacidadBase = 4,
            CapacidadMaxima = 5,
            Unidades = 2,
            Metros = 48,
            Camas = "1 cama doble y 2 camas sencillas",
            PrecioTemporadaBaja = 340000m,
            PrecioTemporadaAlta = 400000m,
            Comodidades = new[] { "Baño privado con agua caliente", "Wi-Fi", "Televisión", "Sala independiente", "Mini nevera" }
        }
    };

    public IReadOnlyList<Servicio> Servicios { get; } = new List<Servicio>
    {
        new Servicio { Nombre = "Desayuno", Icono = "bi-cup-hot", Descripcion = "Opciones para comenzar bien el día, disponibles por un valor adicional por persona." },
        new Servicio { Nombre = "Wi-Fi", Icono = "bi-wifi", Descripcion = "Conexión a internet en habitaciones y zonas comunes." },
        new Servicio { Nombre = "Piscina", Icono = "bi-water", Descripcion = "Un espacio para relajarte y disfrutar del clima de Guatapé." },
        new Servicio { Nombre = "Parqueadero", Icono = "bi-car-front", Descripcion = "Zona de estacionamiento para los huéspedes." },
        new Servicio { Nombre = "Recepción", Icono = "bi-bell", Descripcion = "Atención personalizada durante toda tu estadía." },
        new Servicio { Nombre = "Guía de planes", Icono = "bi-compass", Descripcion = "Recomendaciones para conocer la Piedra del Peñol, el malecón y el pueblo de zócalos." }
    };

    public HotelService()
    {
        SembrarReservas();
    }

    public Habitacion? ObtenerHabitacion(int id) => Habitaciones.FirstOrDefault(h => h.Id == id);

    public static bool EsTemporadaAlta(DateTime fecha)
    {
        var f = fecha.Date;
        return (f.Month == 6 && f.Day >= 15) || (f.Month == 7 && f.Day <= 15)
            || (f.Month == 12 && f.Day >= 15) || (f.Month == 1 && f.Day <= 15);
    }

    public Cotizacion Cotizar(Habitacion habitacion, DateTime entrada, DateTime salida, int personas, bool desayuno)
    {
        int noches = (salida.Date - entrada.Date).Days;
        decimal alojamiento = 0;

        for (var dia = entrada.Date; dia < salida.Date; dia = dia.AddDays(1))
        {
            alojamiento += EsTemporadaAlta(dia) ? habitacion.PrecioTemporadaAlta : habitacion.PrecioTemporadaBaja;
        }

        int adicionales = Math.Max(0, personas - habitacion.CapacidadBase);

        return new Cotizacion
        {
            Noches = noches,
            Alojamiento = alojamiento,
            PersonasAdicionales = adicionales * TarifaPersonaAdicional * noches,
            Desayuno = desayuno ? personas * TarifaDesayuno * noches : 0
        };
    }

    public int Disponibles(int habitacionId, DateTime entrada, DateTime salida)
    {
        var habitacion = ObtenerHabitacion(habitacionId);
        if (habitacion == null) return 0;

        lock (_candado)
        {
            return Math.Max(0, habitacion.Unidades - Ocupadas(habitacionId, entrada, salida));
        }
    }

    public Reserva? Registrar(Reserva reserva)
    {
        var habitacion = ObtenerHabitacion(reserva.HabitacionId);
        if (habitacion == null) return null;

        lock (_candado)
        {
            if (Ocupadas(reserva.HabitacionId, reserva.FechaEntrada, reserva.FechaSalida) >= habitacion.Unidades)
            {
                return null;
            }

            reserva.Codigo = "BD-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            reserva.FechaRegistro = DateTime.Now;
            reserva.Total = Cotizar(habitacion, reserva.FechaEntrada, reserva.FechaSalida,
                                    reserva.Personas, reserva.IncluyeDesayuno).Total;
            _reservas.Add(reserva);
            return reserva;
        }
    }

    public Reserva? ObtenerReserva(string codigo)
    {
        lock (_candado)
        {
            return _reservas.FirstOrDefault(r => r.Codigo == codigo);
        }
    }

    // Debe llamarse dentro del candado.
    private int Ocupadas(int habitacionId, DateTime entrada, DateTime salida) =>
        _reservas.Count(r => r.HabitacionId == habitacionId
                          && r.FechaEntrada.Date < salida.Date
                          && r.FechaSalida.Date > entrada.Date);

    // Reservas de ejemplo para que la consulta de disponibilidad muestre casos reales.
    private void SembrarReservas()
    {
        var hoy = DateTime.Today;
        int n = 0;

        void Agregar(int habitacionId, int desdeDias, int hastaDias)
        {
            n++;
            _reservas.Add(new Reserva
            {
                Codigo = $"BD-DEMO{n}",
                Nombre = "Huésped de ejemplo",
                HabitacionId = habitacionId,
                FechaEntrada = hoy.AddDays(desdeDias),
                FechaSalida = hoy.AddDays(hastaDias),
                Personas = 2
            });
        }

        // Suite familiar agotada y superior casi llena en una ventana próxima.
        Agregar(3, 5, 9);
        Agregar(3, 6, 10);
        Agregar(2, 5, 9);
        Agregar(2, 6, 9);
        Agregar(2, 7, 10);
    }
}
