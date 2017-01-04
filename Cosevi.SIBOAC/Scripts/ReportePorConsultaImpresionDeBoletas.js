
(function () {
    $(document).ready(function () {
        prepararCampos();
        cargarDatosDelegacion();
        cargarDatosInspector();
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
            url: '/api/Delegacions',
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



    function cargarDatosInspector() {
        $.ajax({
            type: 'GET',
            url: '/api/Inspectors',
            //data: {},
            success: function (results) {
                var listI = $('#listaInspectores');
                for (var i = 0; i < results.length; i++) {
                    if (results[i].Id != null && results[i].Id.trim().length>0)
                        listI.append('<li data-id=' + results[i].Id + ' class="list-group-item">' + results[i].Id + '</li>');
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
        $('#todosInspector').click(marcarTodosInspector);
        $('#limpiarInspector').click(limpiarTodosInspector);
        $('#ExportaExcel').click(descargarExcel);
        $('#ExportarPDF').click(descargarPDF);
    };


    function generarReporte() {
        var idDelegaciones = '';
        var idInspectores = '';
        var desde = $('#desde').val();
        var hasta = $('#hasta').val();
        var delegacionesSeleccionadas = $('#listaDelegaciones li.active');
        var inspectoresSeleccionados = $('#listaInspectores li.active');
        for (var i = 0; i < delegacionesSeleccionadas.length; i++) {
            idDelegaciones += 'idDelegaciones=' + $(delegacionesSeleccionadas[i]).data('id') + '&';
        };

        for (var i = 0; i < inspectoresSeleccionados.length; i++) {
            idInspectores += 'idInspectores=' + $(inspectoresSeleccionados[i]).data('id') + '&';
        };
        
        limpiar();

        $.ajax({
            type: 'GET',
            url: '/api/ReportePorConsultaImpresionDeBoletas?' + idDelegaciones + idInspectores + 'desde=' + desde + '&hasta=' + hasta,
            success: function (results) {
                var reporte = $('#TablaContenido');
                for (var i = 0; i < results.length; i++) {
                    reporte.append('<tr><td>' + results[i].DescripcionDelegacion + '</td>' +
                        '<td>' + results[i].CodigoInspector + '</td>' +
                        '<td>' + results[i].Serie + '</td>'+
                        '<td>' + results[i].Boletas + '</td>' +
                        '<td>' + results[i].FechaInfraccion + '</td>' +
                        '<td>' + results[i].FechaDescarga + '</td>' +
                        '<td>' + results[i].CodigoArticulo + '</td>' +
                        '<td>' + results[i].Provincia + '</td>' +
                        '<td>' + results[i].CoordenadaX + '</td>' +
                        '<td>' + results[i].CoordenadaY + '</td></tr>');
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



    function marcarTodosInspector() {
        var items = $('#listaInspectores li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            if (!$(items[i]).hasClass('active')) {
                $(items[i]).addClass('active')
            }
        }
    };

    function limpiarTodosInspector() {
        var items = $('#listaInspectores li.list-group-item');
        for (var i = 0; i < items.length; i++) {
            $(items[i]).removeClass('active');
        }
    };

    //Para limpiar el reporte
    function limpiar() {
        var reporte = $('#reporte');
        reporte.empty();
    };

    function descargarExcel() {
        alert("Hola");
        $("#TablaContenido").tableExport({
            bootstrap: false
        });
    }

    function descargarPDF() {
        $.ajax({
            type: 'GET',
            url: '/api/ReportePorConsultaImpresionDeBoletas',
            //data: {},
            success: function (results) {
                alert("Hola");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };

})();
    