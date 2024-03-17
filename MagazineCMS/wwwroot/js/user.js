var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#user-table').DataTable({
        "ajax": { url: '/admin/manageuser/getall' },
        "columns": [
            { "data": "firstname", "width": "15%" },
            { "data": "lastname", "width": "15%" },
            { "data": "avatarUrl", "width": "15%" },
            { "data": "email", "width": "10%" },
            { "data": "faculty.name", "width": "10%" },
            { "data": "role", "width": "5%" },
            {
                data: "id",
                "render": function (data) {
                    return `
                    <div class="btn-group d-flex justify-content-center align-items-center" role="group" style="">
                        <a href="/admin/manageuser/edit?id=${data}" class="btn btn-outline-warning mx-2 rounded"> <i class="bi bi-pencil-square"></i>Edit</a>               
                        <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-outline-danger mx-2 rounded"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>
                    `
                },
                "width": "10%"
            }
        ]
    });
}
