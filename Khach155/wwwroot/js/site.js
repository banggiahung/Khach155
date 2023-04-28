// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var elementPosition = $('#nav-menu').offset();

$(window).scroll(function () {
    if ($(window).scrollTop() > elementPosition.top) {
        $('#nav-menu').css('position', 'fixed').css('top', '0').css('z-index', '9').css('width', '100%');
    } else {
        $('#nav-menu').css('position', 'static');
    }
});