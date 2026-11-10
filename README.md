# Hotel Brisa Dorada

Aplicación web desarrollada con **ASP.NET Core MVC** y **C#** para el Seguimiento 3 de Programación Web.
Es un hotel ficticio ubicado en Guatapé, Antioquia, donde se pueden consultar habitaciones, tarifas y disponibilidad, y registrar reservas.

**Aplicación publicada:** _(https://hotel-brisa-dorada.onrender.com/Reservas/Confirmacion?codigo=BD-C10D6C7F)_

## Funcionalidades

- Páginas de inicio, habitaciones (con detalle), servicios, tarifas y contacto.
- Consulta de disponibilidad por fechas y número de personas.
- Formulario de reserva con validaciones, cálculo del valor según temporada y código de confirmación.
- Formulario de contacto con validaciones.

## Estructura del proyecto

| Carpeta | Responsabilidad |
|---|---|
| `Models/` | Entidades y modelos de vista (`Habitacion`, `Reserva`, `Servicio`, `Cotizacion`, etc.) |
| `Views/` | Vistas Razor (`.cshtml`) con HTML y Bootstrap |
| `Controllers/` | Controladores y acciones GET y POST (`Home`, `Habitaciones`, `Reservas`) |
| `Services/` | Lógica de negocio: disponibilidad, tarifas y registro de reservas |
| `Helpers/` | Formato de precios y fechas |
| `wwwroot/` | Archivos estáticos: CSS, JavaScript y librerías |

## Cómo ejecutarlo

Requiere el SDK de .NET 10.

```bash
cd HotelBrisaDorada
dotnet run
```

Luego abrir `http://localhost:5265`.

## Notas

- Las reservas se guardan en memoria y se pierden al reiniciar la aplicación.
- Las tarifas y los datos de contacto son ficticios.
