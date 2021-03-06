﻿$(document).ready(function () {

    $.ajaxSetup({ cache: false });
    getAllRiskAssessment();

    $("#date-container1 .input-group.date").datepicker({
        autoclose: true
    });

    $("#date-container2 .input-group.date").datepicker({
        autoclose: true
    });
});

function getViewForAddRisk() {
    $.ajax({
        url: "/administration/riskmanagement/AddRisk",
        type: "GET",
        success: function (data) {
            $("#modal-content-riskManagement").empty().html(data);
            $("#SurveyDate").val("");
            $("#EntryDate").val("");
           
            $("#date-container1 .input-group.date").datepicker({
                autoclose: true
            });

            $("#date-container2 .input-group.date").datepicker({
                autoclose: true
            });
        },
        error: function () {
            alert("error");
        }
    });
}

function AddRiskAssessment() {
    var dataModel = {
        SubjectTitle: $("#SubjectTitle").val(),
        Organization: $("#Organization").val(),
        EntryDate: $("#EntryDate").val(),
        Owner: $("#Owner").val(),
        DocumentNo: $("#DocumentNo").val(),
        RevisionNo: "1.0",
        ApprovedBy: "",
        SurveyorName: $("#SurveyorName").val(),
        SurveyorTelephone: $("#SurveyorTelephone").val(),
        SurveyorEmail: $("#SurveyorEmail").val(),
        SurveyDate: $("#SurveyDate").val(),
        SiteName: $("#SiteName").val(),
        SiteCountry: $("#SiteCountry").val(),
        SiteAdress: $("#SiteAdress").val(),
        SiteStateProvince: $("#SiteStateProvince").val(),
        ContactPersonName: $("#ContactPersonName").val(),
        ContactPersonTelephone: $("#ContactPersonTelephone").val(),
        ContactPersonFaxNumber: $("#ContactPersonFaxNumber").val(),
        ContactPersonEmail: $("#ContactPersonEmail").val(),
        ContactPersonWebsiteUrl: $("#ContactPersonWebsiteUrl").val()
    };

    $.ajax({
        url: "/administration/riskmanagement/AddRisk",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

            getAllRiskAssessment();
            //$("#modal-riskManagement").modal("hide");
            //$("#modal-content-riskManagement").empty();
            ClearRiskManagementDOM();
        },
        error: function (e) {
            alert("error " );
        }
    });
};

function getRiskAssessmentToEdit(dataId) {
    $.ajax({
        url: "/administration/riskmanagement/EditRisk",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { RiskAssessmentID: dataId },
        success: function (data) {
            $("#modal-content-riskManagement").empty().html(data);                           

            $("#date-container1 .input-group.date").datepicker({
                autoclose: true
            });

            $("#date-container2 .input-group.date").datepicker({
                autoclose: true
            });
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function getRiskAssessmentToDelete(dataId) {    
    $.ajax({
        url: "/administration/riskmanagement/DeleteRisk",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { RiskAssessmentID: dataId },
        success: function (data) {
            $("#modal-content-riskDetail").empty().html(data);
        },
        error: function () {
            alert("error");
        }
    });
}

function deleteRiskAssessment() {

    var dataModel = {        
        RiskAssessmentID: $("#RiskAssessmentID").val()
    };

    $.ajax({
        url: "/administration/riskmanagement/DeleteRisk",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-riskDetail").modal("hide");
            getAllRiskAssessment();

        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getAllRiskAssessment() {
    $.ajax({
        url: "/administration/riskmanagement/GetRiskAssessment",
        type: "POST",
        success: function (data) {
            $("#risk-content").empty().html(data);

            $(".itemdetail").on("hidden.bs.collapse", function (event) {
                event.stopPropagation();                 
                $(".fas").addClass("fa-caret-up").removeClass("fa-caret-down");
            });
            $(".itemdetail").on("show.bs.collapse", function (event) {               
                event.stopPropagation();
                $(".fas").addClass("fa-caret-down").removeClass("fa-caret-up");
            });

            const items = document.querySelectorAll(".accordion a");

            for (var i = 0, len = items.length; i < len; i++) {
                items[i].addEventListener('click', toggleAccordion);
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function AddRiskDeitailScore(param) {
    
    var selectIdText = "#riskSelect_" + param;
    var inputId = "#riskParticipant_" + param;

    var dataModel = {
        RiskAssessmentID: param,
        CategoryID: $(selectIdText).val(),
        CategoryName: $(selectIdText).find(":selected").text(),
        ParticipantsNo: $(inputId).val()
    };

    $.ajax({
        url: "/administration/riskmanagement/AddRiskDeitailScore",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            getRiskDetailScore(dataModel);
        },
        error: function (e) {
            alert("error " + e.responseText + " -- " + e.message);
        }
    });
}

function getRiskDetailScore(dataModel) {
    var sectionDetailScore = "#ContainerDetailScore_" + dataModel.RiskAssessmentID;
    $.ajax({
        url: "/administration/riskmanagement/GetRiskDetailScore",
        type: "POST",
        data: JSON.stringify(dataModel),
        contentType: "application/json; charset=utf-8",
        success: function (data) {            
            $(sectionDetailScore).empty().html(data);
            
            const items = document.querySelectorAll(".accordion a");

            for (var i = 0, len = items.length; i < len; i++) {
                items[i].addEventListener('click', toggleAccordion);
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function getRiskDetailToEdit(param) {
    
    var detectValueId = param.split("_");

    var dataModel = {
        RiskAssessmentID: detectValueId[0],
        RiskDetailsID: detectValueId[1]
    };

    $.ajax({
        url: "/administration/riskmanagement/GetRiskGuidelinesScoreEdit",
        type: "POST",
        data: JSON.stringify(dataModel),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#modal-content-riskGuideLine").empty().html(data);
            
            const items = document.querySelectorAll(".accordion a");

            for (var i = 0, len = items.length; i < len; i++) {
                items[i].addEventListener("click", toggleAccordion);
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function getRiskDetailToDelete(param) {

    var detectValueId = param.split("_");

    $.ajax({
        url: "/administration/riskmanagement/DeleteRiskDeitailScore",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { RiskDetailsID: detectValueId[1], RiskAssessmentID: detectValueId[0] },        
        success: function (data) {
            $("#modal-content-riskDetail").empty().html(data);
            
        },
        error: function () {
            alert("error");
        }
    });
}

function deleteRiskDetail() {

    var dataModel = {
        RiskDetailsID: $("#RiskDetailsID").val(),
        RiskAssessmentID: $("#RiskAssessmentID").val()
    };

    $.ajax({
        url: "/administration/riskmanagement/DeleteRiskDeitailScore",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            $("#modal-riskDetail").modal("hide");
            getAllRiskAssessment();

        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getViewForAddContactPerson(param) {

    $.ajax({
        url: "/administration/riskmanagement/AddContactPerson",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { RiskAssessmentID: param },
        success: function (data) {
            $("#modal-content-riskDetail").empty().html(data);
            
        },
        error: function () {
            alert("error");
        }
    });
}

function AddContactPerson() {
    var dataModel = {
        RiskAssessmentID: $("#RiskAssessmentID").val(),
        FullName: $("#idFullName").val(),
        PhoneNumber: $("#idPhoneNumber").val()
    };

    $.ajax({
        url: "/administration/riskmanagement/AddContactPerson",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            getContactPerson(dataModel);

            ClearRiskDetailDOM();
        },
        error: function (e) {
            alert("error ");
        }
    });
};

function getContactPerson(dataModel) {
    var contactDetail = "#ContactDetail_" + dataModel.RiskAssessmentID;
    $.ajax({
        url: "/administration/riskmanagement/GetContactPersonList",
        type: "POST",
        data: JSON.stringify(dataModel),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(contactDetail).empty().html(data);
            
            const items = document.querySelectorAll(".accordion a");

            for (var i = 0, len = items.length; i < len; i++) {
                items[i].addEventListener("click", toggleAccordion);
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function getContactPersonToDelete(param) {

    var detectValueId = param.split("_");

    $.ajax({
        url: "/administration/riskmanagement/DeleteContactPerson",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { RiskAssessmentID: detectValueId[0], ContactPersonID: detectValueId[1] },
        success: function (data) {
            $("#modal-content-riskDetail").empty().html(data);
        },
        error: function () {
            alert("error");
        }
    });
}

function deleteContactPerson() {

    var dataModel = {
        ContactPersonID: $("#ContactPersonID").val(),
        RiskAssessmentID: $("#RiskAssessmentID").val()
    };

    $.ajax({
        url: "/administration/riskmanagement/DeleteContactPerson",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            
            var personModel = {
                RiskAssessmentID: $("#RiskAssessmentID").val()
            };
            getContactPerson(personModel);

            ClearRiskDetailDOM();

        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getFileToUpload(param) {    

    $.ajax({
        url: "/administration/riskmanagement/Upload",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { dataId: param },
        success: function (data) {
            $("#modal-content-riskDetail").empty().html(data);
        },
        error: function () {
            alert("error");
        }
    });
}

function uploadFile() {

    var detectValueId = $("#DataId").val();
    var documentName = $("#idDocumentName").val(); 
    var fileToUpload = $("#FormControlFile")[0].files[0];

    var dataModel = {
        RiskAssessmentID: $("#DataId").val()
    };

    var formData = new FormData();
    formData.append("file", fileToUpload);
    formData.append("RiskId", detectValueId);
    formData.append("documentName", documentName);

    $("#loader").show();
    $("#inputControl").hide();
    $("#modelFooterControl").hide();
    $.ajax({
            url: "/administration/riskmanagement/Upload",
            type: "POST",
            contentType: false,
            data: formData,
            cache: false,
            processData: false,
            success: function(file) {
                //alert("Uploading is done");
                //$("#loader").hide();
                getDocument(dataModel);

                ClearRiskDetailDOM();
            },
            xhr: function() {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress",
                    function(evt) {
                        if (evt.lengthComputable) {                                                        
                            
                        }
                    },
                    false);
                return xhr;
            }
        })
        .fail(function(jqXhr, textStatus, errorThrown) {
            if (errorThrown === "abort") {
                alert("Uploading was aborted");
            } else {
                alert("Uploading failed");
            }
        });
}

function getDocument(dataModel) {
    var documentDetail = "#DocumentDetail_" + dataModel.RiskAssessmentID;
    $.ajax({
        url: "/administration/riskmanagement/GetDocumentList",
        type: "POST",
        data: JSON.stringify(dataModel),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(documentDetail).empty().html(data);

            const items = document.querySelectorAll(".accordion a");

            for (var i = 0, len = items.length; i < len; i++) {
                items[i].addEventListener("click", toggleAccordion);
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function getDocumentToDelete(param) {

    var detectValueId = param.split("_");

    $.ajax({
        url: "/administration/riskmanagement/DeleteDocument",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { RiskAssessmentID: detectValueId[0], DocId: detectValueId[1] },
        success: function (data) {
            $("#modal-content-riskDetail").empty().html(data);
        },
        error: function () {
            alert("error");
        }
    });
}

function deleteDocument() {

    var dataModel = {
        DocId: $("#DocId").val(),
        RiskAssessmentID: $("#RiskAssessmentID").val()
    };

    $("#loader").show();
    $("#modelFooterControl").hide();
    $("#deleteMessage").hide();

    $.ajax({
        url: "/administration/riskmanagement/DeleteDocument",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

            var docModel = {
                RiskAssessmentID: $("#RiskAssessmentID").val()
            };
            getDocument(docModel);

            ClearRiskDetailDOM();

        },
        error: function (e) {
            alert("error ");
        }
    });
}

function getDocumentToDownload(param) {

    var detectValueId = param.split("_");

    $.ajax({
        url: "/administration/riskmanagement/DownloadFile",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { RiskAssessmentID: detectValueId[0], DocId: detectValueId[1] },
        success: function (data) {
            $("#modal-content-riskDetail").empty().html(data);

            downloadDocument();
        },
        error: function () {
            alert("error");
        }
    });
}

function downloadDocument() {

    var dataModel = {
        DocId: $("#DocId").val(),
        RiskAssessmentID: $("#RiskAssessmentID").val()
    };

    $.ajax({
        url: "/administration/riskmanagement/DownloadFile",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {            

            ClearRiskDetailDOM();
        },
        error: function (e) {
            alert("error ");
        }
    });
}

function ClearRiskDetailDOM() {
    $("#modal-riskDetail").modal("hide");
    $("#modal-content-riskDetail").empty();
}

function ClearRiskManagementDOM() {
    $("#modal-riskManagement").modal("hide");
    $("#modal-content-riskManagement").empty();
}

function ClearRiskGuideLineDOM() {
    $("#modal-riskGuideLine").modal("hide");
    $("#modal-content-riskGuideLine").empty();
}

function toggleAccordion() {
    this.classList.toggle('active');
    this.nextElementSibling.classList.toggle('active');
}

function toggleCaret() {
    //this.classList.toggle('active');
    //this.nextElementSibling.classList.toggle('active');
    console("clik");
    this.on("hidden.bs.collapse", function (event) {
        event.stopPropagation();
        $(".fas").addClass("fa-caret-up").removeClass("fa-caret-down");
    });
    this.on("show.bs.collapse", function (event) {
        event.stopPropagation();
        $(".fas").addClass("fa-caret-down").removeClass("fa-caret-up");
    });
}

function calculateMaturityOnly(sectionImpactParam) {
    //RiskSectionScoreID = index 1
    //RiskGuidelinesScoreID = index 2
    //GuidelinesNo = index 3
    var detectValue = sectionImpactParam.split("_");

    var participantNo = detectValue[5];
    var fullId = detectValue[0] + "_" + detectValue[1] + "_" + detectValue[2] + "_" + detectValue[3];

    var totalPaticipantScore = 0;
    for (var i = 1; i <= participantNo; i++) {
        var selectName = "#" + fullId + "_p" + i;
        var currentValueParticipant = $(selectName).val();
        totalPaticipantScore = parseFloat(totalPaticipantScore) + parseFloat(currentValueParticipant);
    }

    var impactName = "impact" + fullId;
    var impactScore = $("#" + impactName).find(":selected").text();
    var maturityScore = 0;    

    maturityScore = (((parseFloat(totalPaticipantScore) / parseFloat(participantNo)) * parseFloat(impactScore))) / parseFloat(5);
    changeColor("maturityScore1" + fullId, maturityScore);

    $("#maturityScore1" + fullId).text(Math.round10(maturityScore, -1));

    calculateMaturitySection(sectionImpactParam);

    var fullMautiryId = detectValue[1] + "_" + detectValue[2] + "_" + detectValue[3];
    var riskGuidelinesScoreId = $("#hiddenMaturityScoreId_" + fullMautiryId).text();

    var dataModel3 = {
        RiskGuidelinesScoreID: riskGuidelinesScoreId,
        MaturityEarned: $("#maturityScore1" + fullId).text(),
        Impact: impactScore,
        Comments: $("#commentSection_" + fullMautiryId).val()
    };

    UpdateRiskGuidelinesScore(dataModel3);
}

function UpdateRiskGuidelinesScore(dataModel) {
    $.ajax({
        url: "/administration/riskmanagement/UpdateRiskGuidelinesScore",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

        },
        error: function (e) {
            alert("error ");
        }
    });
};

function UpdateRiskGuidelinesComment(param) {
    var detectValue = param.split("_");
    console.log(param);
    var dataModel = {
        RiskGuidelinesScoreID: detectValue[2],
        Comments: $("#" + param).val()
    };
    
    $.ajax({
        url: "/administration/riskmanagement/UpdateRiskGuidelinesComment",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

        },
        error: function (e) {
            alert("error ");
        }
    });
};

function calculateImpact(sectionImpactParam) {
    //RiskSectionScoreID = index 1
    //RiskGuidelinesScoreID = index 2
    //GuidelinesNo = index 3
    var detectValue = sectionImpactParam.split("_");    

    var participantNo = detectValue[5];
    var fullId = detectValue[0] + "_" + detectValue[1] + "_" + detectValue[2] + "_" + detectValue[3];
    
    var totalPaticipantScore = 0;
    for (var i = 1; i <= participantNo; i++) {
        var selectName = "#" + fullId + "_p" + i;
        var currentValueParticipant = $(selectName).val();
        
        totalPaticipantScore = parseFloat(totalPaticipantScore) + parseFloat(currentValueParticipant);        
    }    
    
    var impactName = "impact" + fullId;
    var impactScore = $("#" + impactName).find(":selected").text();
    var maturityScore = 0;
    
    var rFullId = fullId + "_" + detectValue[4];
    changeColor("div" + rFullId, $("#" + rFullId).find(":selected").text());
   
    maturityScore = (((parseFloat(totalPaticipantScore) / parseFloat(participantNo)) * parseFloat(impactScore))) / parseFloat(5);    
    changeColor("maturityScore1" + fullId, maturityScore);

    $("#maturityScore1" + fullId).text(Math.round10(maturityScore, -1));    

    calculateMaturitySection(sectionImpactParam);

    var fullIdParticipant = detectValue[1] + "_" + detectValue[2] + "_" + detectValue[3] + "_" + detectValue[4];
    var participantsId = $("#hiddenParticipant_" + fullIdParticipant).text();    

    var dataModel1 = {
        RiskParticipantsID: participantsId,
        ParticipantScore: $("#" + rFullId).find(":selected").text()
    };

    UpdateRiskParticipantsScore(dataModel1);

    var fullMautiryId = detectValue[1] + "_" + detectValue[2] + "_" + detectValue[3];
    var riskGuidelinesScoreId = $("#hiddenMaturityScoreId_" + fullMautiryId).text();

    var dataModel2 = {
        RiskGuidelinesScoreID: riskGuidelinesScoreId,
        MaturityEarned: $("#maturityScore1" + fullId).text(),
        Impact: impactScore,
        Comments: $("#commentSection_" + fullMautiryId).val()
    };

    UpdateRiskGuidelinesScore(dataModel2);
};

function UpdateRiskParticipantsScore(dataModel) {
    $.ajax({
        url: "/administration/riskmanagement/UpdateRiskParticipantsScore",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

        },
        error: function (e) {
            alert("error ");
        }
    });
};

function calculateMaturitySection(sectionImpactParam) {
    var detectValue = sectionImpactParam.split("_");

    var sectionId = detectValue[1];
    var totalRecord = $("#totalRecord_" + sectionId).text();
    var hiddenIds = $("#hiddenID_" + sectionId).text();
    var hiddenMinRecord = $("#MinRecord_" + sectionId).text();
    var hiddenMaxRecord = $("#MaxRecord_" + sectionId).text();

    var fullId = detectValue[0] + "_" + detectValue[1];
    var totalmaturityEarned = 0;
    
    var responseCounter = 1;
    for (var i = parseInt(hiddenMinRecord); i <= hiddenMaxRecord; i++) {        
        var maturityEarned = $("#maturityScore1" + fullId + "_" + i + "_" + responseCounter).text();
        
        totalmaturityEarned = parseFloat(totalmaturityEarned) + parseFloat(maturityEarned);
        responseCounter = responseCounter + 1;
    }

    totalmaturityEarned = parseFloat(totalmaturityEarned) / parseFloat(totalRecord);
    $("#sectionMaturity_" + hiddenIds).text(Math.round10(totalmaturityEarned, -1));

    var sectionEfficiency = (Math.round10(totalmaturityEarned, -1) / 5) * 100;
    $("#sectionEfficiency_" + hiddenIds).text(Math.round10(sectionEfficiency, -1) + "%");

    changeColor("sectionMaturity_" + hiddenIds, Math.round10(totalmaturityEarned, -1));
    changeColorEfficiency("sectionEfficiency_" + hiddenIds, Math.round10(sectionEfficiency, -1));
    changeColorRiskLevel("sectionRiskLevel_" + hiddenIds, Math.round10(totalmaturityEarned, -1));

    validateBeforeUpdateSection(hiddenIds, totalmaturityEarned, sectionEfficiency);

    calulateMaturityRiskDetail(hiddenIds);    
};

function validateBeforeUpdateSection(param, totalmaturityEarned, sectionEfficiency) {
    //RiskAssessmentID = index 0
    //RiskDetailsID = index 1
    //RiskSectionScoreID = index 2

    var detectValue = param.split("_");
    var fullId = detectValue[0] + "_" + detectValue[1];

    var score = Math.round10(totalmaturityEarned, -1);
    var scoreEfficiency = Math.round10(sectionEfficiency, -1);
    var scoreResult = "HIGH";
    if (score >= 0 && score <= 2) {
        scoreResult = "HIGH";
    }
    else if (score > 2 && score <= 4) {
        scoreResult = "MEDIUM";
    }
    else if (score > 4 && score <= 5) {
        scoreResult = "LOW";
    }

    var dataModel2 = {
        RiskSectionScoreID: detectValue[2],
        RiskDetailsID: detectValue[1],
        RiskAssessmentID: detectValue[0],
        Maturity: score,
        Efficiency: scoreEfficiency,
        RiskLevel: scoreResult
    };

    UpdateRiskSectionScore(dataModel2);
};

function UpdateRiskSectionScore(dataModel) {
    $.ajax({
        url: "/administration/riskmanagement/UpdateRiskSectionScore",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

        },
        error: function (e) {
            alert("error ");
        }
    });
};

function calulateMaturityRiskDetail(param) {
    //RiskAssessmentID = index 0
    //RiskDetailsID = index 1
    //RiskSectionScoreID = index 2

    var detectValue = param.split("_");
    var fullId = detectValue[0] + "_" + detectValue[1];
    
    var totalSectionRecord = $("#SectionTotalRecord_" + fullId).text();
    var riskDetailsId = $("#RiskDetailsID_" + fullId).text();
    var hiddenMinRecord = $("#SectionMinRecord_" + fullId).text();
    var hiddenMaxRecord = $("#SectionMaxRecord_" + fullId).text();

    var totalmaturityEarned = 0;
    
    for (var i = parseInt(hiddenMinRecord); i <= parseInt(hiddenMaxRecord); i++) {
        var maturityEarned = $("#sectionMaturity_" + fullId + "_" + i).text();
        totalmaturityEarned = parseFloat(totalmaturityEarned) + parseFloat(maturityEarned);  
        console.log(totalmaturityEarned);
    }

    totalmaturityEarned = parseFloat(totalmaturityEarned) / parseFloat(totalSectionRecord);
    $("#Maturity" + fullId).text(Math.round10(totalmaturityEarned, -1));
    changeColor("divMaturity" + fullId, Math.round10(totalmaturityEarned, -1));

    var sectionEfficiency = (Math.round10(totalmaturityEarned, -1) / 5) * 100;
    $("#Efficiency" + fullId).text(Math.round10(sectionEfficiency, -1) + "%");
    changeColorEfficiency("divEfficiency" + fullId, Math.round10(sectionEfficiency, -1));

    changeColorRiskLevel2("RiskLevel" + fullId, Math.round10(totalmaturityEarned, -1));   

    var dataModel1 = {
        RiskDetailsID: riskDetailsId,
        Maturity: $("#Maturity" + fullId).text(),
        Efficiency: Math.round10(sectionEfficiency, -1),
        RiskLevel: $("#RiskLevel" + fullId).text()
    };

    console.log(dataModel1);
    UpdateRiskDetailScore(dataModel1);
};

function UpdateRiskDetailScore(dataModel) {
    $.ajax({
        url: "/administration/riskmanagement/UpdateRiskDetailScore",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

        },
        error: function (e) {
            alert("error ");
        }
    });
};

function changeColor(control, score) {
    if (score >= 0 && score <= 1) {
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("red");
    }
    else if (score > 1 && score <= 2) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("orange");
    }
    else if (score > 2 && score <= 3) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("yellow");
    }
    else if (score > 3 && score <= 3.8) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("green lighten-2");
    }
    else if (score > 3.8 && score <= 5) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).addClass("green");
    }
};

function changeColorEfficiency(control, score) {
    if (score >= 0 && score <= 20) {
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("red");
    }
    else if (score > 20 && score <= 40) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("orange");
    }
    else if (score > 40 && score <= 60) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("yellow");
    }
    else if (score > 60 && score <= 80) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("green lighten-2");
    }
    else if (score > 80 && score <= 100) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("orange");
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green lighten-2");
        $("#" + control).addClass("green");
    }
};

function changeColorRiskLevel(control, score) {
    if (score >= 0 && score <= 2) {
        $("#" + control).removeClass("yellow");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("red");
        $("#" + control).text("HIGH");
    }
    else if (score > 2 && score <= 4) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("green");
        $("#" + control).addClass("yellow");
        $("#" + control).text("MEDIUM");
    }
    else if (score > 4 && score <= 5) {
        $("#" + control).removeClass("red");
        $("#" + control).removeClass("yellow");
        $("#" + control).addClass("green");
        $("#" + control).text("LOW");
    }    
};

function changeColorRiskLevel2(control, score) {
    if (score >= 0 && score <= 2) {
        $("#div" + control).removeClass("yellow");
        $("#div" + control).removeClass("green");
        $("#div" + control).addClass("red");
        $("#" + control).text("HIGH");
    }
    else if (score > 2 && score <= 4) {
        $("#div" + control).removeClass("red");
        $("#div" + control).removeClass("green");
        $("#div" + control).addClass("yellow");
        $("#" + control).text("MEDIUM");
    }
    else if (score > 4 && score <= 5) {
        $("#div" + control).removeClass("red");
        $("#div" + control).removeClass("yellow");
        $("#div" + control).addClass("green");
        $("#" + control).text("LOW");
    }
};
