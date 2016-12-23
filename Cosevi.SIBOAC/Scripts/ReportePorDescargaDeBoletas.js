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
        var idRadioButton = $('input[name="opcionRadio"]:checked').val();
        var desde = $('#desde').val();
        var hasta = $('#hasta').val();

        limpiar();

        $.ajax({
            type: 'GET',
            url: '/api/ReportePorDescargaDeBoletas?idRadio=' + idRadioButton + '&' + '&desde=' + desde + '&hasta=' + hasta,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<div class="row"><div class="col-md-2">' + results[i].Serie + '</div><div class="col-md-1">' + results[i].Boletas + '</div><div class="col-md-1">' + results[i].FechaDescarga + '</div><div class="col-md-1">' + results[i].FechaAccidente + '</div><div class="col-md-1">' + results[i].CodigoAutoridad + '</div><div class="col-md-1">' + results[i].CodigoDelegacion + '</div><div class="col-md-1">' + results[i].InfoPlaca + '</div><div class="col-md-1">' + results[i].Identificacion + '</div><div class="col-md-1">' + results[i].Nombre + '</div><div class="col-md-1">' + results[i].CodRol + '</div><div class="col-md-1">' + results[i].SerieNumParteOficial +'</div><div class="col-md-1">' + results[i].DescripcionAutoridad + '</div><div class="col-md-1">' + results[i].DescripcionDelegacion);
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