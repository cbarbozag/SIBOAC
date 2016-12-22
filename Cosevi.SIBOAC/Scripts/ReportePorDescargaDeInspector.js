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
        var numeroHH = $('#numeroHH').val();
        var codigoInspector = $('#codigoInspector').val();
        var desde = $('#desde').val();
        var hasta = $('#hasta').val();

        limpiar();

        $.ajax({
            type: 'GET',
            url: '/api/ReportePorDescargaDeInspector?numeroHH=' + numeroHH + '&codigoInspector=' + codigoInspector + '&desde=' + desde + '&hasta=' + hasta,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<div class="row"><div class="col-md-1">' + results[i].SerieBoleta + '</div><div class="col-md-1">' + results[i].NumeroBoleta + '</div><div class="col-md-1">' + results[i].CodigoInspector + '</div><div class="col-md-1">' + results[i].NombreInspector + '</div><div class="col-md-1">' + results[i].FechaDescarga + '</div><div class="col-md-1">' + results[i].FechaAccidente + '</div><div class="col-md-1">' + results[i].Autoridad + '</div><div class="col-md-1">' + results[i].Delegacion + '</div><div class="col-md-1">' + results[i].ClasePlaca + '</div><div class="col-md-1">' + results[i].CodigoNumeroPlaca + '</div><div class="col-md-1">' + results[i].NumeroHH + '</div><div class="col-md-1">' + results[i].SerieNumParte + '</div></div>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };

    function limpiar() {
        var reporte = $('#reporte');
        reporte.empty();
    };
})();