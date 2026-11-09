using System.Globalization;

namespace HotelBrisaDorada.Helpers;

public static class Formato
{
    private static readonly string[] Meses =
    {
        "enero", "febrero", "marzo", "abril", "mayo", "junio",
        "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"
    };

    // 180000 -> $180.000 (sin depender de la cultura del servidor)
    public static string Cop(this decimal valor) =>
        "$" + valor.ToString("N0", CultureInfo.InvariantCulture).Replace(",", ".");

    // 12 de noviembre de 2026
    public static string Fecha(this DateTime fecha) =>
        $"{fecha.Day} de {Meses[fecha.Month - 1]} de {fecha.Year}";

    public static string Plural(int cantidad, string singular, string plural) =>
        cantidad == 1 ? $"{cantidad} {singular}" : $"{cantidad} {plural}";
}
