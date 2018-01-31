var RoleController = function () {
    var self = this;

    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true }
            }
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                loadData();
            }
        });
        $("#btn-search").on('click', function () {
            loadData();
        });

        $("#ddl-show-page").on('change', function () {
            cms.configs.pageSize = $(this).val();
            cms.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Role/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    cms.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtDescription').val(data.Description);
                    $('#modal-add-edit').modal('show');
                    cms.stopLoading();

                },
                error: function (status) {
                    cms.notify('Có lỗi xảy ra', 'error');
                    cms.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var name = $('#txtName').val();
                var description = $('#txtDescription').val();

                $.ajax({
                    type: "POST",
                    url: "/Admin/Role/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function (response) {
                        cms.notify('Update role successful', 'success');
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
                return false;
            }

        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            cms.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Role/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function (response) {
                        cms.notify('Delete successful', 'success');
                        cms.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        cms.notify('Has an error in deleting progress', 'error');
                        cms.stopLoading();
                    }
                });
            });
        });


    };

    function resetFormMaintainance() {
        $('#hidId').val('');
        $('#txtName').val('');
        $('#txtDescription').val('');
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/role/GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
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
                            Name: item.Name,
                            Id: item.Id,
                            Description: item.Description
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $('#tbl-content').html('');
                }
                cms.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / cms.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
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
}