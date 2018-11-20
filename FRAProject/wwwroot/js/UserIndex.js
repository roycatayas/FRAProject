$(document).ready(function () {
    var editUserBaseUrl = '/administration/users/EditUser';

    $('#users-table').DataTable({
        "ajax": {
            "url": "/administration/users/get-all-json",
            "type": "post"
        },
        processing: true,
        serverSide: true,
        lengthMenu: [10, 25, 50],
        searchHighlight: true,
        columns: [
            {
                data: "email",
                className: "text-center"
            },
            {
                data: "fullName",
                className: "text-center"
            },
            {
                data: "organization",
                className: "text-center"
            },
            {
                data: "phoneNumber",
                className: "text-center"
            },
            {
                data: "lockoutEnabled",
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<input type="checkbox" class="editor-active" ' + (row.lockoutEnabled ? 'checked' : '') + ' disabled />';
                    }

                    return data;
                },
                className: "text-center"
            },
            {
                data: "lockoutEndDateTimeUtc",
                className: "text-center"
            },
            {
                render: function (data, type, row) {
                    var htmlButtonEdit = '<a id="editUserModal1" data-toggle="modal" data-target="#modal-action-user-edit" data-backdrop="static"data-keyboard="false" class="btn btn-sm btn-primary" onclick=getUserToEdit("' + row.id + '");><span class="glyphicon glyphicon-pencil Padding5"></span>Edit</a>';
                    var spaceSpan = '<span class="Padding5"></span>';
                    var htmlButtonDelete = '<a id="editUserModal2" href="' + editUserBaseUrl + '/' + row.id + '" data-toggle="modal" asp-action="EditUser" data-target="#modal-action-user-edit" data-backdrop="static"data-keyboard="false" class="btn btn-sm btn-danger"><span class="glyphicon glyphicon-trash Padding5"></span>Delete</a>';
                    if (type === 'display') {
                        return htmlButtonEdit + spaceSpan + htmlButtonDelete;
                    }

                    return '';
                },
                className: "text-center CommandColWidth"
            }]
    });

});

function getUserToEdit(id) {
    $.ajax({
        url: "/administration/users/EditUser/" + id,
        type: "GET",        
        success: function (data) {            
            $("#modal-content-editUser").html(data);
            //alert(data);
        },
        error: function () {
            alert("error");
        }
    });
}

function getViewForAddUser() {
    $.ajax({
        url: "/administration/Users/AddUser",
        type: "GET",
        success: function (data) {
            $("#modal-content-addUser").html(data);
            //alert(data);
        },
        error: function () {
            alert("error");
        }
    });
}

