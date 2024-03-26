// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*----------------Terms and Conditions------------------*/

document.getElementById("registerButton").addEventListener("click", function () {
    var checkbox = document.getElementById("termsCheckbox");
    var errorMessage = document.getElementById("errorMessage");
    if (!checkbox.checked) {
        errorMessage.style.display = "block";
        return false; 
    }
    errorMessage.style.display = "none";

});