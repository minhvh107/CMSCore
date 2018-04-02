window.loadMainModalEvent = function () {
    $(".fileUploadImage").on('change', function () {
        var fileUpload = $(this).get(0);
        var files = fileUpload.files;
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        $.ajax({
            type: "POST",
            url: "/Admin/Upload/UploadImage",
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {
                $('#listImage').append(genBoxImage(response.Data));
                $.notifySuccess(response);
                loadImageFc();
            },
            error: function () {
                $.notifyError(response);
            }
        });
    });
    loadImageFc();

    $(".btn-save-image").on('click', function (e) {
        var imageList = [];
        $.each($('#listImage').find('img'), function (i, item) {

            imageList.push($(this).data('path'));
        });
        $("#JsonListImage").val(imageList);

        var $form = $('.modal form');
        var url = $($form).attr('action');
        //var data = $($form).serializeObject();
        var data = {
            ProductId: $("#ProductId").val(),
            JsonListImage: imageList
        };
        $.appendSpin();
        $.postStringify(url, data, function (response) {
            $.removeSpin();
            if (response.Success) {
                $.notifySuccess(response);
                $("#myModal").hideModal();
            } else {
                $('label.error').hide();
                $('input').removeClass('error');
                if (response.Errors != null && response.Errors != undefined) {
                    validator.showErrors(response.Errors);
                } else {
                    $.notifyError(response);
                }
            }
        });
    });
}

function loadImageFc() {
    $(".delImage").on("click", function () {
        var delImg = $(this);
        $.confirm({
            title: "Xác nhận xóa",
            content: 'Bạn có muốn xóa ảnh này?',
            buttons: {
                confirm: {
                    text: "Đồng ý",
                    btnClass: 'btn-blue',
                    action: function () {
                        $(delImg).parents(".col-md-2").remove();
                    }
                },
                cancel: {
                    text: "Hủy",
                    action: function () {
                    }
                }
            }
        });
    });
}

function genBoxImage(path) {
    return '<div class="col-lg-2 col-md-2 col-sm-2 col-xs-12">' +
            '<div class="boxImage">' +
                '<span class="delImage"><i class="fa fa-trash"></i></span>' +
                '<img class="img-responsive imgUpload" data-path="' + path + '" src="' + path + '"/>' +
            '<div>' +
        '</div>';
}