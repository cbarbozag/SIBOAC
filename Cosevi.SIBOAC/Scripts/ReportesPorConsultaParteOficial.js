(function () {
    $(document).ready(function () {
        prepararCampos();
        cargarDatos();
        prepararEventos();
    });

    function prepararCampos() {
        $(":input[type='radio']").on("change", function () {
            $('div[id^=opc-]').hide();
            if ($(this).prop("checked")) {
                var checkedRadio = '#opc-' + $(this).val();
                $(checkedRadio).show();
            }
        });
    };

    function cargarDatos() {
        
    };

    function prepararEventos() {
        $('#generar').click(generarReporte);
    };

    function generarReporte() {

        var serieParte = $('#serieParte').val();
        var numeroParte = $('#numeroParte').val();
        var serieBoleta = $('#serieBoleta').val();
        var numeroBoleta = $('#numeroBoleta').val();
        var tipoId = $('#tipoId').val();
        var numeroID = $('#numeroID').val(); 
        var numeroPlaca = $('#numeroPlaca').val();
        var codigoPlaca = $('#codigoPlaca').val();
        var clasePlaca = $('#clasePlaca').val();

        limpiar();

        $.ajax({
            type: 'GET',
            url: '/api/ReportePorConsultaParteOficial?serieParte=' + serieParte + '&numeroParte=' + numeroParte + '&serieBoleta=' + serieBoleta + '&numeroBoleta=' + numeroBoleta +
                '&tipoId=' + tipoId + '&numeroID=' + numeroID + '&numeroPlaca=' + numeroPlaca + '&codigoPlaca=' + codigoPlaca + '&clasePlaca=' + clasePlaca,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<tr><td>' + results[i].SerieBoleta + '</td>' +
                        '<td>' + results[i].NumeroBoleta + '</td>' +
                        '<td>' + results[i].FechaAccidente + '</td>' +
                        '<td>' + results[i].Autoridad + '</td>' +
                        '<td>' + results[i].Delegacion + '</td>' +
                        '<td>' + results[i].ClasePlaca + '</td>' +
                        '<td>' + results[i].CodigoNumeroPlaca + '</td>' +
                        '<td>' + results[i].Identificacion + '</td>' +
                        '<td>' + results[i].Nombre + '</td>' +
                        '<td>' + results[i].Rol + '</td>' +
                        '<td>' + results[i].SerieParte + '</td>' +
                        '<td>' + results[i].NumeroParte + '</td></tr>');
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