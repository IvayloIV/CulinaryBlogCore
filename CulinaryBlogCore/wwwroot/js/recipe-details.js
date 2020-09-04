$('.add-product-button').on('click', addProductButton);
$('.product-add').on('click', addProduct);

$('.remove-product').on('click', removeProduct);
$('.remove-product-confirm').on('click', moveIdToModel);

function addProductButton(event) {
    event.preventDefault();
    let target = $(event.target);

    let article = target.closest('article');
    let products = article.find('.add-product-block');
    let addButton = target.find('.fas').context;

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
}

function addProduct(event) {
    event.preventDefault();

    let article = $(event.target).closest('article');
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
        if (!res.isValid) {
            errorMessage.text(res.message);
            errorMessage.fadeIn(600);
            return;
        }

        let removeItem = $('<span>').addClass('remove-product')
            .attr('data-toggle', 'modal').attr('data-target', '#deleteProductModal')
            .append($('<i>').addClass('fas fa-trash'));
        let liItem = $('<li>').text(inputField.val()).val(res.productId).css('display', 'none')
            .prepend(" ").prepend(removeItem.on('click', moveIdToModel));

        productList.append(liItem);
        inputField.val('');
        liItem.fadeIn(700);
    });
}

function moveIdToModel(event) {
    let productId = $(event.target).closest('li').val();
    $('#deleteProductModal .remove-product').data('productId', productId);
}

async function removeProduct(event) {
    event.preventDefault();
    let target = $(event.target);

    let productId = target.data('productId');
    let product = target.closest('section').find(`li[value='${productId}']`);

    await $.ajax({
        url: `/Product/Delete/${product.val()}`,
        method: 'POST'
    });

    $('.btn-close-model').click();
    product.fadeOut(400, () => {
        product.remove();
    });
}