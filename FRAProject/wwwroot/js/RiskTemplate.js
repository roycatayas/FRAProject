function getViewForAddRiskTemplate() {
    $.ajax({
        url: "/administration/risksetting/AddRiskTemplate",
        type: "GET",
        success: function (data) {
            $("#modal-content-riskTemplate").empty().html(data);
            $("#modal-content-Category").empty();
            $("#modal-content-Section").empty();

            makeComboxForCategory();
            makeComboxForSection();
        },
        error: function () {
            alert("error");
        }
    });
}

function AddRiskTemplate() {
    var dataModel = {
        CategoryID: $("#oldSelectionCategory option[selected]").val(),
        SectionID: $("#oldSelectionSection option[selected]").val(),
        TempNumber: $("#TempNumber").val(),
        Questions: $("#Questions").val(),
        ControlGuidelines: $("#ControlGuidelines").val(),
        Impact: $("#Impact").val()
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
            $("#modal-riskTemplate").modal("hide");
            $("#modal-content-riskTemplate").empty();
        },
        error: function (e) {
            alert("error " + e.responseText + " -- " + e.message);
        }
    });
}

function GetToDeleteRiskTemplate(dataId) {
    $.ajax({
        url: "/administration/risksetting/DeleteRiskTemplate",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { TemplateID: dataId },
        success: function (data) {
            $("#modal-content-riskTemplate").empty().html(data);
            //alert(data);
        },
        error: function (e) {
            alert("error " + e.message);
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
            $("#modal-riskTemplate").modal("hide");
            getAllRiskTemplate();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function GetToEditRiskTemplate(dataId) {
    $("#modal-content-riskTemplate").empty();

    $.ajax({
        url: "/administration/risksetting/EditRiskTemplate",
        type: "GET",
        dataType: "html",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: { TemplateID: dataId },
        success: function (data) {

            $("#modal-content-riskTemplate").empty().html(data);

            makeComboxForCategory();
            makeComboxForSection();

            $("#oldSelectionCategory option[value=" + $("#CategoryIndex").val() + "]").attr("selected", "selected");
            $("#selectionBoxSpanCategory").html($("#oldSelectionCategory option[selected]").html());

            $("#oldSelectionSection option[value=" + $("#SectionIndex").val() + "]").attr("selected", "selected");
            $("#selectionBoxSpanSection").html($("#oldSelectionSection option[selected]").html());            
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function EditRiskTemplate() {

    var dataModel = {
        TemplateID: $("#DataId").val(),
        CategoryID: $("#oldSelectionCategory option[selected]").val(),
        SectionID: $("#oldSelectionSection option[selected]").val(),
        TempNumber: $("#TempNumber").val(),
        Questions: $("#Questions").val(),
        ControlGuidelines: $("#ControlGuidelines").val(),
        Impact: $("#Impact").val()
    };

    $.ajax({
        url: "/administration/risksetting/EditRiskTemplate",
        type: "POST",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-riskTemplate").modal("hide");
            $("#Setting-container").empty().html(data);
            applyRiskTemplateTable();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function getAllRiskTemplate() {
    $.ajax({
        url: "/administration/risksetting/GetRiskTemplate",
        type: "GET",
        success: function (data) {

            $("#Setting-container").empty().html(data);
            applyRiskTemplateTable();
        },
        error: function () {
            alert("error");
        }
    });
}

function applyRiskTemplateTable() {
    $('#riskTemplate-table').DataTable({
        "ajax": {
            "url": "/administration/risksetting/GetRiskTemplates",
            "type": "POST"
        },
        processing: true,
        serverSide: true,
        lengthMenu: [10, 25, 50],
        searchHighlight: true,
        columns: [
            {
                data: "templateID",
                className: "text-center"
            },
            {
                data: "categoryName",
                className: "text-center"
            },
            {
                data: "sectionName",
                className: "text-center"
            },
            {
                data: "tempNumber",
                className: "text-center"
            },
            {
                data: "questions",
                className: "text-center"
            },
            {
                data: "controlGuidelines",
                className: "text-center"
            },
            {
                data: "impact",
                className: "text-center"
            },
            {
                render: function (data, type, row) {
                    var htmlButtonEdit = '<a id="riskEdit" data-toggle="modal" data-target="#modal-riskTemplate" data-backdrop="static"data-keyboard="false" class="btn btn-sm btn-primary" onclick=GetToEditRiskTemplate("' + row.templateID + '");><span class="glyphicon glyphicon-pencil Padding5"></span>Edit</a>';
                    var spaceSpan = '<span class="Padding5"></span>';
                    var htmlButtonDelete = '<a id="riskDelete" data-toggle="modal" data-target="#modal-riskTemplate" data-backdrop="static"data-keyboard="false" class="btn btn-sm btn-danger" onclick=GetToDeleteRiskTemplate("' + row.templateID + '");><span class="glyphicon glyphicon-trash Padding5"></span>Delete</a>';
                    if (type === 'display') {
                        return htmlButtonEdit + spaceSpan + htmlButtonDelete;
                    }

                    return '';
                },
                className: "text-center CommandColWidth"
            }]
    });
}

// #region Category Combobox
function makeComboxForCategory() {
    var countOption = $(".old-select-category option").length;

    if ($(".old-select-category option[selected]").length === 1) {
        $("#selectionBoxSpanCategory").empty().html($(".old-select-category option[selected]").html());
    }
    else {
        $("#selectionBoxSpanCategory").empty().html($(".old-select-category option:first-child").html());
    }


    $('.old-select-category option').each(function () {
        newValue = $(this).val();
        newHTML = $(this).html();
        $('.new-select-category').append('<div class="new-option-category" data-value="' + newValue + '" onclick=newOptionForCategory("' + encodeURIComponent(newHTML) + '")><p>' + newHTML + '</p></div>');

    });

    var reverseIndex = countOption;
    $('.new-select-category .new-option-category').each(function () {
        $(this).css('z-index', reverseIndex);
        reverseIndex = reverseIndex - 1;
    });

    closeSelectCategory(countOption);
}

function newOptionForCategory(newValue) {
    //remove all %20 from string due to encodeURIComponent conversion
    newValue = newValue.replace(/%20/g, ' ');
    

    $('#selectionBoxSpanCategory').text(newValue);
    $('#selectionBoxCategory').click();

    $("#oldSelectionCategory option").each(function () {
        if ($(this).text() === newValue) {
            $(this).attr("selected", "selected");

            var categoryIndex = $(this).val(); //$("#oldSelectionCategory option[selected]").val();
            getSectionMakeCombobox(categoryIndex);

        } else {
            $(this).removeAttr("selected");
        }
    });


}

function closeSelectCategory(countOption) {
    var i = 0;
    $('.new-select-category .new-option-category').each(function () {
        $(this).removeClass('reveal');
        if (i < countOption - 3) {
            $(this).css('top', 0);
            $(this).css('box-shadow', 'none');
            $(this).css('display', 'none');
        }
        else if (i === countOption - 3) {
            $(this).css('top', '-1px');
            $(this).css('display', 'none');
        }
        else if (i === countOption - 2) {
            $(this).css({
                'top': '-1px',
                'left': '2px',
                'right': '2px',
                'display': 'none'
            });
        }
        else if (i === countOption - 1) {
            $(this).css({
                'top': '-1px',
                'left': '4px',
                'right': '4px',
                'display': 'none'
            });
        }
        i++;
    });
}

function onSelectionCategory() {
    $("#selectionBoxSection").css("z-index", "0");

    $("#selectionBoxCategory").toggleClass("open");
    if ($("#selectionBoxCategory").hasClass("open") === true) {
        openSelectForCategory();
    } else {
        var countOption = $(".old-select-category option").length;
        closeSelectCategory(countOption);
    }
}

function openSelectForCategory() {
    
    var heightSelect = $(".new-select-category").height();
    var j = 1;
    $(".new-select-category .new-option-category").each(function () {
        $(this).addClass('reveal');
        $(this).css({
            'box-shadow': '0 0px 1px rgba(0,0,0,0.1)',
            'left': '0',
            'right': '0',
            'top': j * (heightSelect + 1) + 'px',
            'display': 'inline'
        });
        j++;
    });
}
// #endregion

// #region Section Combobox
function makeComboxForSection() {
    var countOption = $(".old-select-section option").length;

    if ($(".old-select-section option[selected]").length === 1) {
        $("#selectionBoxSpanSection").empty().html($(".old-select-section option[selected]").html());
    }
    else {
        $("#selectionBoxSpanSection").empty().html($(".old-select-section option:first-child").html());
    }


    $('.old-select-section option').each(function () {
        newValue = $(this).val();
        newHTML = $(this).html();
        $('.new-select-section').append('<div class="new-option-section" data-value="' + newValue + '" onclick=newOptionForSection("' + encodeURIComponent(newHTML) + '")><p>' + newHTML + '</p></div>');

    });

    var reverseIndex = countOption;
    $('.new-select-section .new-option-section').each(function () {
        $(this).css("z-index", reverseIndex);
        reverseIndex = reverseIndex - 1;
    });

    closeSelectForSection(countOption);
}

function closeSelectForSection(countOption) {
    var i = 0;
    $(".new-select-section .new-option-section").each(function () {
        $(this).removeClass("reveal");
        if (i < countOption - 3) {
            $(this).css("top", 0);
            $(this).css("box-shadow", 'none');
            $(this).css("display", 'none');
        }
        else if (i === countOption - 3) {
            $(this).css("top", '-1px');
            $(this).css("display", 'none');
        }
        else if (i === countOption - 2) {
            $(this).css({
                'top': '-1px',
                'left': '2px',
                'right': '2px',
                'display': 'none'
            });
        }
        else if (i === countOption - 1) {
            $(this).css({
                'top': '-1px',
                'left': '4px',
                'right': '4px',
                'display': 'none'
            });
        }
        i++;
    });
}

function newOptionForSection(newValue) {
    //remove all %20 from string due to encodeURIComponent conversion
    newValue = newValue.replace(/%20/g, ' ');

    $('#selectionBoxSpanSection').text(newValue);
    $('#selectionBoxSection').click();

    $("#oldSelectionSection option").each(function () {
        if ($(this).text() === newValue) {
            $(this).attr("selected", "selected");
        } else {
            $(this).removeAttr("selected");
        }
    });
}

function openSelectForSection() {
    var heightSelect = $(".new-select-section").height();
    var j = 1;
    $(".new-select-section .new-option-section").each(function () {
        $(this).addClass('reveal');
        $(this).css({
            'box-shadow': '0 0px 1px rgba(0,0,0,0.1)',
            'left': '0',
            'right': '0',
            'top': j * (heightSelect + 1) + 'px',
            'display': 'inline',
            'white-space': 'nowrap'
        });
        j++;
    });
}

function onSelectionSection() {
    $("#selectionBoxSection").toggleClass("open");
    if ($("#selectionBoxSection").hasClass("open") === true) {
        openSelectForSection();
    } else {
        var countOption = $(".old-select-section option").length;
        closeSelectForSection(countOption);
    }
}
// #endregion

function getSectionMakeCombobox(dataId) {
    $.ajax({
        url: "/administration/risksetting/GetSectiocForComboboxRiskTemplate",
        type: "GET",
        dataType: "html",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: { CategoryID: dataId },
        success: function (data) {
            $("#sectionList").empty().html(data);

            makeComboxForSection();
        },
        error: function () {
            alert("error");
        }
    });
}