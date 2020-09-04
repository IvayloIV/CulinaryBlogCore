$('.new-trick').on('click', async () => {
    let chefsSelect = $('.trick-create .chefs-tricks');
    await loadCategories(chefsSelect);
    clearCreateForm();

    $('.tricks-title').css('height', '39rem');
    $('.new-trick').fadeOut(1000);
});

$('.trick-img').on('change', (event) => {
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
        let article = $('<article>').data('trickId', res.id)
            .append(buildArticleButtons())
            .append($('<div>').addClass('tricks-articles-picture')
                .append($('<img>').attr('src', res.imagePath).attr('alt', res.name)))
            .append(buildArticleContent(res, target))
            .append(buildArticleUpdateForm());
        $('.tricks-articles').prepend(article);
        clearCreateForm();
        hideCreateForm();
    });
});

$('.btn-trick-update').on('click', bthTrickUpdate);

async function bthTrickUpdate(event) {
    let article = $(event.target).closest('article');
    let name = article.find('.tricks-articles-content h4').text();
    let description = article.find('.tricks-articles-content p').text();
    let chefId = article.find('.tricks-articles-content .chef-id').text();

    $(article).find('.btn-trick-update').css('display', 'none');
    $(article).find('.undo').css('display', 'inline-block');

    $(article).find('.trick-update .add-trick-name').val(name);
    $(article).find('.trick-update .add-trick-description').val(description);
    let chefsSelect = article.find('.trick-update .chefs-tricks');
    await loadCategories(chefsSelect, chefId);
    moveUpdateForm('54px', '388px', 1, article);
}

$('.tricks .undo').on('click', trickUndo);

function trickUndo(event) {
    let article = $(event.target).closest('article');
    undoButtons(article);
    moveUpdateForm('388px', '54px', -1, article);
}

function undoButtons(article) {
    $(article).find('.btn-trick-update').css('display', 'inline-block');
    $(article).find('.undo').css('display', 'none');
}

$('.btn-trick-delete').on('click', addTrickId);

$('.delete-trick').on('click', (event) => {
    event.preventDefault();

    let trickId = $(event.target).data('trickId')
    $.ajax({
        method: 'POST',
        url: `/Trick/Delete/${trickId}`,
        data: {
            id: trickId
        }
    }).then((res) => {
        $('.tricks-articles article').each((i, e) => {
            if ($(e).data('trickId') == trickId) {
                $(e).remove();
            }
        });
        $('.btn-close-model').click();
    });
});


$('.btn-trick-update-submit').on('click', updateTrickBtn);

function updateTrickBtn(event) {
    let article = $(event.target).closest('article');
    let trickId = article.data('trickId');
    $('.update-trick').data('trickId', trickId)
}

$('.update-trick').on('click', (event) => {
    let trickId = $(event.target).data('trickId');
    let article = $('.tricks-articles article')
        .toArray()
        .find(a => $(a).data('trickId') === trickId);

    let updateMessage = $(article).find('.trick-update-message');

    let updateForm = $(article).find('.trick-update');
    if (!updateForm[0].checkValidity()) {
        $('.btn-close-model').click();
        updateMessage.text('Fill all fields.');
        updateMessage.css('display', 'block');
        return;
    } else {
        updateMessage.css('display', 'none');
    }

    let name = $(article).find('.add-trick-name').val();
    let description = $(article).find('.add-trick-description').val();
    let chefId = $(article).find('.chefs-tricks').val();
    let chefName = $(article).find('.chefs-tricks :selected').text();

    $.ajax({
        method: 'POST',
        url: `/Trick/Update/${trickId}`,
        data: {
            Name: name,
            Description: description,
            ChefId: chefId
        }
    }).then((res) => {
        $(article).find('.tricks-articles-content h4').text(res.name);
        $(article).find('.tricks-articles-content p').text(res.description);
        $(article).find('.tricks-articles-content .chef-name-trick').text(`chef ${chefName}`);
        $(article).find('.tricks-articles-content .chef-id').text(res.chefId);

        undoButtons(article);
        $('.btn-close-model').click();
        moveUpdateForm('388px', '54px', -1, article);
    });
})

function hideCreateForm() {
    $('.tricks-title').css('height', '24rem');
    $('.new-trick').fadeIn(1000);
}

function clearCreateForm() {
    $('.trick-create .add-trick-name').val('');
    $('.trick-create .add-trick-description').val('');
    $('.trick-create .chefs-tricks').val('default');
    $('.trick-create .trick-img').val('');
}

function addTrickId(event) {
    let trickId = $(event.target).closest('article').data('trickId');
    $('.delete-trick').data('trickId', trickId);
}

async function loadCategories(htmlTag, chefId) {
    let categories = await $.ajax({
        method: 'GET',
        url: 'Administration/Chef/GetAll'
    });

    htmlTag.empty();
    if (chefId == null) {
        htmlTag.append($('<option>').val('default').attr('selected', 'selected').attr('disabled', 'true').text('Select chef'));
    }
    categories.forEach(c => {
        let option = $('<option>');
        if (c.id == chefId) {
            option.attr('selected', 'selected');
        }
        htmlTag.append(option.val(c.id).text(c.lastName));
    });
}

function buildArticleButtons() {
    return $('<div>').addClass('update-trick-buttons')
        .append($('<button>').attr('type', 'button').addClass('btn-trick-update').on('click', bthTrickUpdate)
            .append($('<i>').addClass('fas fa-pen')))
        .append($('<button>').attr('type', 'button').addClass('undo').on('click', trickUndo)
            .append($('<i>').addClass('fas fa-undo-alt')))
        .append($('<button>').attr('type', 'button').addClass('btn-trick-delete').on('click', addTrickId)
                .attr('data-toggle', 'modal').attr('data-target', '#deleteTrickModal')
            .append($('<i>').addClass('fas fa-times')));
}

function buildArticleContent(res, target) {
    return $('<div>').addClass('tricks-articles-content')
        .append($('<h4>').text(res.name))
        .append($('<p>').text(res.description))
        .append($('<span>').addClass('chef-name-trick').text(`chef ${target.find('.chefs-tricks :selected').text()}`))
        .append($('<span>').addClass('chef-id').text(target.find('.chefs-tricks :selected').val()));
}

function buildArticleUpdateForm() {
    return $('<form>').addClass('trick-update').attr('action', '#').attr('method', 'post')
        .append($('<h2>').text('Update'))
        .append($('<input>').attr('type', 'text').attr('name', 'name')
            .attr('placeholder', 'Name').addClass('add-trick-name'))
        .append($('<textarea>').attr('name', 'description').attr('rows', '4').attr('placeholder', 'Description')
            .addClass('add-trick-description'))
        .append($('<select>').attr('name', 'chef').addClass('chefs-tricks'))
        .append($('<button>').attr('type', 'button').addClass('btn-trick-update-submit').on('click', updateTrickBtn)
            .attr('data-toggle', 'modal').attr('data-target', '#updateTrickModal').text('Update'));
}

function moveUpdateForm(moveMin, moveMax, zIndex, article) {
    let updateForm = $(article).find('.trick-update');
    let articleContent = $(article).find('.tricks-articles-content');

    let direction = '';
    if (updateForm.css('right') === moveMin) {
        direction = 'right';
    } else {
        direction = 'left';
    }

    let articleContentMax = moveMax;
    if (zIndex === -1) {
        updateForm.css('z-index', zIndex);
        articleContentMax = '0px';
    }

    updateForm.animate({
        [direction]: moveMax,
    }, 1500);
    articleContent.animate({
        [direction]: articleContentMax
    }, 1500, () => {
        updateForm.css('z-index', zIndex);
    });
}