(function () {
    $(document).ready(function () {
        prepararCampos();
        prepararEventos();
    });

    function prepararCampos() {
        $('.input-daterange').datepicker({
            startView: 2
        });
    };


    function prepararEventos() {
        $('#generar').click(generarReporte);

    };


    function generarReporte() {
        var idRadioButton = $('input[name="radio"]:checked').val();
        var desde = $('#desde').val();
        var hasta = $('#hasta').val();

        limpiar();

        $.ajax({
            type: 'GET',
            url: '/api/ReportePorDescargaDeBoletas?' + idRadioButton + 'desde=' + desde + '&hasta=' + hasta,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<div class="row"><div class="col-md-2">' + results[i].Serie + '</div><div class="col-md-1">' + results[i].Boletas + '</div><div class="col-md-1">' + results[i].FechaAccidente + '</div><div class="col-md-1">' + results[i].Autoridad + '</div><div class="col-md-1">' + results[i].Delegacion + '</div><div class="col-md-1">' + results[i].ClasePlaca + '</div><div class="col-md-1">' + results[i].CodigoPlaca + '</div><div class="col-md-1">' + results[i].NumeroPlaca);
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };

    //Para limpiar el reporte
    function limpiar() {
        var reporte = $('#reporte');
        reporte.empty();
    };
})();