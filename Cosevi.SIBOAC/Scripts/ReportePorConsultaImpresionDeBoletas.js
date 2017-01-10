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
       // $('#ExportarPDF').click(descargarPDF);
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

   
        var tablesToExcel = (function () {
            var uri = 'data:application/vnd.ms-excel;base64,'
            , tmplWorkbookXML = '<?xml version="1.0"?><?mso-application progid="Excel.Sheet"?><Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">'
              + '<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office"><Author>Axel Richter</Author><Created>{created}</Created></DocumentProperties>'
              + '<Styles>'
              + '<Style ss:ID="Currency"><NumberFormat ss:Format="Currency"></NumberFormat></Style>'
              + '<Style ss:ID="Date"><NumberFormat ss:Format="Medium Date"></NumberFormat></Style>'
              + '</Styles>'
              + '{worksheets}</Workbook>'
            , tmplWorksheetXML = '<Worksheet ss:Name="{nameWS}"><Table>{rows}</Table></Worksheet>'
            , tmplCellXML = '<Cell{attributeStyleID}{attributeFormula}><Data ss:Type="{nameType}">{data}</Data></Cell>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
            return function (tables, wsnames, wbname, appname) {
                var ctx = "";
                var workbookXML = "";
                var worksheetsXML = "";
                var rowsXML = "";

                for (var i = 0; i < tables.length; i++) {
                    if (!tables[i].nodeType) tables[i] = document.getElementById(tables[i]);
                    for (var j = 0; j < tables[i].rows.length; j++) {
                        rowsXML += '<Row>'
                        for (var k = 0; k < tables[i].rows[j].cells.length; k++) {
                            var dataType = tables[i].rows[j].cells[k].getAttribute("data-type");
                            var dataStyle = tables[i].rows[j].cells[k].getAttribute("data-style");
                            var dataValue = tables[i].rows[j].cells[k].getAttribute("data-value");
                            dataValue = (dataValue) ? dataValue : tables[i].rows[j].cells[k].innerHTML;
                            var dataFormula = tables[i].rows[j].cells[k].getAttribute("data-formula");
                            dataFormula = (dataFormula) ? dataFormula : (appname == 'Calc' && dataType == 'DateTime') ? dataValue : null;
                            ctx = {
                                attributeStyleID: (dataStyle == 'Currency' || dataStyle == 'Date') ? ' ss:StyleID="' + dataStyle + '"' : ''
                                   , nameType: (dataType == 'Number' || dataType == 'DateTime' || dataType == 'Boolean' || dataType == 'Error') ? dataType : 'String'
                                   , data: (dataFormula) ? '' : dataValue
                                   , attributeFormula: (dataFormula) ? ' ss:Formula="' + dataFormula + '"' : ''
                            };
                            rowsXML += format(tmplCellXML, ctx);
                        }
                        rowsXML += '</Row>'
                    }
                    ctx = { rows: rowsXML, nameWS: wsnames[i] || 'Sheet' + i };
                    worksheetsXML += format(tmplWorksheetXML, ctx);
                    rowsXML = "";
                }

                ctx = { created: (new Date()).getTime(), worksheets: worksheetsXML };
                workbookXML = format(tmplWorkbookXML, ctx);

                console.log(workbookXML);

                var link = document.createElement("A");
                link.href = uri + base64(workbookXML);
                link.download = wbname || 'Workbook.xls';
                link.target = '_blank';
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        })();
  


})();
    