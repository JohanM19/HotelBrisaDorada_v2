using HotelBrisaDorada.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBrisaDorada.Controllers;

public class HabitacionesController : Controller
{
    private readonly HotelService _hotel;

    public HabitacionesController(HotelService hotel)
    {
        _hotel = hotel;
    }

    public IActionResult Index()
    {
        return View(_hotel.Habitaciones);
    }

    public IActionResult Detalles(int id)
    {
        var habitacion = _hotel.ObtenerHabitacion(id);
        if (habitacion == null)
        {
            return NotFound();
        }

        return View(habitacion);
    }

    public IActionResult Tarifas()
    {
        return View(_hotel.Habitaciones);
    }
}
