$('.product-add').on('click', (event) => {
    event.preventDefault();
    let inputField = $('.product-input');
    let productList = $('.ol-products');
    let recipeIdVal = $('.recipe-id').val();

    $.ajax({
        url: '/Product/Create',
        method: 'POST',
        data: {
            Name: inputField.val(),
            RecipeId: recipeIdVal
        }
    }).then((res) => {
        let removeItem = $('<span>').addClass('remove-product')
            .append($('<i>').addClass('fas fa-trash'));
        let liItem = $('<li>').text(inputField.val()).val(res).css('display', 'none')
            .prepend(removeItem.on('click', removeProduct));

        productList.append(liItem);
        inputField.val('');
        liItem.fadeIn(700);
    });
});

$('.remove-product').on('click', removeProduct);

async function removeProduct(event) {
    event.preventDefault();
    let product = $(event.target).parent().parent();

    await $.ajax({
        url: `/Product/Delete/${product.val()}`,
        method: 'POST'
    });
    product.fadeOut(400, () => {
        product.remove();
    });
}

$('.add-product-button').on('click', (event) => {
    event.preventDefault();
    let products = $('.add-product-block');
    let addButton = $(event.target).find('.fas').context;

    if (products.css('display') === 'none') {
        products.fadeIn(600);
        $(addButton).removeClass('fa-cart-plus');
        $(addButton).addClass('fa-cart-arrow-down');
    } else {
        products.fadeOut(600);
        $(addButton).removeClass('fa-cart-arrow-down');
        $(addButton).addClass('fa-cart-plus');
    }
});