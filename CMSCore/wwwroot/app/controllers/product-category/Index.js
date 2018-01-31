var productCategoryController = function () {
    var loadData = function () {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            dataType: "json",
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
                var treeArr = cms.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                //var $tree = $("#treeProductCategory");

                $("#treeProductCategory").tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$("#tt").tree("select", node.target);
                        $("#hidIdM").val(node.id);
                        // display context menu
                        $("#contextMenu").menu("show", {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree("getNode", target);
                        if (point === "append") {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });

                            //Update to database
                            $.ajax({
                                url: "/Admin/ProductCategory/UpdateParentId",
                                type: "post",
                                dataType: "json",
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }
                        else if (point === "top" || point === "bottom") {
                            $.ajax({
                                url: "/Admin/ProductCategory/ReOrder",
                                type: "post",
                                dataType: "json",
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }
                    }
                });
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
                data.push({
                    id: 0,
                    text: "Cấp cha",
                    parentId: 0,
                    sortOrder: 0
                });
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
        $("#txtOrderM").val("");
        $("#txtHomeOrderM").val("");
        $("#txtImageM").val("");

        $("#txtMetakeywordM").val("");
        $("#txtMetaDescriptionM").val("");
        $("#txtSeoPageTitleM").val("");
        $("#txtSeoAliasM").val("");

        $("#ckStatusM").prop("checked", true);
        $("#ckShowHomeM").prop("checked", false);
    }

    function registerEvents() {
        $("#frmMaintainance").validate({
            errorClass: "red",
            ignore: [],
            lang: "en",
            rules: {
                txtNameM: { required: true },
                txtOrderM: { required: true, number: true },
                txtHomeOrderM: { required: true, number: true }
            }
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
                    tedu.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    tedu.notify('There was error uploading files!', 'error');
                }
            });
        });

        $("body").on("click", "#btnCreate", function (e) {
            e.preventDefault();
            initTreeDropDownCategory();
            $("#modal-add-edit").modal("show");
        });

        $("body").on("click", "#btnEdit", function (e) {
            e.preventDefault();
            var that = $("#hidIdM").val();
            $.ajax({
                type: "GET",
                url: "/Admin/ProductCategory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    cms.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $("#hidIdM").val(data.Id);
                    $("#txtNameM").val(data.Name);
                    initTreeDropDownCategory(data.ParentId);

                    $("#txtDescM").val(data.Description);

                    $("#txtImageM").val(data.ThumbnailImage);

                    $("#txtSeoKeywordM").val(data.SeoKeywords);
                    $("#txtSeoDescriptionM").val(data.SeoDescription);
                    $("#txtSeoPageTitleM").val(data.SeoPageTitle);
                    $("#txtSeoAliasM").val(data.SeoAlias);

                    $("#ckStatusM").prop("checked", data.Status == 1);
                    $("#ckShowHomeM").prop("checked", data.HomeFlag);
                    $("#txtOrderM").val(data.SortOrder);
                    $("#txtHomeOrderM").val(data.HomeOrder);

                    $("#modal-add-edit").modal("show");
                    cms.stopLoading();

                },
                error: function (status) {
                    cms.notify("Có lỗi xảy ra", "error");
                    cms.stopLoading();
                }
            });
        });

        $("body").on("click", "#btnDelete", function (e) {
            e.preventDefault();
            var that = $("#hidIdM").val();
            cms.confirm("Are you sure to delete?", function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        cms.startLoading();
                    },
                    success: function (response) {
                        cms.notify("Deleted success", "success");
                        cms.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        cms.notify("Has an error in deleting progress", "error");
                        cms.stopLoading();
                    }
                });
            });
        });

        $("body").on("click", "#btnSave", function (e) {
            if ($("#frmMaintainance").valid()) {
                e.preventDefault();
                var id = parseInt($("#hidIdM").val());
                var name = $("#txtNameM").val();
                var parentId = $("#ddlCategoryIdM").combotree("getValue");
                if (parentId === "") {
                    parentId = 0;
                }

                var description = $("#txtDescM").val();

                var image = $("#txtImageM").val();
                var order = parseInt($("#txtOrderM").val());
                var homeOrder = $("#txtHomeOrderM").val();

                var seoKeyword = $("#txtSeoKeywordM").val();
                var seoMetaDescription = $("#txtSeoDescriptionM").val();
                var seoPageTitle = $("#txtSeoPageTitleM").val();
                var seoAlias = $("#txtSeoAliasM").val();
                var status = $("#ckStatusM").prop("checked") == true ? 1 : 0;
                var showHome = $("#ckShowHomeM").prop("checked");

                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        SortOrder: order,
                        HomeFlag: showHome,
                        LevelCate: 0,
                        Image: image,
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
                        cms.notify("Update success", "success");
                        $("#modal-add-edit").modal("hide");

                        resetFormMaintainance();

                        cms.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        cms.notify("Has an error in update progress", "error");
                        cms.stopLoading();
                    }
                });
            }
            return false;

        });

    }

    this.initialize = function () {
        loadData();
        registerEvents();
    }


}