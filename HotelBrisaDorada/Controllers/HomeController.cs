using System.Diagnostics;
using HotelBrisaDorada.Models;
using HotelBrisaDorada.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBrisaDorada.Controllers;

public class HomeController : Controller
{
    private readonly HotelService _hotel;

    public HomeController(HotelService hotel)
    {
        _hotel = hotel;
    }

    public IActionResult Index()
    {
        var modelo = new HomeViewModel
        {
            Habitaciones = _hotel.Habitaciones,
            Servicios = _hotel.Servicios
        };
        return View(modelo);
    }

    public IActionResult Servicios()
    {
        return View(_hotel.Servicios);
    }

    [HttpGet]
    public IActionResult Contacto()
    {
        return View(new ContactoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contacto(ContactoViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        TempData["Mensaje"] =
            $"Gracias, {modelo.Nombre}. Recibimos tu mensaje y te responderemos al correo {modelo.Correo}.";

        return RedirectToAction(nameof(Contacto));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult NoEncontrado()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
