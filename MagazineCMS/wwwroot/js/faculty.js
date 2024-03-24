$(document).ready(function () {
    loadDataTable();
});

var i = 1;

function loadDataTable() {
    dataTable = $('#faculty-table').DataTable({
        "ajax": {
            url: '/manager/faculty/getall',
            dataSrc: 'data'
        },
        "columns": [
            {
                "data": null,
                "width": "2.5%",
                "className": "table-cell",
                "render": function (data) {
                    return i++;
                }
            },
            {
                "data": "faculty.name",
                "width": "20%",
                "className": "table-cell"
            },
            {
                "data": "magazineCount",
                "width": "20%",
                "className": "table-cell"
            },
            {
                "data": "userCount",
                "width": "20%",
                "className": "table-cell"
            },
            {
                "data": null,
                "width": "25%",
                "render": function (data) {
                    return `<button onclick="editFaculty(${data.id})" class="btn btn-primary btn-sm">Edit</button>
                            <button onclick="deleteFaculty(${data.id})" class="btn btn-danger btn-sm">Delete</button>`;
                }
            }
        ]
    });
}

function detailFaculty(id) {
    // Redirect to detail view of faculty
    window.location.href = '/manager/faculty/details/' + id;
}

function editFaculty(id) {
    // Redirect to edit view of faculty
    window.location.href = '/manager/faculty/edit/' + id;
}

function deleteFaculty(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'You are about to delete this faculty.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET", // Change POST to GET
                url: '/manager/faculty/delete/' + id,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error('Error deleting faculty');
                }
            });
        }
    });
}
