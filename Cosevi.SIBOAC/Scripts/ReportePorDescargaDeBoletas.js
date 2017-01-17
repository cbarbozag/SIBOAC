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
                var reporte = $('#TablaContenido');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<tr><td>' + results[i].Serie + '</td>' +
                        '<td>' + results[i].Boletas + '</td>' +
                        '<td>' + results[i].FechaDescarga + '</td>' +
                        '<td>' + results[i].FechaAccidente + '</td>' +
                        '<td>' + results[i].CodigoAutoridad + '</td>' +
                        '<td>' + results[i].CodigoDelegacion + '</td>' +
                        '<td>' + results[i].InfoPlaca + '</td>' +
                        '<td>' + results[i].Identificacion + '</td>' +
                        '<td>' + results[i].Nombre + '</td>' +
                        '<td>' + results[i].CodRol + '</td>' +
                        '<td>' + results[i].SerieNumParteOficial + '</td></tr>');
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