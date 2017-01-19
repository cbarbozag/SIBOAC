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
            url: 'api/SIBOACUsuarios',
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
        $('#todos').click(marcarTodos);
        $('#limpiar').click(limpiarTodos);
    };

    function generarReporte() {
        var idUsuarios = '';
        var desde = $('#desde').val();
        var hasta = $('#hasta').val();
        var usuariosSeleccionados = $('li.active');
        for (var i = 0; i < usuariosSeleccionados.length; i++) {
            idUsuarios += 'idUsuarios=' + $(usuariosSeleccionados[i]).data('id') + '&';
        };

        limpiar();

        $.ajax({
            type: 'GET',
            url: 'api/ReportesPorUsuario?' + idUsuarios + 'desde='+ desde +'&hasta='+ hasta,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<tr><td>' + results[i].Usuario + '</td>' +
                        '<td>' + results[i].Autoridad + '</td>' +
                        '<td>' + results[i].FechaAccidente + '</td>' +
                        '<td>' + results[i].Serie + '</td>' +
                        '<td>' + results[i].NumeroParte + '</td>' +
                        '<td>' + results[i].Boletas + '</td>' +
                        '<td>' + results[i].FechaDescarga + '</td>' +
                        '<td>' + results[i].ClasePlaca + '</td>' +
                        '<td>' + results[i].CodigoPlaca + '</td>' +
                        '<td>' + results[i].NumeroPlaca + '</td>' +
                        '<td>' + results[i].EstadoPlano + '</td></tr>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };

    function marcarTodos() {
        var items = $('li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            if (!$(items[i]).hasClass('active')) {
                $(items[i]).addClass('active')
            }
        }
    };

    function limpiarTodos() {
        var items = $('li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            $(items[i]).removeClass('active');
        }
        limpiar();
    };

    function limpiar() {
        var reporte = $('#reporte');
        reporte.empty();
    };
})();