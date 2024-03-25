// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Active item option selected sidebar
$(document).ready(function () {
    var currentUrl = window.location.href;
    $('.nav-item a').each(function () {
        var linkUrl = this.href;
        if (currentUrl == linkUrl) {
            $(this).closest('.nav-item').addClass('active');
        }
    });
});

//Add toggled into class sidebar when click
//$(document).ready(function () {
//    var currentUrl = window.location.href;
//    $('.navbar-nav .nav-item a').each(function () {
//        var linkUrl = this.href;
//        if (currentUrl == linkUrl) {
//            $(this).closest('.nav-item').addClass('toggled');
//        }
//    });
//});