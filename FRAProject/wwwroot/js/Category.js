$(document).ready(function () {
    getAllCategory();
});

function getAllCategory() {
    $.ajax({
        url: "/administration/category/GetCagetoryList",
        type: "POST",
        success: function (data) {
            $("#category-content").empty().html(data);

            $("#category-table").DataTable();
        },
        error: function () {
            alert("error");
        }
    });   
}

function getViewForAddCategory() {
    $.ajax({
        url: "/administration/category/AddCategory",
        type: "GET",
        success: function (data) {
            $("#modal-content-category").empty().html(data);        
        },
        error: function () {
            alert("error");
        }
    });
}


function addCategory() {    
    var dataModel = {
        CategoryName: $("#idCategoryName").val()
    };

    $.ajax({
        url: "/administration/category/AddCategory",
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
            alert("error ");
        }
    });
};

function getCategoryToDelete(dataId) {
    $.ajax({
        url: "/administration/category/DeleteCategory",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { CategoryID: dataId },
        success: function (data) {
            $("#modal-content-category").empty().html(data);            
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function deleteCategory() {
 
    var dataModel = {
        CategoryName: $("#CategoryName").val(),
        CategoryID: $("#CategoryID").val()
    };

    $.ajax({
        url: "/administration/category/DeleteCategory",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-category").modal("hide");
            getAllCategory();
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getCategoryToEdit(dataId) {
    $.ajax({
        url: "/administration/category/EditCategory",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { CategoryID: dataId },
        success: function (data) {
            $("#modal-content-category").empty().html(data);            
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function editCategory() {

    var dataModel = {
        CategoryName: $("#idCategoryName").val(),
        CategoryID: $("#CategoryID").val()
    };

    $.ajax({
        url: "/administration/category/EditCategory",
        type: "POST",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-category").modal("hide");
            $("#modal-content-category").empty();
            getAllCategory();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}


function cancelCategory() {
    $("#modal-category").modal("hide");
    $("#modal-content-category").empty();
}