//var spinner = new Spinner().spin();
var validator;
var subValidator = null;
var dataTableMyModal = [];
var spinner = new Spinner().spin();
var formStringify = "";
var enumMethod = {
    Post: "POST",
    Get: "GET"
};
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
    $.notifySuccess = function (response) {
        var pNotify = new PNotify({
            title: 'Thành công!',
            text: response.Message,
            type: 'success',
            styling: 'bootstrap3',
            delay: 3000
        });
    };

    $.notifyError = function (response) {
        if (response.StatusCode == 500) {
            $.alert({
                title: "Có lỗi xảy ra",
                content: response.Message,
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
                text: response.Message,
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

    /* Modal */
    $.fn.openModal = function (content) {
        if (content != null && content.length > 0) {
            this.html(content);
            validator = this.find('form').validate();
            $.loadEventModal();
            $.initDatepicker();
            this.initICheck();
            this.initCkeditor();
            this.initFileImage();
            this.initImportFileExcel();
            $.initSelect2();
            $.isDisable();
            this.focusFirstInput();
            //chỉ cho nhập số
            $(".currency").formatNumber();
            $('.currency2').formatNumber2();

            //TODO: Sub Modal
            if ($('#subModal').length > 0) {
                $.loadSubformEvent();
                if (this.find('table.bulk_action').length > 0) {
                    $.loadEventRowClick();
                    $.loadActiveSubControl();
                    //Todo : Table Modal
                    if ($('#JsonTableMyModal').length > 0 && $("#JsonTableMyModal").val() != "") {
                        var obj = JSON.parse($("#JsonTableMyModal").val());
                        dataTableMyModal = obj;
                    }
                }
            }

            //Todo: Load event of myModal
            if (window.loadMainModalEvent != null && window.loadMainModalEvent != undefined) {
                window.loadMainModalEvent();
            }

            formStringify = JSON.stringify($("#myModal form").serializeObject());


        }
        $(this).modal({ backdrop: 'static', keyboard: false });
        $(this).modal('show');
    };

    $.loadEventModal = function () {
        $(".btn-save").on('click', function (e) {
            if (window.savefnc != null && window.savefnc != undefined) {
                window.savefnc();
            } else {
                e.preventDefault();
                var $modal = $(this).parents(".modal");
                var $form = $(this).parents(".modal").find('form');
                $form.submitForm(function (response) {
                    $modal.hideModal();
                    $.reloadData();
                });

            }
        });
    }

    /* Sub Modal */
    $.fn.openSubModal = function (content) {
        if (content != null && content.length > 0) {
            this.html(content);
            subValidator = this.find('form').validate();
            $.loadEventSubModal();
            $.initDatepicker();
            this.initICheck();
            this.initCkeditor();
            this.initFileImage();
            this.initImportFileExcel();
            $.initSelect2();
            $.isDisable();
            this.focusFirstInput();
            $("#subModal .currency").formatNumber();
            $('#subModal .currency2').formatNumber2();
            if ($("#Guid").val() == "") {
                $("#Guid").val($.generateGuid());
            }
            if (window.loadSubModalEvent != null && window.loadSubModalEvent != undefined) {
                window.loadSubModalEvent();
            }

        }
        $(this).modal({ backdrop: 'static', keyboard: false });
        $(this).modal('show');
    };

    $.loadEventSubModal = function () {
        $(".btn-save-sub").on('click', function (e) {
            e.preventDefault();
            var $modal = $(this).parents(".modal");
            var $form = $(this).parents(".modal").find('form');
            var url = $($form).attr('action');
            if ($($form).valid()) {
                var obj = $($form).serializeObject();

                $.postStringify(url, obj, function (response) {
                    if (response.Success) {
                        $.notifySuccess(response);
                        //Todo Table Modal
                        var index = -1;
                        dataTableMyModal.map(function (item, idx) {
                            if (item.Guid == obj["Guid"]) {
                                index = idx;
                            }
                        });
                        if (index > -1) {
                            dataTableMyModal.splice(index, 1);
                        }
                        dataTableMyModal.push(obj);
                        $("#JsonTableMyModal").val(JSON.stringify(dataTableMyModal));
                        $("#tableMyModal tr").each(function (i, v) {
                            if ($(this).hasClass('no-data') || $(this).attr('guid') == obj["Guid"]) {
                                $(this).remove();
                            }
                        });
                        $("#tableMyModal").append(response.Data);
                        $.loadSubformEvent();
                        $.loadActiveSubControl();
                        $modal.hideModal();
                    } else {
                        $('label.error').hide();
                        $('input').removeClass('error');
                        if (response.Errors != null && response.Errors != undefined) {
                            subValidator.showErrors(response.Errors);
                        } else {
                            $.notifyError(response);
                        }
                    }
                });
            }
        });
    }

    $.loadActiveSubControl = function () {
        if ($('#myModal table.bulk_action tbody tr.selected').length >= 1 && $('#myModal table.bulk_action tbody tr.selected').attr("guid") != undefined
            && $('#subModal table.bulk_action tbody tr.selected').attr("guid") != "") {
            $(".btn-remove-subitem").removeClass("disabled");
            $(".btn-update-subitem").removeClass("disabled");
        } else {
            $(".btn-remove-subitem").addClass("disabled");
            $(".btn-update-subitem").addClass("disabled");
        }
    }

    $.loadSubformEvent = function () {
        $.loadEventRowClick();
        $(".btn-add-subitem").off('click').on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }
            var urlGetContent = $(this).attr('url-content');
            $.callAjax(urlGetContent, null, enumMethod.Get, function (response) {
                if (response.Success) {
                    $("#subModal").openSubModal(response.Data);
                } else {
                    $.notifyError(response);
                }
            });
        });

        $('#myModal table.bulk_action td').dblclick(function () {
            var urlGetContent = $("#sub-url-view").val();
            var guid = $(this).parent().attr("guid");
            if (guid != undefined && guid.length > 0) {
                var item = null;
                dataTableMyModal.map(function (item1, idx) {
                    if (item1.Guid == guid) {
                        item = item1;
                    }
                });
                if (item != null) {
                    $.postStringify(urlGetContent, item, function (response) {
                        if (response.Success) {
                            $("#subModal").openSubModal(response.Data);
                            $('#subModal input').prop('disabled', 'true');
                            $('#subModal select').prop('disabled', 'true');
                        } else {
                            $.notifyError(response);
                        }
                    });
                }
            }
        });

        $(".btn-update-subitem").off('click').on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }
            var urlGetContent = $(this).attr('url-content');
            var table = $(this).parent().parent().find('table.bulk_action');

            if ($(table).find("tr.selected").length <= 0) {
                $.notifyWarning();
            } else if ($(table).find("tr.selected").length == 1) {
                var guid = $(table).find("tr.selected").attr("guid");
                if (guid != undefined && guid.length > 0) {
                    var item = null;
                    dataTableMyModal.map(function (item1, idx) {
                        if (item1.Guid == guid) {
                            item = item1;
                        }
                    });
                    $.postStringify(urlGetContent, item, function (response) {
                        $("#subModal").openSubModal(response.Data);
                    });
                }
            } else {
                $.notifyError(response);
            }
        });

        $(".btn-remove-subitem").off('click').on('click', function () {
            if ($(this).hasClass('disabled')) {
                return;
            }
            if ($("#tableMyModal tr.selected").length <= 0) {
                $.notifyWarning();
            } else if ($("#tableMyModal tr.selected").length === 1) {
                var guid = $("#tableMyModal tr.selected").attr("guid");
                if (guid == undefined || guid.length <= 0) {
                    return;
                }
                var index = -1;
                dataTableMyModal.map(function (item, idx) {
                    if (item.Guid == guid) {
                        index = idx;
                    }
                });
                if (index > -1) {
                    $.confirm({
                        title: 'Xác nhận xóa',
                        content: "Bạn có muốn xóa dòng này?",
                        buttons: {
                            confirm: {
                                text: "Đồng ý",
                                btnClass: 'btn-blue',
                                action: function () {
                                    dataTableMyModal.splice(index, 1);
                                    $.notifySuccess({ Message: "Xóa thành công" });
                                    $("#JsonTableMyModal").val(JSON.stringify(dataTableMyModal));
                                    $("#tableMyModal tr.selected").remove();
                                    if (listBillDetails.length == 0) {
                                        $("#tableMyModal").append('<tr class="no-data"><td colspan="4">Không có dữ liệu</td></tr>');
                                        $("#JsonTableMyModal").val("");
                                    }
                                    $.loadEventRowClick();
                                    $.loadActiveSubControl();
                                }
                            },
                            cancel: {
                                text: "Hủy",
                                action: function () {
                                    //$.alert('Canceled!');
                                }
                            }
                        }
                    });
                }
            } else {
                console.log("error");
            }
        });
    }

    /* All Modal */
    $.fn.hideModal = function () {
        $(this).html('');
        $(this).modal('hide');
    };

    /* Table */
    $.loadActiveControl = function () {
        if ($('table.bulk_action tbody tr.selected').length >= 1 && $('table.bulk_action tbody tr.selected').attr("item-id") != undefined
            && $('table.bulk_action tbody tr.selected').attr("item-id") != "") {
            $(".btn-remove-item").removeClass("disabled");
            $(".btn-update-item").removeClass("disabled");
        } else {
            $(".btn-remove-item").addClass("disabled");
            $(".btn-update-item").addClass("disabled");
        }

    }

    $.loadEventTable = function () {
        $('table.bulk_action').find('tbody tr:first').addClass('selected');
        $('table.bulk_action td').on('click', function () {
            $(this).parent().parent().find('tr').removeClass('selected');
            $(this).parent().toggleClass('selected');
        });

        $(".btn-add-item").on('click', function () {
            var urlGetContent = $(this).attr('url-content');
            $.appendSpin();
            $.callAjax(urlGetContent, null, enumMethod.Get, function (response) {
                $.removeSpin();
                if (response.Success) {
                    $("#myModal").openModal(response.Data);
                } else {
                    $.notifyError(response);
                }
            });
        });

        $('table.bulk_action td').dblclick(function () {
            var itemId = $(this).parent().attr("item-id");
            var urlGetContent = $(this).parents('.x_panel').find("#url-view").val();
            if (itemId != undefined && itemId != "" && urlGetContent != undefined && urlGetContent != "") {
                $.appendSpin();
                $.callAjax(urlGetContent, { id: itemId }, enumMethod.Get, function (response) {
                    $.removeSpin();
                    if (response.Success) {
                        $("#myModal").openModal(response.Data);
                        if (!response.CanEdit) {
                            $('#myModal input').prop('disabled', 'true');
                            $('#myModal select').prop('disabled', 'true');
                            $('#myModal textarea').prop('disabled', 'true');
                        }
                    } else {
                        $.notifyError(response);
                    }
                });
            }
        });

        $(".btn-update-item").on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }
            var urlGetContent = $(this).attr('url-content');
            var table = $(this).parent().parent().find('table.bulk_action');
            if ($(table).find("tr.selected").length <= 0) {
                $.notifyWarning();
            } else if ($(table).find("tr.selected").length == 1) {
                var itemId = $(table).find("tr.selected").attr("item-id");
                $.appendSpin();
                $.callAjax(urlGetContent, { id: itemId }, enumMethod.Get, function (response) {
                    $.removeSpin();
                    if (response.Success) {
                        $("#myModal").openModal(response.Data);
                    } else {
                        $.notifyError(response);
                    }
                });
            } else {
                console.log("error");
            }
        });

        $(".btn-remove-item").on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }
            var urlRemove = $(this).attr('url-content');
            var table = $(this).parent().parent().find('table.bulk_action');
            if ($(table).find("tr.selected").length <= 0) {
                $.notifyWarning();
            } else if ($(table).find("tr.selected").length === 1 && $(table).find("tr.selected").attr('item-id') != "") {
                $.confirm({
                    title: "Xác nhận xóa",
                    content: 'Bạn có muốn xóa dòng này?',
                    buttons: {
                        confirm: {
                            text: "Đồng ý",
                            btnClass: 'btn-blue',
                            action: function () {
                                var itemId = $(table).find("tr.selected").attr("item-id");
                                $.callAjax(urlRemove, { id: itemId }, enumMethod.Post, function (response) {
                                    if (response.Success) {
                                        $.reloadData();
                                        $.notifySuccess(response);
                                    } else {
                                        $.notifyError(response);
                                    }
                                });
                            }

                        },
                        cancel: {
                            text: "Hủy",
                            action: function () {
                                
                            }
                        }
                    }
                });
            } else {
                console.log("error");
            }
            return false;
        });

        $('.btn-export-item').on('click', function (e) {
            e.preventDefault();
            var $form = $("#frmFilter");
            var url = $(this).attr('url-content');
            var obj = $form.serializeObject();
            $.postStringify(url, obj, function (response) {
                if (response.Success) {
                    window.location.href = response.Data;
                } else {
                    $.notifyError(response);
                }
            });
        });

        $('.btn-print-excel').on('click', function (e) {
            e.preventDefault();
            var url = $(this).attr('url-content');
            var table = $(this).parent().parent().find('table.bulk_action');
            if ($(table).find("tr.selected").length <= 0) {
                $.notifyWarning();
            } else if ($(table).find("tr.selected").length === 1 && $(table).find("tr.selected").attr('item-id') != "") {
                $.confirm({
                    title: "Xác nhận xóa",
                    content: 'Bạn có muốn in hoá đơn này ?',
                    buttons: {
                        confirm: {
                            text: "Đồng ý",
                            btnClass: 'btn-blue',
                            action: function () {
                                var itemId = $(table).find("tr.selected").attr("item-id");
                                $.postStringify(url, { id: itemId }, function (response) {
                                    if (response.Success) {
                                        window.location.href = response.Data;
                                    } else {
                                        $.notifyError(response);
                                    }
                                });
                            }

                        },
                        cancel: {
                            text: "Hủy",
                            action: function () {

                            }
                        }
                    }
                });
            } else {
                console.log("error");
            }
        });

        $('.btn-import-item').on('click', function (e) {
            e.preventDefault();
            var urlGetContent = $(this).attr('url-content');
            $.appendSpin();
            $.callAjax(urlGetContent, null, enumMethod.Get, function (response) {
                $.removeSpin();
                if (response.Success) {
                    $("#myModal").openModal(response.Data);
                } else {
                    $.notifyError(response);
                }
            });
        });
    }

    $.loadEventRowClick = function () {
        $('#myModal table.bulk_action').find('tbody tr:first').addClass('selected');
        $('table.bulk_action td').on('click', function () {
            $(this).parent().parent().find('tr').removeClass('selected');
            $(this).parent().toggleClass('selected');
        });

    }

    /* Form */
    $.postStringify = function (url, obj, callback) {
        $.appendSpin();
        $.ajax({
            type: "POST",
            url: url,
            data: obj,
            dataType: "json",
            async: false,
            success: function (response) {
                $.removeSpin();
                if (response.StatusCode == 401) {
                    window.location.href = response.Data;
                }
                if (callback != null && callback != undefined) {
                    callback(response);
                } else if (response.Success) {
                    $.notifySuccess(response);
                } else {
                    $.notifyError(response);
                }
            },
            error: function (error) {
                $.removeSpin();
                console.log(error);
            }
        });
    };

    $.fn.submitForm = function (callback) {
        var url = $(this).attr('action');
        if ($(this).valid()) {
            var obj = $(this).serializeObject();
            if ($('#Content').length) {
                obj['Content'] = CKEDITOR.instances['Content'].getData();
            }
           
            obj['HomeFlag'] = $('#HomeFlag').is(':checked') ? 1 : 0;
            obj['HotFlag'] = $('#HotFlag').is(':checked') ? 1 : 0;
            obj['Status'] = $('#Status').is(':checked') ? 1 : 0;

            $.postStringify(url, obj, function (response) {
                if (response.StatusCode == 401) {
                    window.location.href = response.Data;
                }
                if (response.Success) {
                    $.notifySuccess(response);
                    if (callback != null && callback != undefined) {
                        callback(response);
                    }
                } else {
                    $('label.error').hide();
                    $('input').removeClass('error');
                    if (subValidator != null && subValidator != undefined) {
                        subValidator.showErrors(response.Errors);
                    } else {
                        validator.showErrors(response.Errors);
                    }
                    if (response.Errors == null || response.Errors == undefined) {
                        $.notifyError(response);
                    }
                }
            });
        }
    };

    $.callAjax = function (url, obj, method, callback) {
        //$.appendSpin();
        $.ajax({
            type: method,
            url: url,
            data: obj,
            dataType: 'json',
            async: false,
            success: function (response) {
                //$.removeSpin();
                if (response.StatusCode == 401) {
                    window.location.href = response.Data;
                }
                if (callback != null && callback != undefined) {
                    callback(response);
                } else if (response.Success) {
                    $.notifySuccess(response);
                } else {
                    $.notifyError(response);
                }
            },
            error: function (error) {
                $.removeSpin();
                console.log(error);
            }
        });
    };

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $.reloadData = function () {
        var url = $("#frmFilter").attr('action');
        var obj = $("#frmFilter").serializeObject();
        $.postStringify(url, obj, function (response) {
            if (response.Success) {
                $("#listContent").html(response.Data);
                $(".x_paging").html(response.Pagination);
                $.loadEventTable();
                $.loadEventPaging();
                $.initSelect2();
                $.isDisable();

                $("#listContent").initICheck();
                //Todo: load event on page
                if (window.loadPageEvent != undefined) {
                    window.loadPageEvent();
                }
                //Todo: reload other region in page
                if (window.reloadPage != undefined) {
                    window.reloadPage(response);
                }
            }
        });
    }
    /* Paging */
    $.loadEventPaging = function () {
        $('.page-size').on('change', function () {
            var number = $(this).val();
            $("#PageSize").val(number);
            $("#CurrentPage").val(1);
            $.reloadData();
        });

        $('.page-number').on('click', function () {
            var number = $(this).attr('page-number');
            $("#CurrentPage").val(number);
            $.reloadData();
        });
    }

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

    // File Excel
    $.fn.initImportFileExcel = function () {
        
        $(".btn-import").on('click', function () {
            var fileUpload = $(".fileInputExcel").get(0);
            var files = fileUpload.files;

            var $modal = $(this).parents(".modal");
            var $form = $(this).parents(".modal").find('form');

            // Create FormData object  
            var fileData = new FormData();
            // Adding one more key to FormData object  
            var lstData = $form.serializeObject();
            
            //fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }

            $.ajax({
                url: '/Admin/Product/ImportExcel',
                type: 'POST',
                data: fileData,
                beforeSend: function () {
                    $.appendSpin();
                },
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $modal.hideModal();
                    $.reloadData();
                    $.removeSpin();
                },
                error: function () {
                   $.notifyError('Has an error in progress');
                    $.removeSpin();
                }
            });
            return false;
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

$(document).ready(function () {
    $.loadEventTable();
    $.loadActiveControl();
    $.loadEventPaging();
    $.initSelect2();
    $.isDisable();
    $.initDatepicker();
    // Button Tìm kiếm
    $('.btn-search').on('click', function (e) {
        e.preventDefault();
        var $form = $(this).parents('form');
        var url = $form.attr('action');
        var obj = $form.serializeObject();
        obj["CurrentPage"] = 1;
        obj["PageSize"] = $("#PageSize").val();
        $.postStringify(url, obj, function (response) {
            if (response.Success) {
                $("#listContent").html(response.Data);
                $(".x_paging").html(response.Pagination);
                $.loadEventTable();
                $.loadActiveControl();
                $.loadEventPaging();
                $.initSelect2();
                $.isDisable();
                $("#listContent").initICheck();
                //Todo: load event on page
                if (window.loadPageEvent != undefined) {
                    window.loadPageEvent();
                }
                //Todo: reload other region in page
                if (window.reloadPage != undefined) {
                    window.reloadPage(response);
                }
            }
        });
    });

});

$(document)
    .on('shown.bs.modal', '.modal.submodal', function () {
        var backdrop = $(".modal-backdrop.in")[1];
        if (backdrop != null) {
            $(backdrop).css('z-index', "1055");
        }
    })
    .on('hidden.bs.modal', '.modal.submodal', function () {
        subValidator = null;
        $(document.body).addClass('modal-open');
        $("#subModal").html('');
    })
    .on('hidden.bs.modal', '.modal.mainmodal', function () {
        listBillDetails = [];
        formStringify = "";
    });
// On load save form current state

$(window).bind('beforeunload', function (e) {
    var obj = $("#myModal form").serializeObject();
    if (formStringify !== "" && formStringify !== JSON.stringify(obj)) {
        return true;
    }
    else e = null;
    // i.e; if form state change show box not.
});

function browseServer() {
    var finder = new CKFinder();
    finder.language = 'vi';
    finder.selectActionFunction = setFileField;
    //finder.SelectFunction = showFileInfo;
    finder.selectMultiple = true;
    finder.popup();
}

function setFileField(fileUrl) {
    document.getElementById('Image').value = fileUrl;

    $("#displayImage").attr('src', fileUrl);
}

function showFileInfo(fileUrl, file, files) {
    var valImg = "";
    for (var i = 0; i < files.length; i++) {
        valImg = valImg + files[i].url;
    }
    document.getElementById('Image').value = valImg;
}

