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
                var listA = $('#listaInspectores');
                for (var i = 0; i < results.length; i++) {
                    listA.append('<li data-id=' + results[i].Id + ' class="list-group-item">' + results[i].Descripcion + '</li>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };
