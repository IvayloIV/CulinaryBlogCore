$('#recipe-subscription').on('submit', (event) => {
    event.preventDefault();
    let emailInput = $(event.target).find('input[name="email"]');
    let emailVal = emailInput.val();
    let message = $('.subscription-message')

    emailInput.val('');
    message.fadeIn(600);
    message.text('Sending...');

    $.ajax({
        url: '/Mail/Recipe/Subscribe',
        method: 'POST',
        data: {
            email: emailVal
        }
    }).then((res) => {
        message.text(res);
    });
});