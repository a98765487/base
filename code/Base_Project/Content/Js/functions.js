function showLoading() {
    $(".loading-area").addClass("d-flex");
}
function hideLoading() {
    $(".loading-area").removeClass("d-flex");
}
function showError(ex) {
    $(".error-utility").empty();
    if (typeof ex == 'object') {
        ex.forEach(function (error) {
            $(".error-utility").append("<p>" + error.ErrorText + "<p>");
        });
    }
    else {
        $(".error-utility").empty();
        $(".error-utility").append(ex);
    }
}