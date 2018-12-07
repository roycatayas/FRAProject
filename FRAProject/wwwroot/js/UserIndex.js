$(document).ready(function () {

    getAllUser();
});

function getAllUser() {
    
    $.ajax({
        url: "/administration/users/GetUserList",
        type: "POST",
        success: function (data) {
            $("#user-content").empty().html(data);

            $("#users-table").DataTable();
        },
        error: function () {
            alert("error");
        }
    });
}

function getUserToEdit(userId) {
    $.ajax({        
        url: "/administration/users/EditUser",
        type: "GET",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: { id: userId },
        success: function (data) {            
            $("#modal-content-user").empty().html(data);
            
        },
        error: function () {
            alert("error");
        }
    });
}

function EditUser() {
    var lockoutEnabled = false;
    if ($('#idLockoutEnabled').prop('checked')) {
        lockoutEnabled = true;
    } else {
        lockoutEnabled = false;
    }

    var dataModel = {
        Id: $("#Id").val(),
        FirstName: $("#idFirstName").val(),
        LastName: $("#idLastName").val(),               
        Address: $("#idAddress").val(),
        PhoneNumber: $("#idPhoneNumber").val(),        
        ApplicationRoleId: $("#ApplicationRoleIdi").val(),
        LockoutEnabled: lockoutEnabled
    };

    $.ajax({
        url: "/administration/Users/EditUser",
        type: "POST",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {
            getAllUser();
            $("#modal-user").modal("hide");
            $("#modal-content-user").empty();
        },
        error: function (e) {
            alert("error " + e.message);
        }
    });
}

function getViewForAddUser() {
    $.ajax({
        url: "/administration/Users/AddUser",
        type: "GET",
        success: function (data) {
            $("#modal-content-user").empty().html(data);
            
        },
        error: function () {
            alert("error");
        }
    });
}

function AddUser() {

    var dataModel = {
        FirstName: $("#idFirstName").val(),
        LastName: $("#idLastName").val(),
        Email: $("#idEmail").val(),
        Password: $("#idPassword").val(),
        Address: $("#idAddress").val(),
        PhoneNumber: $("#idPhoneNumber").val(),
        Orginization: $("#idOrginization").val(),
        ApplicationRoleId: $("#ApplicationRoleId").val()
    };

    $.ajax({
        url: "/administration/Users/AddUser",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel),
        success: function (data) {

            getAllUser();
            $("#modal-user").modal("hide");
            $("#modal-content-user").empty();
        },
        error: function (e) {
            alert("error " + e.responseText + " -- " + e.message);
        }
    });
};

function cancelUser() {
    $("#modal-user").modal("hide");
    $("#modal-content-user").empty();
}