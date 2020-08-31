$('.new-trick').on('click', async () => {
    let categories = await $.ajax({
        method: 'GET',
        url: 'Administration/Chef/GetAll'
    });

    let createTrickForm = $('.trick-create');
    let chefsSelect = createTrickForm.find('.chefs-tricks');

    chefsSelect.empty();
    chefsSelect.append($('<option>').val('default').attr('selected', 'true').attr('disabled', 'true').text('Select one'));
    categories.forEach(c => {
        chefsSelect.append($('<option>').val(c.id).text(c.lastName));
    });
    clearCreateForm();
    $('.tricks-title').css('height', '39rem');
    $('.new-trick').fadeOut(1000);
});

$('.trick-img').on('change', (event) => {
    console.log($(event.target).val());
    $('.curr-image-trick').text($(event.target).val().split('\\').slice(-1));
});

$('.trick-back-create').on('click', hideCreateForm);

$('.trick-create').on('submit', (event) => {
    event.preventDefault();
    let target = $(event.target);

    let formData = new FormData();
    formData.append("Name", target.find('.add-trick-name').val());
    formData.append("Description", target.find('.add-trick-description').val());
    formData.append("ChefId", target.find('.chefs-tricks').val());
    formData.append("Image", target.find('.trick-img').prop('files')[0]);

    $('.curr-image-trick').text('Add image');

    $.ajax({
        method: 'POST',
        url: '/Trick/Create',
        processData: false,
        contentType: false,
        data: formData
    }).then((res) => {
        hideCreateForm();

        console.log(res);
        let article = $('<article>')
            .append($('<div>').addClass('tricks-articles-picture')
                .append($('<img>').attr('src', res.imagePath).attr('alt', res.name)))
            .append($('<div>').addClass('tricks-articles-content')
                .append($('<h4>').text(res.name))
                .append($('<p>').text(res.description))
                .append($('<span>').text(`chef ${target.find('.chefs-tricks :selected').text()}`)));
        $('.tricks-articles').prepend(article);
        clearCreateForm();
    });
});

function hideCreateForm() {
    $('.tricks-title').css('height', '24rem');
    $('.new-trick').fadeIn(100);
}

function clearCreateForm() {
    $('.add-trick-name').val('');
    $('.add-trick-description').val('');
    $('.chefs-tricks').val('default');
    $('.trick-img').val('');
}