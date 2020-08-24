$('.product-add').on('click', (event) => {
    event.preventDefault();

    let article = $(event.target).parent().parent().parent().parent().parent();
    let inputField = article.find('.product-input');
    let productList = article.find('.ol-products');
    let recipeIdVal = article.find('.recipe-id').val();
    let errorMessage = article.find('.error-message-details');

    if (inputField.val().length === 0) {
        errorMessage.text('Product name cannot be empty!');
        errorMessage.fadeIn(600);
        return;
    } else {
        errorMessage.fadeOut(600);
    }
    $.ajax({
        url: '/Product/Create',
        method: 'POST',
        data: {
            Name: inputField.val(),
            RecipeId: recipeIdVal
        }
    }).then((res) => {
        if (res === -1) {
            errorMessage.text('Product already exists!');
            errorMessage.fadeIn(600);
            return;
        }
        let removeItem = $('<span>').addClass('remove-product')
            .attr('data-toggle', 'modal').attr('data-target', '#deleteProductModal')
            .append($('<i>').addClass('fas fa-trash'));
        let liItem = $('<li>').text(inputField.val()).val(res).css('display', 'none')
            .prepend(" ").prepend(removeItem.on('click', moveIdToModel));

        productList.append(liItem);
        inputField.val('');
        liItem.fadeIn(700);
    });
});

$('.remove-product').on('click', removeProduct);
$('.remove-product-confirm').on('click', moveIdToModel);

function moveIdToModel(event) {
    let productId = $(event.target).parent().parent().val();
    $('#deleteProductModal .remove-product').attr('data-product-id', productId);
}

async function removeProduct(event) {
    event.preventDefault();
    let productId = $(event.target).attr('data-product-id');
    let product = $(event.target).parent().parent().parent().parent().parent().find(`li[value='${productId}']`)

    await $.ajax({
        url: `/Product/Delete/${product.val()}`,
        method: 'POST'
    });

    $('.btn-close-model').click();
    product.fadeOut(400, () => {
        product.remove();
    });
}

$('.add-product-button').on('click', (event) => {
    event.preventDefault();
    let article = $(event.target).parent().parent().parent().parent();
    let products = article.find('.add-product-block');
    let addButton = $(event.target).find('.fas').context;

    if (products.css('display') === 'none') {
        products.fadeIn(600);
        $(addButton).removeClass('fa-cart-plus');
        $(addButton).addClass('fa-cart-arrow-down');
    } else {
        products.fadeOut(600);
        $(addButton).removeClass('fa-cart-arrow-down');
        $(addButton).addClass('fa-cart-plus');
        article.find('.error-message-details').fadeOut(600);
    }
});