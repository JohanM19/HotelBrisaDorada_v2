// Hotel Brisa Dorada - comportamiento del lado del cliente
(function () {
    "use strict";

    function dosDigitos(n) {
        return n < 10 ? "0" + n : String(n);
    }

    function formatoFecha(fecha) {
        return fecha.getFullYear() + "-" + dosDigitos(fecha.getMonth() + 1) + "-" + dosDigitos(fecha.getDate());
    }

    // La fecha de salida siempre debe ser posterior a la de llegada.
    document.querySelectorAll("[data-fecha-entrada]").forEach(function (entrada) {
        var formulario = entrada.closest("form");
        var salida = formulario ? formulario.querySelector("[data-fecha-salida]") : null;
        if (!salida) return;

        function ajustar() {
            if (!entrada.value) return;

            var minimo = new Date(entrada.value + "T00:00:00");
            minimo.setDate(minimo.getDate() + 1);
            var minimoTexto = formatoFecha(minimo);

            salida.min = minimoTexto;
            if (!salida.value || salida.value < minimoTexto) {
                salida.value = minimoTexto;
            }
        }

        entrada.addEventListener("change", ajustar);
        ajustar();
    });
})();
