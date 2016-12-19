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
       
    };

})();