$('.rating-star').on('click', (event) => {
    let newRating = $(event.target).attr('data-rating');
    let recipeId = $(event.target).parent().attr('data-recipe-id');
    let recipeRatingField = $(event.target).parent().parent().parent().find('.recipe-vote');
    let recipeRating = recipeRatingField.text().split(/[()]/)[1];

    console.log(`/Recipe/ChangeRating/${recipeId}`);
    $.ajax({
        url: `/Recipe/ChangeRating/${recipeId}`,
        method: 'POST',
        data: { newRating }
    }).then((newRating) => {
        recipeRatingField.text(`${Number(newRating.toFixed(2))} (${Number(recipeRating) + 1})`);
    });
});