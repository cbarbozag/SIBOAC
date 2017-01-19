(function () {
    $(document).ready(function () {
        prepararCampos();
        cargarDatosDelegacion();
        cargarDatosAutoridad();
        prepararEventos();
    });

    function prepararCampos() {
        $('.input-daterange').datepicker({
            startView: 2
        });
    };


    function cargarDatosDelegacion() {
        $.ajax({
            type: 'GET',
            url: 'api/Delegacions',
            //data: {},
            success: function (results) {
                var listD = $('#listaDelegaciones');
                for (var i = 0; i < results.length; i++) {
                    listD.append('<li data-id=' + results[i].Id + ' class="list-group-item">' + results[i].Descripcion + '</li>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };


    function cargarDatosAutoridad() {
        $.ajax({
            type: 'GET',
            url: 'api/AutoridadJudicial',
            //data: {},
            success: function (results) {
                var listA = $('#listaAutoridades');
                for (var i = 0; i < results.length; i++) {
                    listA.append('<li data-id=' + results[i].Id + ' class="list-group-item">' + results[i].Descripcion + '</li>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };


    function prepararEventos() {
        $('#generar').click(generarReporte);
        $('#todosDelegacion').click(marcarTodosDelegacion);
        $('#limpiarDelegacion').click(limpiarTodosDelegacion);
        $('#todosAutoridad').click(marcarTodosAutoridad);
        $('#limpiarAutoridad').click(limpiarTodosAutoridad);

    };


    function generarReporte() {
        var idDelegaciones = '';
        var idAutoridades = '';
        var idRadio = $('input[name="opcionConsulta"]:checked').val();
        var desde = $('#desde').val();
        var hasta = $('#hasta').val();
        var delegacionesSeleccionadas = $('#listaDelegaciones li.active');
        var autoridadesSeleccionadas = $('#listaAutoridades li.active');
        for (var i = 0; i < delegacionesSeleccionadas.length; i++) {
            idDelegaciones += 'idDelegaciones=' + $(delegacionesSeleccionadas[i]).data('id') + '&';
        };

        for (var i = 0; i < autoridadesSeleccionadas.length; i++) {
            idAutoridades += 'idAutoridades=' + $(autoridadesSeleccionadas[i]).data('id') + '&';
        };

        limpiar();

        $.ajax({
            type: 'GET',
            url: 'api/ReportePorDescargaDeParteOficial?idRadio=' + idRadio + '&' + idDelegaciones + idAutoridades + '&desde=' + desde + '&hasta=' + hasta,
            success: function (results) {
                var reporte = $('#reporte');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<tr><td>' + results[i].SerieParte + '</td>' +
                        '<td>' + results[i].NumeroParte + '</td>' +
                        '<td>' + results[i].FechaDescarga + '</td>' +
                        '<td>' + results[i].FechaEntrega + '</td>' +
                        '<td>' + results[i].FechaAccidente + '</td>' +
                        '<td>' + results[i].SerieNumeroBoleta + '</td>' +
                        '<td>' + results[i].Autoridad + '</td>' +
                        '<td>' + results[i].Delegacion + '</td>' +
                        '<td>' + results[i].InfoPlaca + '</td>' +
                        '<td>' + results[i].StatusPlano + '</td>' +
                        '<td>' + results[i].PlacaConfiscada + '</td>' +
                        '<td>' + results[i].VehDetenido + '</td></tr>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };



    function marcarTodosDelegacion() {
        var items = $('#listaDelegaciones li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            if (!$(items[i]).hasClass('active')) {
                $(items[i]).addClass('active')
            }
        }
    };

    function limpiarTodosDelegacion() {
        var items = $('#listaDelegaciones li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            $(items[i]).removeClass('active');
        }
    };



    function marcarTodosAutoridad() {
        var items = $('#listaAutoridades li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            if (!$(items[i]).hasClass('active')) {
                $(items[i]).addClass('active')
            }
        }
    };

    function limpiarTodosAutoridad() {
        var items = $('#listaAutoridades li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            $(items[i]).removeClass('active');
        }
    };





    //Para limpiar el reporte
    function limpiar() {
        var reporte = $('#reporte');
        reporte.empty();
    };



})();