$(document).ready(function () {
    getAllSection();
});

function getAllSection() {
    $.ajax({
        url: "/administration/section/GetSectionList",
        type: "POST",
        success: function (data) {
            $("#section-content").empty().html(data);

            $("#section-table").DataTable();
        },
        error: function () {
            alert("error");
        }
    });
}

function getViewForAddSection() {
    $.ajax({
        url: "/administration/section/AddSection",
        type: "GET",
        success: function (data) {
            $("#modal-content-section").empty().html(data);
        },
        error: function () {
            alert("error");
        }
    });
}

function AddSection() {    
    var dataModel = {
        CategoryId: $("#CategoryId").val(),
        SectionName: $("#idSectionName").val()
    };

    $.ajax({
        url: "/administration/section/AddSection",
        type: "POST",
        async: true,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {            
            cancelSection();

            getAllSection();
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getSectionToEdit(dataId) {
    $.ajax({
        url: "/administration/section/EditSection",
        type: "GET",
        dataType: "html",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: { SectionID: dataId },
        success: function (data) {
            $("#modal-content-section").empty().html(data);                     
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function editSection() {

    var dataModel = {
        SectionID: $("#SectionID").val(),
        CategoryId: $("#CategoryId").val(),
        SectionName: $("#idSectionName").val()
    };

    $.ajax({
        url: "/administration/section/EditSection",
        type: "POST",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            cancelSection();

            getAllSection();
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getSectionToDelete(dataId) {
    $.ajax({
        url: "/administration/section/DeleteSection",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { sectionId: dataId },
        success: function (data) {
            $("#modal-content-section").empty().html(data);  
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function deleteSection() {

    var dataModel = {        
        SectionID: $("#SectionID").val()
    };

    $.ajax({
        url: "/administration/section/DeleteSection",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            cancelSection();

            getAllSection();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function cancelSection() {
    $("#modal-section").modal("hide");
    $("#modal-content-section").empty();
}