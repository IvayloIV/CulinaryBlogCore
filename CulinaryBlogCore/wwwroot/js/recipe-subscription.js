$('#recipe-subscription').on('submit', (event) => {
    event.preventDefault();
    let emailInput = $(event.target).find('input[name="email"]');
    let emailVal = emailInput.val();
    let message = $('.subscription-message')

    emailInput.val('');
    message.fadeIn(600);
    message.text('Waiting...');

    $.ajax({
        url: '/Mail/Recipe/Subscribe',
        method: 'POST',
        data: {
            email: emailVal
        }
    }).then((res) => {
        message.text(res.message);
    });
});

$('.subscriptions-send-form').on('submit', (event) => {
    event.preventDefault();
    let message = $('.subscriptions-send-message');

    $('.btn-close-model').click();
    message.text('Sending...');
    message.fadeIn(600);

    $.ajax({
        url: '/Administration/Mail/Recipe/Send',
        method: 'POST'
    }).then((res) => {
        message.text(res.message);
    });
});