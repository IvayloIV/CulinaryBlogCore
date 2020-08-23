$('.rating-star').on('click', (event) => {
    let rating = $(event.target).attr('data-rating');
    let recipeId = $(event.target).parent().attr('data-recipe-id');
    let recipeRatingField = $(event.target).parent().parent().parent().find('.recipe-vote');
    let recipeRating = recipeRatingField.text().split(/[()]/)[1];

    let ratingBlock = $(event.target).parent().parent();
    ratingBlock.removeClass('rating');
    ratingBlock.empty();

    ratingBlock.addClass('rating-already');
    for (let i = 1; i <= 5; i++) {
        ratingBlock.append($('<span>').addClass('rating-blocked').html(i <= rating ? '★' : '☆'));
    }

    $.ajax({
        url: `/Recipe/ChangeRating`,
        method: 'POST',
        data: {
            Rating: rating,
            RecipeId: recipeId
        }
    }).then((newRating) => {
        console.log(newRating);
        ratingBlock.append($('<span>').addClass('recipe-vote').text(`${newRating.toFixed(2)} (${Number(recipeRating) + 1})`));
    });
});