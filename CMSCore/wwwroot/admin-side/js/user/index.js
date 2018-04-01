$(document).ready(function () {
    setTimeout(function () {
        $(".isactive ").prop('disabled', 'true');
    }, 200);
    loadTableRoleEvent($("table.bulk_action tr.selected").attr('item-id'));
    $('table.bulk_action td').on('click', function () {
        var table = $(this).parents('table');
        table.removeClass("bulk_action");
        reloadListRoles();
        table.addClass("bulk_action");
    });
});

window.loadPageEvent = function () {
    setTimeout(function () {
        $(".isactive ").prop('disabled', 'true');
    }, 200);
    $('table.bulk_action td').on('click', function () {
        var table = $(this).parents('table');
        table.removeClass("bulk_action");
        reloadListRoles();
        table.addClass("bulk_action");
    });
}

window.reloadPage = function () {
    reloadListRoles();
}

function reloadListRoles() {
    var id = $("#listContent table tr.selected").attr("item-id");
    $.callAjax("/Admin/User/GetListRoles", { id: id }, enumMethod.Post, function (response) {
        if (response.Success) {
            $("#listRoles").html(response.Data);
            loadTableRoleEvent(id);
            //$.loadActiveControl();
        } else {
            $.notifyError(response);
        }
    });
}

function loadTableRoleEvent(userId) {
    $('#listRoles table').find('tbody tr:first').addClass('selected');
    $('#listRoles table td').on('click', function () {
        $(this).parent().parent().find('tr').removeClass('selected');
        $(this).parent().toggleClass('selected');
    });
    $(".btn-assign-role").on('click', function () {
        if (userId == undefined || userId == null || userId == "") {
            $.notifyWarning({
                title: 'Chú ý!',
                text: "Không có người dùng nào được chọn.",
                type: 'warning',
                styling: 'bootstrap3'
            });
        } else {
            var urlGetContent = $(this).attr('url-content') + "?userId=" + userId;
            $.appendSpin();
            $.callAjax(urlGetContent, null, enumMethod.Get, function (response) {
                $.removeSpin();
                if (response.Success) {
                    $("#myModal").openModal(response.Data);
                    loadSaveEvent();
                } else {
                    $.notifyError(response);
                }
            });
        }
    });
    $(".btn-remove-role").on('click', function () {
        if ($(this).hasClass('disabled')) {
            return false;
        }
        var urlRemove = $(this).attr('url-content');
        var table = $(this).parent().parent().find('table');
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
                            $.callAjax(urlRemove, { id: itemId, userId: userId }, enumMethod.Post, function (response) {
                                if (response.Success) {
                                    $.notifySuccess(response);
                                    $("#listRoles").html(response.Data);
                                    loadTableRoleEvent(userId);
                                    //$.loadActiveControl();
                                    //$("table.bulk_action tr.selected>td").click();
                                } else {
                                    $.notifyError(response);
                                }
                            });
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
        } else {
            console.log("error");
        }
    });
}

function loadSaveEvent() {
    $(".btn-assign").on('click', function () {
        var lstRoles = [];
        $(".role-item-check").each(function () {
            if ($(this).is(":checked")) {
                lstRoles.push($(this).parents('.role-item').attr('role-id'));
            }
        });
        console.log(lstRoles);
        if (lstRoles.length > 0) {
            var $form = $('.modal form');
            var url = $($form).attr('action');
            var userId = $("table.bulk_action tr.selected").attr("item-id");
            var data = {
                listRoles: lstRoles,
                userId: userId
            }
            $.appendSpin();
            $.postStringify(url, data, function (response) {
                $.removeSpin();
                if (response.Success) {
                    $.notifySuccess(response);
                    $("#myModal").hideModal();
                    $("#listRoles").html(response.Data);
                    loadTableRoleEvent(userId);
                } else {
                    $.notifyError(response);
                }
            });
        } else {
            $.notifyWarning();
        }
    });
}

window.disabledCheckbox = function () {
    setTimeout(function () {
        $(".isactive ").prop('disabled', 'true');
    }, 200);
}