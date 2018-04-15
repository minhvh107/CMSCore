(function ($) {
    /*  Loading   */
    $.appendSpin = function () {
        spinner = new Spinner().spin();
        $('body').append("<div class='overlay'></div>");
        $(".overlay").append(spinner.el);
        $("body").addClass("body-overflow");
    };

    $.removeSpin = function () {
        $("body").removeClass("body-overflow");
        setTimeout(function () {
            spinner.stop();
            $(".overlay").remove();
        }, 1000);
    };

    /* Thông báo */
    $.notifySuccess = function (message) {
        var pNotify = new PNotify({
            title: 'Thành công!',
            text: message,
            type: 'success',
            styling: 'bootstrap3',
            delay: 3000
        });
    };

    $.notifyError = function (message) {
        if (response.StatusCode == 500) {
            $.alert({
                title: "Có lỗi xảy ra",
                content: message,
                type: 'red',
                columnClass: "small",
                buttons: {
                    confirm: {
                        text: "Đóng",
                        btnClass: 'btn-blue'
                    }
                }
            });
        } else {
            var pNotify = new PNotify({
                title: 'Có lỗi xảy ra!',
                text: message,
                type: 'error',
                styling: 'bootstrap3',
                delay: 3000
            });
        }
    };

    $.notifyWarning = function (options) {
        var pNotify;
        if (options != null) {
            pNotify = new PNotify(options);
        } else {
            pNotify = new PNotify({
                title: 'Chú ý!',
                text: "Không có dòng nào được chọn.",
                type: 'warning',
                styling: 'bootstrap3'
            });
        }
    };

    /* init */
    /* Tiền tệ 100000 => 100,000 */
    $.fn.digits = function () {
        return this.each(function () {
            $(this).text($(this).val().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
    }

    /* Format Number */
    $.fn.formatNumber = function () {
        this.number(true, 0, ".", ",");
    }

    $.fn.formatNumber2 = function () {
        this.number(true, 2, ".", ",");
    }

    // Datepicker
    $.initDatepicker = function () {
        $(".datepicker").datetimepicker({
            format: "DD/MM/YYYY",
            locale: 'vi'
        });
    }

    // Check box
    $.fn.initICheck = function () {
        $(this).find('input.flat').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        });

        $("input[type='text']").on("focus", function () {
            $(this).select();
        });
    }

    // Ckeditor
    $.fn.initCkeditor = function () {
        $('.ckeditor').each(function () {
            CKEDITOR.replace(this.id);
        });
    }

    // File Image
    $.fn.initFileImage = function () {
        $('.btnSelectImg').on('click', function () {
            $(this).parents(".input-group").find(".fileInputImage").click();
        });
        $(".fileInputImage").on('change', function () {
            var idInputFile = $(this).parents(".input-group").find(".form-control").attr("id");
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
                    $("#" + idInputFile).val(response.Data);
                    $.notifySuccess(response);
                },
                error: function () {
                    $.notifyError(response);
                }
            });
        });
    }

    // Focus Input
    $.fn.focusFirstInput = function () {
        this.find("input:not([type=hidden]):first").focus();
    }

    // Select 2
    $.initSelect2 = function () {
        $('select').not(".page-size").not('.no-select2').select2({
            "language": {
                "noResults": function () {
                    return "Không có dữ liệu";
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            }
        });
    }

    // Disable input
    $.isDisable = function () {
        setTimeout(function () {
            $(".isDisable").prop('disabled', 'true');
        }, 200);
    }

    /* Gen Guid */
    $.generateGuid = function guid() {
        return $.s4() + $.s4() + '-' + $.s4() + '-' + $.s4() + '-' +
            $.s4() + '-' + $.s4() + $.s4() + $.s4();
    }

    $.s4 = function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }


}(jQuery));