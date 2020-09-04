$('.rating-star').on('click', (event) => {
    let target = $(event.target);

    let rating = target.data('rating');
    let recipeId = target.closest('.ratring-stars').data('recipeId');
    let recipeRatingField = target.closest('div').find('.recipe-vote');
    let recipeRating = recipeRatingField.text().split(/[()]/)[1];

    let ratingBlock = target.closest('p');
    ratingBlock.removeClass('rating');
    ratingBlock.find('span').each((i, e) => {
        if ($(e).attr('class') === 'rating-star') {
            $(e).remove();
        }
    });

    ratingBlock.addClass('rating-already');

    for (let i = 5; i >= 1; i--) {
        ratingBlock.prepend($('<span>').addClass('rating-blocked').html(i <= rating ? ' ★ ' : ' ☆ '));
    }

    $.ajax({
        url: `/Recipe/ChangeRating`,
        method: 'POST',
        data: {
            Rating: rating,
            RecipeId: recipeId
        }
    }).then((newRating) => {
        ratingBlock.find('.recipe-vote').text(`${newRating.toFixed(2)} (${Number(recipeRating) + 1})`);
    });
});