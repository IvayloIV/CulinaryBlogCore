$('#receipt-menu').on('click', () => {
    let ul = $('.header-inner-nav');

    if (!ul.hasClass('clicked')) {
        ul.addClass('clicked');
    } else {
        ul.removeClass('clicked');
    }
});