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
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="bi bi-lock-fill"></i>  Lock
                            </a> 
                            <button onclick=deleteUser('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="bi bi-trash-fill"></i> Delete
                            </button>
                        </div>
                    `;
                    } else {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="bi bi-unlock-fill"></i>  UnLock
                            </a>
                            <button onclick=deleteUser('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="bi bi-trash-fill"></i> Delete
                            </button>
                        </div>
                    `;
                    }
                },
                "width": "10%",
                "className": "table-cell"
            }
        ]
    });
}

function LockUnlock(id) {
    // Display SweetAlert confirmation dialog
    Swal.fire({
        title: 'Are you sure?',
        text: 'You are about to perform this action.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed!'
    }).then((result) => {
        if (result.isConfirmed) {
            // User confirmed, make the AJAX call
            $.ajax({
                type: "POST",
                url: '/Admin/ManageUser/LockUnlock',
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                }
            });
        }
    });
}

function deleteUser(userId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Send DELETE request on confirmation
            fetch(`/admin/manageuser/deleteUser/${userId}`, {
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