(function () {
    $(document).ready(function () {
        prepararCampos();
        cargarDatos();
        prepararEventos();
    });

    function prepararCampos() {
        $('.input-daterange').datepicker({});
    };

    function cargarDatos() {
        $.ajax({
            type: 'GET',
            url: '/api/SIBOACUsuarios',
            //data: {},
            success: function (results) {
                var list = $('#list');
                for (var i = 0; i < results.length; i++) {
                    list.append('<li class="list-group-item">' + results[i].Usuario + ' - ' + results[i].Nombre + '</li>');
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
        $.ajax({
            type: 'GET',
            url: '/api/SIBOACUsuarios',
            //data: {},
            success: function (results) {
                var list = $('#list');
                for (var i = 0; i < results.length; i++) {
                    list.append('<li class="list-group-item">' + results[i].Usuario + ' - ' + results[i].Nombre + '</li>');
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
            }
        });
    };
})();