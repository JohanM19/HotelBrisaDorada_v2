using System.ComponentModel.DataAnnotations;

namespace HotelBrisaDorada.Models;

public class ContactoViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El asunto es obligatorio.")]
    [StringLength(80, ErrorMessage = "Máximo 80 caracteres.")]
    [Display(Name = "Asunto")]
    public string Asunto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe tu mensaje.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "El mensaje debe tener entre 10 y 1000 caracteres.")]
    [Display(Name = "Mensaje")]
    public string Mensaje { get; set; } = string.Empty;
}
