var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#user-table').DataTable({
        "ajax": { url: '/admin/manageuser/getall' },
        "columns": [
            { "data": "firstname", "width": "15%", "className": "table-cell" },
            { "data": "lastname", "width": "15%", "className": "table-cell" },
            { "data": "avatarUrl", "width": "15%", "className": "table-cell" },
            { "data": "email", "width": "10%", "className": "table-cell" },
            { "data": "faculty.name", "width": "10%", "className": "table-cell" },
            { "data": "role", "width": "5%", "className": "table-cell" },
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
                "width": "10%",
                "className": "table-cell"
            }
        ]
    });


    //CSS to shorten data when it's too long
    var css = '.table-cell { max-width: 180px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }',
        head = document.head || document.getElementsByTagName('head')[0],
        style = document.createElement('style');

    head.appendChild(style);

    style.type = 'text/css';
    if (style.styleSheet) {
        // This is required for IE8 and below.
        style.styleSheet.cssText = css;
    } else {
        style.appendChild(document.createTextNode(css));
    }

}