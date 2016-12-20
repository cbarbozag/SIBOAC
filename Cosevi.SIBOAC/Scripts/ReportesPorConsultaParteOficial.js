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

        $.ajax({
            type: 'GET',
            url: '/api/ReportePorConsultaParteOficial?serieParte=' + serieParte + '&numeroParte=' + numeroParte + '&serieBoleta=' + serieBoleta + '&numeroBoleta=' + numeroBoleta +
                '&tipoId=' + tipoId + '&numeroID=' + numeroID + '&numeroPlaca=' + numeroPlaca + '&codigoPlaca=' + codigoPlaca + '&clasePlaca=' + clasePlaca,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<div class="row"><div class="col-md-1">' + results[i].SerieBoleta + '</div><div class="col-md-1">' + results[i].NumeroBoleta + '</div><div class="col-md-1">' + results[i].FechaAccidente + '</div><div class="col-md-1">' + results[i].Autoridad + '</div><div class="col-md-1">' + results[i].Delegacion + '</div><div class="col-md-1">' + results[i].ClasePlaca + '</div><div class="col-md-1">' + results[i].CodigoNumeroPlaca + '</div><div class="col-md-1">' + results[i].Identificacion + '</div><div class="col-md-1">' + results[i].Nombre + '</div><div class="col-md-1">' + results[i].Rol + '</div><div class="col-md-1">' + results[i].SerieParte + '</div><div class="col-md-1">' + results[i].NumeroParte + '</div></div>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };

})();