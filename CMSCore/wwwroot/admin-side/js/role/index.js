$(document).ready(function () {
    setTimeout(function () {
        $(".isactive ").prop('disabled', 'true');
    }, 200);
    loadTableUserEvent($("table.bulk_action tr.selected").attr('item-id'));
    $('table.bulk_action td').on('click', function () {
        var table = $(this).parents('table');
        table.removeClass("bulk_action");
        reloadListUsers();
        table.addClass("bulk_action");
    });
    $(".btn-assign-role").on('click', function () {
        var roleId = $("table.bulk_action tr.selected").attr('item-id');
        var urlGetContent = $(this).attr('url-content') + "?roleId=" + roleId;
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
    });
});

window.loadPageEvent = function () {
    setTimeout(function () {
        $(".isactive ").prop('disabled', 'true');
    }, 200);
    $('table.bulk_action td').on('click', function () {
        var table = $(this).parents('table');
        table.removeClass("bulk_action");
        reloadListUsers();
        table.addClass("bulk_action");
    });
    $(".btn-assign-role").on('click', function () {
        var roleId = $("table.bulk_action tr.selected").attr('item-id');
        var urlGetContent = $(this).attr('url-content') + "?roleId=" + roleId;
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
    });
}

window.reloadPage = function () {
    reloadListUsers();
}

window.loadMainModalEvent = function () {
    $("#list-functions").jstree({
        "core": {
            "themes": {
                "icons": false
            }
        },
        "checkbox": {
            "keep_selected_style": false
        },
        "plugins": ["checkbox"]
    });
}

function reloadListUsers() {
    var id = $("#listContent table tr.selected").attr("item-id");
    $.callAjax("/Admin/Role/GetListUsers", { id: id }, enumMethod.Post, function (response) {
        if (response.Success) {
            $("#listUsers").html(response.Data);
            loadTableUserEvent(id);
            //$.loadActiveControl();
        } else {
            $.notifyError(response);
        }
    });
}

function loadTableUserEvent(roleId) {
    
    $(".btn-assign-user").on('click', function () {
        if (roleId == undefined || roleId == null || roleId == "") {
            $.notifyWarning({
                title: 'Chú ý!',
                text: "Không có vai trò nào được chọn.",
                type: 'warning',
                styling: 'bootstrap3'
            });
        } else {
            var urlGetContent = $(this).attr('url-content') + "?roleId=" + roleId;
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

    $(".btn-remove-user").on('click', function () {
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
                            $.callAjax(urlRemove, { userId: itemId, roleId: roleId }, enumMethod.Post, function (response) {
                                if (response.Success) {
                                    $.notifySuccess(response);
                                    $("#listUsers").html(response.Data);
                                    loadTableUserEvent(roleId);
                                    $.loadActiveControl();
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
        var lstUsers = [];
        $(".user-item-check").each(function () {
            if ($(this).is(":checked")) {
                lstUsers.push($(this).parents('.user-item').attr('user-id'));
            }
        });
        console.log(lstUsers);
        if (lstUsers.length > 0) {
            var $form = $('.modal form');
            var url = $($form).attr('action');
            var roleId = $("table.bulk_action tr.selected").attr("item-id");
            var data = {
                listUsers: lstUsers,
                roleId: roleId
            }
            $.appendSpin();
            $.postStringify(url, data, function (response) {
                $.removeSpin();
                if (response.Success) {
                    $.notifySuccess(response);
                    $("#myModal").hideModal();
                    $("#listUsers").html(response.Data);
                    loadTableUserEvent(roleId);
                } else {
                    $.notifyError(response);
                }
            });
        } else {
            $.notifyWarning();
        }
    });

    $(".btn-save-role-action").on('click', function () {
        var listFunctionAction = [];
        var lstActions = $("#list-functions").jstree("get_selected", true);
        lstActions.map(function (item) {
            if (item.li_attr["fnc-id"] != undefined && item.li_attr["act-id"] != undefined) {
                listFunctionAction.push({
                    FunctionID: item.li_attr["fnc-id"],
                    ActionID: item.li_attr["act-id"]
                });
            }
        });
        var url = "/RoleManagement/SaveRoleAction";
        var data = {
            RoleID: $("#RoleID").val(),
            ListAction: listFunctionAction
        }
        if (listFunctionAction.length > 0) {
            $.appendSpin();
            $.postStringify(url, data, function (response) {
                $.removeSpin();
                if (response.Success) {
                    $.notifySuccess(response);
                    $("#myModal").hideModal();
                } else {
                    $.notifyError(response);
                }
            });
        } else {
            $.notifyWarning({
                title: 'Chú ý!',
                text: "Không có vai trò nào được chọn.",
                type: 'warning',
                styling: 'bootstrap3'
            });
        }
    });
}