// Side menu slide event
$(document).ready(function () {
    $('[data-toggle=offcanvas]').click(function () {
        $('.row-offcanvas').toggleClass('active');
    });

    var loc = window.location.pathname;
    $('.nav-stacked').find('a').each(function () {
        $(this).parent().toggleClass('active', $(this).attr('href') == loc);
    });
});

// Side menu click event
$(".nav a").on("click", function () {
    $(".nav").find(".active").removeClass("active");
    $(this).parent().addClass("active");
});