var UserController = function () {
    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
            }
        });

        $("#ddl-show-page").on('change', function () {
            cms.configs.pageSize = $(this).val();
            cms.configs.pageIndex = 1;
            loadData(true);
        });

        $('body').on('keypress', '#txtKeyword', function (e){
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        $('body').on('click', '#btn-search', function (e) {
            e.preventDefault();
            loadData();
        });

        $('body').on('click', '.btnAdd', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            initRoleList();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btnEdit', function (e) {
            e.preventDefault();
            if ($(this).hasClass('disabled')) {
                return false;
            }
            var table = $(this).parent().parent().find('table.bulk_action');
            if ($(table).find("tr.selected").length <= 0) {
                cms.notify("Không có dòng nào được chọn.", "warning");
            } else if ($(table).find("tr.selected").length == 1) {
                var itemId = $(table).find("tr.selected").attr("item-id");
                $.ajax({
                    type: "GET",
                    url: "/Admin/User/GetById",
                    data: { id: itemId },
                    dataType: "json",
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function (response) {
                        var data = response;
                        $('#hidId').val(data.Id);
                        $('#txtFullName').val(data.FullName);
                        $('#txtUserName').val(data.UserName);
                        $('#txtEmail').val(data.Email);
                        $('#txtPhoneNumber').val(data.PhoneNumber);
                        $('#ckStatus').prop('checked', data.Status === 1);

                        initRoleList(data.Roles);

                        disableFieldEdit(true);
                        $('#modal-add-edit').modal('show');
                        cms.stopLoading();

                    },
                    error: function () {
                        cms.notify('Có lỗi xảy ra', 'error');
                        cms.stopLoading();
                    }
                });
            } else {
                console.log("error");
            }
            return false;
            
        });

        $('body').on('click', '.btnDelete', function (e) {
            e.preventDefault();
            if ($(this).hasClass('disabled')) {
                return false;
            }
            var table = $(this).parent().parent().find('table.bulk_action');
            if ($(table).find("tr.selected").length <= 0) {
                cms.notify("Không có dòng nào được chọn.", "warning");
            } else if ($(table).find("tr.selected").length == 1) {
                var itemId = $(table).find("tr.selected").attr("item-id");
                cms.confirm('Are you sure to delete?', function () {
                    $.ajax({
                        type: "POST",
                        url: "/Admin/User/Delete",
                        data: { id: that },
                        beforeSend: function () {
                            cms.startLoading();
                        },
                        success: function () {
                            cms.notify('Delete successful', 'success');
                            cms.stopLoading();
                            loadData();
                        },
                        error: function () {
                            cms.notify('Has an error', 'error');
                            cms.stopLoading();
                        }
                    });
                });
            } else {
                console.log("error");
            }
            return true;
        });

        $('body').on('click', '#btnSave', function (e){
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();

                var id = $('#hidId').val();
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                var email = $('#txtEmail').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var roles = [];
                $.each($('input[name="ckRoles"]'), function (i, item) {
                    if ($(item).prop('checked') === true)
                        roles.push($(item).prop('value'));
                });
                var status = $('#ckStatus').prop('checked') === true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/User/SaveEntity",
                    data: {
                        Id: id,
                        FullName: fullName,
                        UserName: userName,
                        Password: password,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Status: status,
                        Roles: roles
                    },
                    dataType: "json",
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function () {
                        cms.notify('Save user succesful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        cms.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        cms.notify('Has an error', 'error');
                        cms.stopLoading();
                    }
                });
            }
            return false;
        });

    };

    function disableFieldEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);

    }

    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidId').val('');
        initRoleList();
        $('#txtFullName').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('input[name="ckRoles"]').removeAttr('checked');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#ckStatus').prop('checked', true);

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "/Admin/Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render += Mustache.render(template,
                        {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            }
        });
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/user/GetAllPaging",
            data: {
                keyword: $('#txtKeyword').val(),
                page: cms.configs.pageIndex,
                pageSize: cms.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                cms.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            FullName: item.FullName,
                            Id: item.Id,
                            UserName: item.UserName,
                            Avatar: item.Avatar === undefined ? '<img src="/admin-side/images/user.png" width=25 />' : '<img src="' + item.Avatar + '" width=25 />',
                            DateCreated: cms.dateFormatJson(item.DateCreated),
                            Status: cms.getStatus(item.Status)
                        });
                    });
                }
                else {
                    render = "<td colspan='5'>Không có dữ liệu</td>";
                }
                $('#tblContent').html(render);
                $("#lblTotalRecords").text(response.RowCount);
                $('#listContent').find('tbody tr:first').addClass('selected');
                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
                cms.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / cms.configs.pageSize);
        var countPage = (totalsize + 4) < 7 ? (totalsize + 4) : 7;
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: countPage,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                cms.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }

    this.initialize = function () {
        loadData();
        registerEvents();
    }
}
