function getViewForAddCategory() {
    $.ajax({
        url: "/administration/risksetting/AddCategory",
        type: "GET",
        success: function (data) {
            $("#modal-content-Category").empty().html(data);
            $("#modal-content-Section").empty();
            $("#modal-content-riskTemplate").empty();
            //alert(data);
        },
        error: function () {
            alert("error");
        }
    });
}

function getAllCategory() {
    $.ajax({
        url: "/administration/risksetting/GetCagetory",
        type: "GET",
        success: function (data) {
            $("#Setting-container").empty().html(data);
            applyCatogoryTable();
        },
        error: function () {
            alert("error");
        }
    });
}

function AddCategory() {    
    var dataModel = {
        CategoryName: $("#CategoryName").val()
    };

    $.ajax({
        url: "/administration/risksetting/AddCategory",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            
            getAllCategory();
            $("#modal-category").modal("hide");
            $("#modal-content-Category").empty();
        },
        error: function (e) {
            alert("error " + e.responseText + " -- " + e.message);
        }
    });
};

function GetToDeleteCategory(dataId) {
    $.ajax({
        url: "/administration/risksetting/DeleteCategory",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { CategoryID: dataId },
        success: function (data) {
            $("#modal-content-Category").empty().html(data);
            //alert(data);
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function DeleteCategory() {
 
    var dataModel = {
        CategoryName: $("#CategoryName").val(),
        CategoryID: $("#CategoryID").val()
    };

    $.ajax({
        url: "/administration/risksetting/DeleteCategory",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-category").modal("hide");
            getAllCategory();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function GetToEditCategory(dataId) {
    $.ajax({
        url: "/administration/risksetting/EditCategory",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { CategoryID: dataId },
        success: function (data) {
            $("#modal-content-Category").empty().html(data);
            //alert(data);
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function EditCategory() {

    var dataModel = {
        CategoryName: $("#CategoryName").val(),
        CategoryID: $("#CategoryID").val()
    };

    $.ajax({
        url: "/administration/risksetting/EditCategory",
        type: "POST",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-category").modal("hide");
            $("#Setting-container").empty().html(data);
            applyCatogoryTable();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function applyCatogoryTable() {
    
    $('#category-table').DataTable({
        "ajax": {
            "url": "/administration/risksetting/GetCategorys",
            "type": "POST"
        },
        processing: true,
        serverSide: true,
        lengthMenu: [10, 25, 50],
        searchHighlight: true,
        columns: [
            {
                data: "categoryID",
                className: "text-center"
            },
            {
                data: "categoryName",
                className: "text-center"
            },
            {
                render: function (data, type, row) {
                    var htmlButtonEdit = '<a id="editCategory" data-toggle="modal" data-target="#modal-category" data-backdrop="static"data-keyboard="false" class="btn btn-sm btn-primary" onclick=GetToEditCategory("' + row.categoryID + '");><span class="glyphicon glyphicon-pencil Padding5"></span>Edit</a>';
                    var spaceSpan = '<span class="Padding5"></span>';
                    var htmlButtonDelete = '<a id="deleteCategory" data-toggle="modal" data-target="#modal-category" data-backdrop="static"data-keyboard="false" class="btn btn-sm btn-danger" onclick=GetToDeleteCategory("' + row.categoryID + '");><span class="glyphicon glyphicon-trash Padding5"></span>Delete</a>';
                    if (type === 'display') {
                        return htmlButtonEdit + spaceSpan + htmlButtonDelete;
                    }

                    return '';
                },
                className: "text-center CommandColWidth"
            }]
    });
}