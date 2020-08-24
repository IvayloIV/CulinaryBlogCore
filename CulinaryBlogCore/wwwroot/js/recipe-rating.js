$('.rating-star').on('click', (event) => {
    let rating = $(event.target).attr('data-rating');
    let recipeId = $(event.target).parent().attr('data-recipe-id');
    let recipeRatingField = $(event.target).parent().parent().parent().find('.recipe-vote');
    let recipeRating = recipeRatingField.text().split(/[()]/)[1];

    let ratingBlock = $(event.target).parent().parent();
    ratingBlock.removeClass('rating');
    ratingBlock.find('span').each((i, e) => {
        if ($(e).attr('class') === 'rating-star') {
            $(e).remove();
        }
    });

    ratingBlock.addClass('rating-already');
    for (let i = 5; i >= 1; i--) {
        ratingBlock.prepend($('<span>').addClass('rating-blocked').html(i <= rating ? '★' : '☆'));
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