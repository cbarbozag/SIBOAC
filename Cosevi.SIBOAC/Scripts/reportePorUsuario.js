(function () {
    $(document).ready(function () {
        prepararCampos();
        cargarDatos();
        prepararEventos();
    });

    function prepararCampos() {
        $('.input-daterange').datepicker({
            startView: 2
        });
    };

    function cargarDatos() {
        $.ajax({
            type: 'GET',
            url: '/api/SIBOACUsuarios',
            //data: {},
            success: function (results) {
                var list = $('#list');
                for (var i = 0; i < results.length; i++) {
                    list.append('<li data-id=' + results[i].Id + ' class="list-group-item">' + results[i].Usuario + ' - ' + results[i].Nombre + '</li>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }            
        });
    };

    function prepararEventos() {
        $('#generar').click(generarReporte);

    };

    function generarReporte() {
        var idUsuarios = '';
        var desde = $('#desde').val();
        var hasta = $('#hasta').val();
        var usuariosSeleccionados = $('li.active');
        for (var i = 0; i < usuariosSeleccionados.length; i++) {
            idUsuarios += 'idUsuarios=' + $(usuariosSeleccionados[i]).data('id') + '&';
        };

        

        $.ajax({
            type: 'GET',
            url: '/api/ReportesPorUsuario?' + idUsuarios + 'desde='+ desde +'&hasta='+ hasta,
            success: function (results) {
                var reporte = $('#reporte');                
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<div class="row"><div class="col-md-2">' + results[i].Usuario + '</div><div class="col-md-1">' + results[i].Autoridad + '</div><div class="col-md-1">' + results[i].FechaAccidente + '</div><div class="col-md-1">' + results[i].Serie + '</div><div class="col-md-1">' + results[i].NumeroParte + '</div><div class="col-md-1">' + results[i].Boletas + '</div><div class="col-md-1">' + results[i].FechaDescarga + '</div><div class="col-md-1">' + results[i].ClasePlaca + '</div><div class="col-md-1">' + results[i].CodigoPlaca + '</div><div class="col-md-1">' + results[i].NumeroPlaca + '</div><div class="col-md-1">' + results[i].EstadoPlano + '</div></div>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };
})();