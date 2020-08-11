document.getElementById('receipt-menu').addEventListener('click', function () {
    let element = document.getElementsByClassName('header-inner-nav')[0];

    if (!element.classList.contains("clicked")) {
        element.classList.add("clicked");
    } else {
        element.classList.remove("clicked");
    }
});