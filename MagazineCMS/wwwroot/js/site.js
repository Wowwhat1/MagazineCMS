
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

//Active item option selected sidebar
$(document).ready(function () {
    var currentUrl = window.location.href;
    $('.nav-item a').each(function () {
        var linkUrl = this.href;
        if (currentUrl == linkUrl) {
            $(this).closest('.nav-item').addClass('active');
        }
});