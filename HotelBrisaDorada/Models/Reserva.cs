using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HotelBrisaDorada.Models;

public class Reserva
{
    // Los asigna el sistema; no se aceptan desde el formulario.
    [BindNever]
    [ValidateNever]
    public string Codigo { get; set; } = string.Empty;

    [BindNever]
    [ValidateNever]
    public decimal Total { get; set; }

    [BindNever]
    [ValidateNever]
    public DateTime FechaRegistro { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El documento es obligatorio.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "El documento debe tener entre 5 y 20 caracteres.")]
    [Display(Name = "Documento de identidad")]
    public string Documento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Ingresa un teléfono válido.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un tipo de habitación.")]
    [Display(Name = "Tipo de habitación")]
    public int HabitacionId { get; set; }

    [Required(ErrorMessage = "La fecha de llegada es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de llegada")]
    public DateTime FechaEntrada { get; set; } = DateTime.Today.AddDays(1);

    [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de salida")]
    public DateTime FechaSalida { get; set; } = DateTime.Today.AddDays(2);

    [Range(1, 5, ErrorMessage = "El número de personas debe estar entre 1 y 5.")]
    [Display(Name = "Número de personas")]
    public int Personas { get; set; } = 2;

    [Display(Name = "Incluir desayuno")]
    public bool IncluyeDesayuno { get; set; }

    [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
    [Display(Name = "Comentarios adicionales")]
    public string? Comentarios { get; set; }

    [Display(Name = "Acepto el tratamiento de mis datos")]
    public bool AceptaTerminos { get; set; }
}
