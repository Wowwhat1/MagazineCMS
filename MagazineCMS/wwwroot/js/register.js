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