function initializeMenuShowClickEvent() {
    $('[data-toggle=offcanvas]').click(function () {
        $('.row-offcanvas').toggleClass('active');
    });
};

function initializeMenuOptionSelectEvent() {
    $(".nav a").on("click", function () {
        $(".nav").find(".active").removeClass("active");
        $(this).parent().addClass("active");
    });
};

function markSelectedMenuOption() {
    var location = window.location.pathname;
    $('.nav-stacked').find('a').each(function () {
        $(this).parent().toggleClass('active', $(this).attr('href') == location);
    });
};

function initializeMenuSearchEvents() {
    var menusearch = $('#menusearch');
    var menuoptions = $('#menuoptions');
    menusearch.on('keyup change', function () {
        menuoptions.children('li').each(function () {
            $(this).toggleClass('hide', $(this).children('a').text().toLowerCase().indexOf(menusearch.val().toLowerCase()) < 0);
        });
    });
};

function initializeSorting() {
    $('#menuascendantorder').click(function () {
        sortUnorderedList('menuoptions', true);
    });
    $('#menudescendantorder').click(function () {
        sortUnorderedList('menuoptions', false);
    });
};

function sortUnorderedList(ul, ascendant) {
    var ul = document.getElementById(ul);
    var lis = ul.getElementsByTagName("li");
    var vals = [];
    for (var i = 0, l = lis.length; i < l; i++) {
        vals.push(lis[i].innerHTML);
    };        
    vals.sort();
    if (!ascendant) {
        vals.reverse();
    };        
    for (var i = 0, l = lis.length; i < l; i++) {
        lis[i].innerHTML = vals[i];
    };        
}

$(document).ready(function () {
    initializeMenuShowClickEvent();
    initializeMenuOptionSelectEvent();
    initializeMenuSearchEvents();
    markSelectedMenuOption();
    initializeSorting();
});