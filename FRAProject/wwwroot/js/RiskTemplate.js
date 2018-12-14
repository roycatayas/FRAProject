$(document).ready(function () {
    getAllRiskTemplate();
});

function getAllRiskTemplate() {   

    $.ajax({
        url: "/administration/risksetting/GetRiskTemplateList",
        type: "POST",
        success: function (data) {
            $("#risktemplate-content").empty().html(data);

            $("#riskTemplate-table").DataTable();
        },
        error: function () {
            alert("error");
        }
    });   
}

function getViewForAddRiskTemplate() {
    $.ajax({
        url: "/administration/risksetting/AddRiskTemplate",
        type: "GET",
        success: function (data) {            
            $("#modal-content-risktemplate").empty().html(data);
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function AddRiskTemplate() {
    var dataModel = {
        CategoryID: $("#CategoryId").val(),
        SectionID: $("#SectionId").val(),
        TempNumber: $("#idTempNumber").val(),
        Questions: $("#idQuestions").val(),
        ControlGuidelines: $("#idControlGuidelines").val(),
        Impact: $("#idImpact").val()
    };

    $.ajax({
        url: "/administration/risksetting/AddRiskTemplate",
        type: "POST",
        async: true,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            getAllRiskTemplate();

            cancelTemplate();
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getToDeleteRiskTemplate(dataId) {
    $.ajax({
        url: "/administration/risksetting/DeleteRiskTemplate",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { TemplateID: dataId },
        success: function (data) {
            $("#modal-content-risktemplate").empty().html(data);            
        },
        error: function (e) {
            alert("error");
        }
    });
}

function DeleteRiskTemplate() {

    var dataModel = {
        TemplateID: $("#TemplateID").val()
    };

    $.ajax({
        url: "/administration/risksetting/DeleteRiskTemplate",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            getAllRiskTemplate();

            cancelTemplate();
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getToEditRiskTemplate(dataId) {    

    $.ajax({
        url: "/administration/risksetting/EditRiskTemplate",
        type: "GET",
        dataType: "html",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: { TemplateID: dataId },
        success: function (data) {
            $("#modal-content-risktemplate").empty().html(data);
         
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function EditRiskTemplate() {

    var dataModel = {
        TemplateID: $("#DataId").val(),
        CategoryID: $("#CategoryId").val(),
        SectionID: $("#SectionId").val(),
        TempNumber: $("#idTempNumber").val(),
        Questions: $("#idQuestions").val(),
        ControlGuidelines: $("#idControlGuidelines").val(),
        Impact: $("#idImpact").val()
    };

    $.ajax({
        url: "/administration/risksetting/EditRiskTemplate",
        type: "POST",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            getAllRiskTemplate();

            cancelTemplate();   
        },
        error: function (e) {
            alert("error ");
        }
    });
}


function getSectionBasedOnCategory() {

    var category = $("#CategoryId").val();

    if (category === "Choose") {
        var selecthtml = "<label class='textFWith120'>Select Section</label> <select class='custom-select textFWith483' id='SectionId'> <option selected='selected' value='1'>Select Category 1st</option> </select>";
        $("#sectionList").empty();
        $("#sectionList").html(selecthtml);
    } else {
        $.ajax({
            url: "/administration/risksetting/GetSectiocForComboboxRiskTemplate",
            type: "GET",
            dataType: "html",
            cache: false,
            contentType: "application/json; charset=utf-8",
            data: { CategoryID: $("#CategoryId").val() },
            success: function (data) {
                $("#sectionList").empty().html(data);

            },
            error: function () {
                alert("error");
            }
        });
    }
    
}

function cancelTemplate() {
    $("#modal-risktemplate").modal("hide");      
    $("#modal-content-risktemplate").empty();   
}