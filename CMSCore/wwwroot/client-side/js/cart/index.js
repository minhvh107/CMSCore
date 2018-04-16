var CartController = function () {
    var cachedObj = {
        colors: [],
        sizes: [],
    }
    this.initialize = function () {
        $.when(loadColors(),
            loadSizes())
            .then(function () {
                loadData();
            });

        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    $.notifySuccess('Xóa sản phẩm thành công');
                    loadHeaderCart();
                    loadData();
                }
            });
        });
        $('body').on('keyup', '.txtQuantity', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var q = $(this).val();
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q
                    },
                    success: function () {
                        $.notifySuccess('Cập nhật số lượng thành công');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                $.notifyError('Cập nhật số lượng thất bại');
            }
        });

        $('body').on('change', '.ddlColorId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var colorId = $(this).val();
            var q = $(this).closest('tr').find('.txtQuantity').first().val();
            var sizeId = $(this).closest('tr').find('.ddlSizeId').first().val();

            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        $.notifySuccess('Cập nhật màu sắc thành công');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                $.notifyError('Cập nhật màu sắc thất bại');
            }
        });

        $('body').on('change', '.ddlSizeId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var sizeId = $(this).val();
            var q = parseInt($(this).closest('tr').find('.txtQuantity').first().val());
            var colorId = parseInt($(this).closest('tr').find('.ddlColorId').first().val());
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        $.notifySuccess('Cập nhật kích cỡ thành công');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                $.notifyError('Cập nhật kích cỡ thất bại');
            }
        });
        $('#btnClearAll').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'post',
                success: function () {
                    $.notifySuccess('Xóa giỏ hàng thành công');
                    loadHeaderCart();
                    loadData();
                }
            });
        });
    }
    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                $.notifyError('Lấy danh sách màu thất bại');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                $.notifyError('Lấy danh sách size thất bại');
            }
        });
    }
    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }
    function loadHeaderCart() {
        $("#headerCart").load("/Cart/LoadHeaderCart");
    }
    function loadData() {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Price,
                            Quantity: item.Quantity,
                            Colors: getColorOptions(item.Color == null ? 0 : item.Color.Id),
                            Sizes: getSizeOptions(item.Size == null ? "" : item.Size.Id),
                            Amount: item.Price * item.Quantity,
                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                        });
                    totalAmount += item.Price * item.Quantity;
                });
                $('#lblTotalAmount').text(totalAmount);
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html('You have no product in cart');
            }
        });
        return false;
    }
}