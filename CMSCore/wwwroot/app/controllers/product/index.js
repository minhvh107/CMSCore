var productController = function () {
    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / cms.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($("#paginationUL a").length === 0 || changePageSize === true) {
            $("#paginationUL").empty();
            $("#paginationUL").removeData("twbs-pagination");
            $("#paginationUL").unbind("page");
        }
        //Bind Pagination Event
        $("#paginationUL").twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: "Đầu",
            prev: "Trước",
            next: "Tiếp",
            last: "Cuối",
            onPageClick: function (event, p) {
                cms.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }

    var loadCategories = function () {
        var render = "<option>—Select Category—</option>";
        $.ajax({
            type: "Get",
            url: "/Admin/Product/GetAllCategory",
            dataType: "json",
            success: function (response) {
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $("#ddlCategorySearch").html(render);
            },
            error: function (status) {
                console.log(status);
                cms.notify("Can not load Data", "error");
            }
        });
    }

    var loadData = function (isPageChanged) {
        var template = $("#table-template").html();
        var render = "";
        $.ajax({
            type: "Get",
            url: "/Admin/Product/GetAllPaging",
            data: {
                categoryId: $("#ddlCategorySearch").val(),
                keyword: $("#txtKeyword").val(),
                page: cms.configs.pageIndex,
                pageSize: cms.configs.pageSize
            },
            async: true,
            dataType: "json",
            beforeSend: function () {
                cms.startLoading();
            },
            success: function (response) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image == null ? "<img src='/admin-side/images/user.png' width='25'" : "<img src='" + item.Image + "' width='25' />",
                        CategoryName: item.ProductCategory.Name,
                        Price: cms.formatNumber(item.Price, 0),
                        DateCreated: cms.dateFormatJson(item.DateCreated),
                        Status: cms.getStatus(item.Status)
                    });
                });
                $("#lblTotalRecords").text(response.RowCount);
                if (render == "") {
                    render = "<td colspan='7'>Không có dữ liệu</td>";
                }
                $("#tbl-content").html(render);

                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);

                $('#listContent').find('tbody tr:first').addClass('selected');
                cms.stopLoading();
            },
            error: function (status) {
                console.log(status);
                cms.notify("Can not load Data", "error");
            }
        });
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: "GET",
            dataType: "json",
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = cms.unflattern(data);
                $("#ddlCategoryIdM").combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $("#ddlCategoryIdM").combotree("setValue", selectedId);
                }
            }
        });
    }

    function resetFormMaintainance() {
        $("#hidIdM").val(0);
        $("#txtNameM").val("");
        initTreeDropDownCategory("");

        $("#txtDescM").val("");
        $("#txtUnitM").val("");

        $("#txtPriceM").val("0");
        $("#txtOriginalPriceM").val("");
        $("#txtPromotionPriceM").val("");

        $("#txtImageM").val("");

        $("#txtTagM").val("");
        $("#txtMetakeywordM").val("");
        $("#txtMetaDescriptionM").val("");
        $("#txtSeoPageTitleM").val("");
        $("#txtSeoAliasM").val("");

        CKEDITOR.instances.txtContent.setData("");
        $("#ckStatusM").prop("checked", true);
        $("#ckHotM").prop("checked", false);
        $("#ckShowHomeM").prop("checked", false);

    }

    var registerEvents = function () {
        $("body").on("change", "#ddlShowPage", function () {
            cms.configs.pageSize = $(this).val();
            cms.configs.pageIndex = 1;
            loadData(true);
        });

        $("body").on("click", "#btnSearch", function () {
            loadData();
        });

        $("body").on("keypress", "#txtKeyword", function (e) {
            if (e.which === 13) {
                loadData();
            }
        });

        $("body").on("click", "#btnSelectImg", function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
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
                success: function (path) {
                    $('#txtImage').val(path);
                    cms.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    cms.notify('There was error uploading files!', 'error');
                }
            });
        });

        $("body").on("click", ".btnAdd", function (e) {
            e.preventDefault();
            resetFormMaintainance();
            initTreeDropDownCategory();
            $("#modal-add-edit").modal("show");
        });

        $("body").on("click", ".btnEdit", function (e) {
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
                    url: "/Admin/Product/GetById",
                    data: { id: itemId },
                    dataType: "json",
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function (response) {
                        var data = response;
                        $("#hidIdM").val(data.Id);
                        $("#txtNameM").val(data.Name);
                        initTreeDropDownCategory(data.CategoryId);

                        $("#txtDescM").val(data.Description);
                        $("#txtUnitM").val(data.Unit);

                        $("#txtPriceM").val(data.Price);
                        $("#txtOriginalPriceM").val(data.OriginalPrice);
                        $("#txtPromotionPriceM").val(data.PromotionPrice);

                        $("#txtImageM").val(data.ThumbnailImage);

                        $("#txtTagM").val(data.Tags);
                        $("#txtMetakeywordM").val(data.SeoKeywords);
                        $("#txtMetaDescriptionM").val(data.SeoDescription);
                        $("#txtSeoPageTitleM").val(data.SeoPageTitle);
                        $("#txtSeoAliasM").val(data.SeoAlias);

                        CKEDITOR.instances.txtContent.setData(data.Content);
                        $("#ckStatusM").prop("checked", data.Status == 1);
                        $("#ckHotM").prop("checked", data.HotFlag);
                        $("#ckShowHomeM").prop("checked", data.HomeFlag);

                        $("#modal-add-edit").modal("show");
                        cms.stopLoading();

                    },
                    error: function (status) {
                        cms.notify("Có lỗi xảy ra", "error");
                        cms.stopLoading();
                    }
                });
            } else {
                console.log("error");
            }
        });

        $("body").on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var that = $(this).data("id");
            cms.confirm("Are you sure to delete?", function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function (response) {
                        cms.notify("Delete successful", "success");
                        cms.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        cms.notify("Has an error in delete progress", "error");
                        cms.stopLoading();
                    }
                });
            });
        });

        $("body").on("click", "#btnSave", function (e) {
            if ($("#frmMaintainance").valid()) {
                e.preventDefault();
                var id = $("#hidIdM").val();
                var name = $("#txtNameM").val();
                var categoryId = $("#ddlCategoryIdM").combotree("getValue");

                var description = $("#txtDescM").val();
                var unit = $("#txtUnitM").val();

                var price = $("#txtPriceM").val();
                var originalPrice = $("#txtOriginalPriceM").val();
                var promotionPrice = $("#txtPromotionPriceM").val();

                var image = $("#txtImage").val();

                var tags = $("#txtTagM").val();
                var seoKeyword = $("#txtMetakeywordM").val();
                var seoMetaDescription = $("#txtMetaDescriptionM").val();
                var seoPageTitle = $("#txtSeoPageTitleM").val();
                var seoAlias = $("#txtSeoAliasM").val();

                var content = CKEDITOR.instances.txtContent.getData();
                var status = $("#ckStatusM").prop("checked") == true ? 1 : 0;
                var hot = $("#ckHotM").prop("checked");
                var showHome = $("#ckShowHomeM").prop("checked");

                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        CategoryId: categoryId,
                        Image: image,
                        Price: price,
                        OriginalPrice: originalPrice,
                        PromotionPrice: promotionPrice,
                        Description: description,
                        Content: content,
                        HomeFlag: showHome,
                        HotFlag: hot,
                        Tags: tags,
                        Unit: unit,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    },
                    dataType: "json",
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function (response) {
                        cms.notify("Update product successful", "success");
                        $("#modal-add-edit").modal("hide");
                        resetFormMaintainance();

                        cms.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        cms.notify("Has an error in save product progress", "error");
                        cms.stopLoading();
                    }
                });
                return false;
            }
            return false;
        });
    }

    function registerControls() {
        CKEDITOR.replace('txtContent', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };

    }

    this.initialize = function () {
        loadCategories();
        loadData();
        registerEvents();
        registerControls();
    }
}