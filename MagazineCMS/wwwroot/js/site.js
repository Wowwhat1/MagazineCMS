// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.querySelector('.navbar_menu').addEventListener('mouseover', function () {
    this.style.left = '0';
});

document.querySelector('.navbar_menu').addEventListener('mouseout', function () {
    this.style.left = '-12%';
});