using HotelBrisaDorada.Models;
using HotelBrisaDorada.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBrisaDorada.Controllers
{
    public class ReservasController : Controller
    {
        private readonly HotelService _hotel;

        public ReservasController(HotelService hotel)
        {
            _hotel = hotel;
        }

        // ============================
        // CONSULTAR DISPONIBILIDAD
        // ============================

        [HttpGet]
        public IActionResult Disponibilidad(DateTime? fechaEntrada, DateTime? fechaSalida, int personas = 2)
        {
            var modelo = new DisponibilidadViewModel
            {
                FechaEntrada = fechaEntrada,
                FechaSalida = fechaSalida,
                Personas = personas
            };

            if (!fechaEntrada.HasValue && !fechaSalida.HasValue)
            {
                return View(modelo);
            }

            if (!fechaEntrada.HasValue || !fechaSalida.HasValue)
            {
                modelo.Error = "Debes seleccionar ambas fechas.";
                return View(modelo);
            }

            var entrada = fechaEntrada.Value.Date;
            var salida = fechaSalida.Value.Date;

            if (entrada < DateTime.Today)
            {
                modelo.Error = "La fecha de llegada no puede ser anterior a hoy.";
                return View(modelo);
            }

            if (salida <= entrada)
            {
                modelo.Error = "La fecha de salida debe ser posterior a la de llegada.";
                return View(modelo);
            }

            if (personas < 1 || personas > 5)
            {
                modelo.Error = "El número de personas debe estar entre 1 y 5.";
                return View(modelo);
            }

            foreach (var habitacion in _hotel.Habitaciones)
            {
                bool cabe = personas <= habitacion.CapacidadMaxima;

                modelo.Resultados.Add(new ResultadoDisponibilidad
                {
                    Habitacion = habitacion,
                    CabePersonas = cabe,
                    Disponibles = _hotel.Disponibles(habitacion.Id, entrada, salida),
                    Cotizacion = cabe ? _hotel.Cotizar(habitacion, entrada, salida, personas, false) : null
                });
            }

            modelo.Consultado = true;
            return View(modelo);
        }

        // ============================
        // RESERVAR ESTADÍA
        // ============================

        [HttpGet]
        public IActionResult Create(DateTime? fechaEntrada, DateTime? fechaSalida, int? personas, int? habitacionId)
        {
            var reserva = new Reserva();

            if (fechaEntrada.HasValue) reserva.FechaEntrada = fechaEntrada.Value.Date;
            if (fechaSalida.HasValue) reserva.FechaSalida = fechaSalida.Value.Date;
            if (personas.HasValue && personas.Value >= 1 && personas.Value <= 5) reserva.Personas = personas.Value;
            if (habitacionId.HasValue) reserva.HabitacionId = habitacionId.Value;

            ViewBag.Habitaciones = _hotel.Habitaciones;
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reserva reserva)
        {
            var habitacion = _hotel.ObtenerHabitacion(reserva.HabitacionId);

            if (reserva.FechaEntrada.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(Reserva.FechaEntrada),
                    "La fecha de llegada no puede ser anterior a hoy.");
            }

            if (reserva.FechaSalida.Date <= reserva.FechaEntrada.Date)
            {
                ModelState.AddModelError(nameof(Reserva.FechaSalida),
                    "La fecha de salida debe ser posterior a la de llegada.");
            }

            if (habitacion != null && reserva.Personas > habitacion.CapacidadMaxima)
            {
                ModelState.AddModelError(nameof(Reserva.Personas),
                    $"La {habitacion.Nombre.ToLower()} admite máximo {habitacion.CapacidadMaxima} personas.");
            }

            if (!reserva.AceptaTerminos)
            {
                ModelState.AddModelError(nameof(Reserva.AceptaTerminos),
                    "Debes aceptar el tratamiento de tus datos para continuar.");
            }

            if (ModelState.IsValid)
            {
                var registrada = _hotel.Registrar(reserva);

                if (registrada != null)
                {
                    return RedirectToAction(nameof(Confirmacion), new { codigo = registrada.Codigo });
                }

                ModelState.AddModelError(string.Empty,
                    "No hay habitaciones disponibles de ese tipo en las fechas elegidas. Prueba con otras fechas u otro tipo de habitación.");
            }

            ViewBag.Habitaciones = _hotel.Habitaciones;
            return View(reserva);
        }

        // ============================
        // CONFIRMACIÓN
        // ============================

        [HttpGet]
        public IActionResult Confirmacion(string codigo)
        {
            var reserva = _hotel.ObtenerReserva(codigo ?? string.Empty);
            var habitacion = reserva == null ? null : _hotel.ObtenerHabitacion(reserva.HabitacionId);

            if (reserva == null || habitacion == null)
            {
                return NotFound();
            }

            var modelo = new ConfirmacionViewModel
            {
                Reserva = reserva,
                Habitacion = habitacion,
                Cotizacion = _hotel.Cotizar(habitacion, reserva.FechaEntrada, reserva.FechaSalida,
                                            reserva.Personas, reserva.IncluyeDesayuno)
            };

            return View(modelo);
        }
    }
}
