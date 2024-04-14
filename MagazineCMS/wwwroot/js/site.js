
//Active item option selected sidebar
$(document).ready(function () {
    var currentUrl = window.location.href;
    $('.nav-item a').each(function () {
        var linkUrl = this.href;
        if (currentUrl == linkUrl) {
            $(this).closest('.nav-item').addClass('active');
        }
    })
});