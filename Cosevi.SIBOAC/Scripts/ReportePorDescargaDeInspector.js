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
            url: 'api/ReportePorDescargaDeInspector?numeroHH=' + numeroHH + '&codigoInspector=' + codigoInspector + '&desde=' + desde + '&hasta=' + hasta,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<tr><td>' + results[i].SerieBoleta + '</td>' +
                        '<td>' + results[i].NumeroBoleta + '</td>' +
                        '<td>' + results[i].CodigoInspector + '</td>' +
                        '<td>' + results[i].NombreInspector + '</td>' +
                        '<td>' + results[i].FechaDescarga + '</td>' +
                        '<td>' + results[i].FechaAccidente + '</td>' +
                        '<td>' + results[i].Autoridad + '</td>' +
                        '<td>' + results[i].Delegacion + '</td>' +
                        '<td>' + results[i].ClasePlaca + '</td>' +
                        '<td>' + results[i].CodigoNumeroPlaca + '</td>' +
                        '<td>' + results[i].NumeroHH + '</td>' +
                        '<td>' + results[i].SerieNumParte + '</td></tr>');
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