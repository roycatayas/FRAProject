function getViewForAddSection() {
    $.ajax({
        url: "/administration/risksetting/AddSection",
        type: "GET",
        success: function (data) {
            $("#modal-content-Section").empty().html(data);
            $("#modal-content-Category").empty();
            $("#modal-content-riskTemplate").empty();

            makeCombox();
        },
        error: function () {
            alert("error");
        }
    });
}

function AddSection() {    
    var dataModel = {
        CategoryID: $("#oldSelection option[selected]").val(),
        SectionName: $("#SectionName").val()
    };

    $.ajax({
        url: "/administration/risksetting/AddSection",
        type: "POST",
        async: true,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {            
            getAllSection();
            $("#modal-section").modal("hide");
            $("#modal-content-Section").empty();
        },
        error: function (e) {
            alert("error " + e.responseText + " -- " + e.message);
        }
    });
}

function GetToDeleteSection(dataId) {
    $.ajax({
        url: "/administration/risksetting/DeleteSection",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { SectionID: dataId },
        success: function (data) {
            $("#modal-content-Section").empty().html(data);
            //alert(data);
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function DeleteSection() {

    var dataModel = {
        SectionName: $("#SectionName").val(),
        SectionID: $("#SectionID").val()
    };

    $.ajax({
        url: "/administration/risksetting/DeleteSection",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-section").modal("hide");
            getAllSection();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function GetToEditSection(dataId) {
    $.ajax({
        url: "/administration/risksetting/EditSection",
        type: "GET",
        dataType: "html",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: { SectionID: dataId },
        success: function (data) {
            
            $("#modal-content-Section").empty().html(data);
            
            $("#oldSelection option[value=" + $("#SelectedIndex").val() + "]").attr("selected", "selected");
            $("#selectionBoxSpan").html($("#oldSelection option[selected]").html());

            makeCombox();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function EditSection() {

    var dataModel = {
        SectionID: $("#DataId").val(),
        CategoryID: $("#oldSelection option[selected]").val(),
        SectionName: $("#SectionName").val()
    };

    $.ajax({
        url: "/administration/risksetting/EditSection",
        type: "POST",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-section").modal("hide");
            $("#Setting-container").empty().html(data);
            applySectionTable();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function makeCombox() {
    var countOption = $(".old-select option").length;

    if ($(".old-select option[selected]").length === 1) {
        $(".selection p span").empty().html($(".old-select option[selected]").html());
    }
    else {
        $(".selection p span").empty().html($(".old-select option:first-child").html());
    }

    
    $('.old-select option').each(function () {
        newValue = $(this).val();
        newHTML = $(this).html();
        $('.new-select').append('<div class="new-option" data-value="' + newValue + '" onclick=newOption("' + encodeURIComponent(newHTML) + '")><p>' + newHTML + '</p></div>');

    });

    var reverseIndex = countOption;
    $('.new-select .new-option').each(function () {
        $(this).css('z-index', reverseIndex);
        reverseIndex = reverseIndex - 1;
    });

    closeSelect(countOption);
}

function getAllSection() {
    $.ajax({
        url: "/administration/risksetting/GetSection",
        type: "GET",
        success: function (data) {

            $("#Setting-container").empty().html(data);
            applySectionTable();
        },
        error: function () {
            alert("error");
        }
    });
}

function applySectionTable() {
    $('#section-table').DataTable({
        "ajax": {
            "url": "/administration/risksetting/GetSections",
            "type": "POST"
        },
        processing: true,
        serverSide: true,
        lengthMenu: [10, 25, 50],
        searchHighlight: true,
        columns: [
            {
                data: "sectionID",
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
                render: function (data, type, row) {
                    var htmlButtonEdit = '<a id="editSection" data-toggle="modal" data-target="#modal-section" data-backdrop="static" data-keyboard="false" class="btn btn-sm btn-primary" onclick=GetToEditSection("' + row.sectionID + '");><span class="glyphicon glyphicon-pencil Padding5"></span>Edit</a>';
                    var spaceSpan = '<span class="Padding5"></span>';
                    var htmlButtonDelete = '<a id="deleteSection" data-toggle="modal" data-target="#modal-section" data-backdrop="static" data-keyboard="false" class="btn btn-sm btn-danger" onclick=GetToDeleteSection("' + row.sectionID + '");><span class="glyphicon glyphicon-trash Padding5"></span>Delete</a>';
                    if (type === 'display') {
                        return htmlButtonEdit + spaceSpan + htmlButtonDelete;
                    }

                    return '';
                },
                className: "text-center CommandColWidth"
            }]
    });
}


function onSelection() {
    $("#selectionBox").toggleClass('open');
    if ($("#selectionBox").hasClass('open') === true) {
        openSelect();
    } else {
        var countOption = $(".old-select option").length;
        closeSelect(countOption);
    }
}

function newOption(newValue) {
    //remove all %20 from string due to encodeURIComponent conversion
    newValue = newValue.replace(/%20/g, ' ');
    
    $('#selectionBoxSpan').text(newValue);
    $('#selectionBox').click();

    $("#oldSelection option").each(function () {
        if ($(this).text() === newValue) {
            $(this).attr("selected", "selected");
        } else {
            $(this).removeAttr("selected");
        }
    });            
}

function openSelect() {
    var heightSelect = $('.new-select').height();
    var j = 1;
    $('.new-select .new-option').each(function () {
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

function closeSelect(countOption) {
    var i = 0;
    $('.new-select .new-option').each(function () {
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