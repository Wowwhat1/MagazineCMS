//const { reload } = require("browser-sync");

var dataTable;
$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var i = 1;
    dataTable = $('#faculty-table').DataTable({
        "ajax": { url: '/manager/faculty/getall' },
        "columns": [
            {
                "data": "null", "width": "5%", "render": function () { return i++; }
            },
            { "data": "faculty.name", "width": "15%" },
            {
                "data": "magazineCount",
                "width": "15%",
                "className": "table-cell"
            },
            {
                "data": "userCount",
                "width": "15%",
                "className": "table-cell"
            },
            {
                data: { id: "faculty.id"},
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=editFaculty('${data.faculty.id}') class="btn btn-warning btn-icon-split" data-toggle="modal" data-target="#modalCenter">
                                <span class="icon text-white-50"><i class="fa fa-pencil""></i></span>
                                <span class="text">Edit</span>
                            </a> 
                            <a onclick=deleteFaculty('${data.faculty.id}') class="btn btn-danger btn-icon-split">
                                <span class="icon text-white-50"><i class="fa fa-trash""></i></span>
                                <span class="text">Delete</span>
                            </a> 
                        </div>
                    `
                },
                "width": "10%",
            }
        ]
    });
}

function createFaculty() {
    // Reset the form
    $('#facultyForm').trigger('reset');
    $('#facultyFormTitle').text('Create Faculty');
    $('#modalCenter').modal('show');
}

function editFaculty(id) {
    // Get the faculty data by id using an AJAX request
    $.ajax({
        url: '/manager/faculty/getbyid/' + id,
        type: 'GET',
        success: function (response) {
            var data = response.data;

            $('#facultyId').val(data.faculty.id);
            $('#facultyName').val(data.faculty.name);
            /*$('#facultyMagazineCount').val(data.magazineCount);
            $('#facultyUserCount').val(data.userCount);*/

            // Show the edit modal
            $('#facultyFormTitle').text('Edit Faculty');
            $('#modalCenter').modal('show');
        },
        error: function (xhr, status, error) {
            // Handle the error
            console.log(error);
        }
    });
}

function deleteFaculty(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this faculty!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Send DELETE request on confirmation
            fetch(`/manager/faculty/deletebyid/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        // Reload the DataTable after successful deletion
                        dataTable.ajax.reload();
                    } else {
                        Swal.fire(
                            'Error!',
                            data.message,
                            'error'
                        );
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    Swal.fire(
                        'Error!',
                        "An error occurred during deletion",
                        'error'
                    );
                });

                //dataTable.ajax.reload();
        }
    });
}

