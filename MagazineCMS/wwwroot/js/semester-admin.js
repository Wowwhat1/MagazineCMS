var dataTable;
var i = 1;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#semester-table').DataTable({
        "ajax": { url: '/admin/semester/getall' },
        "columns": [
            {
                "data": "null", "width": "5%", "render": function () { return i++; }
            },
            { "data": "name", "width": "15%" },
            {
                "data": "startDate", "width": "15%",
                "render": function (data) {
                    var date = new Date(data);
                    return date.toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' });
                }
            },
            {
                "data": "endDate", "width": "15%",
                "render": function (data) {
                    var date = new Date(data);
                    return date.toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' });
                }
            },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=editSemester('${data.id}') class="btn btn-warning btn-icon-split" data-toggle="modal" data-target="#modalCenter">
                                <span class="text">Edit</span>
                            </a> 
                            <a onclick=deleteSemester('${data.id}') class="btn btn-danger btn-icon-split">
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

function createSemester() {
    // Reset the form
    $('#semesterForm').trigger('reset');
    $('#semesterFormTitle').text('Create Semester');
    $('#modalCenter').modal('show');
}

function editSemester(id) {
    // Get the semester data by id using an AJAX request
    $.ajax({
        url: '/admin/semester/getbyid/' + id,
        type: 'GET',
        success: function (response) {
            var data = response.data;

            $('#semesterId').val(data.id);
            $('#semesterName').val(data.name);
            $('#semesterStartDate').val(data.startDate);
            $('#semesterEndDate').val(data.endDate);

            // Show the edit modal
            $('#semesterFormTitle').text('Edit Semester');
            $('#modalCenter').modal('show');
        },
        error: function (xhr, status, error) {
            // Handle the error
            console.log(error);
        }
    });
}

function deleteSemester(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this semester!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Send DELETE request on confirmation
            fetch(`/admin/semester/deletebyid/${id}`, {
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
        }
    });
}
