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
                                <a href="/admin/manageuser/edit?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                                     <i class="bi bi-pencil-square"></i> Edit
                                </a>
                        </div>
                    `
                    }
                    else {
                        return `
                        <div class="text-center">
                              <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-unlock-fill"></i>  UnLock
                                </a>
                                <a href="/admin/manageuser/edit?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                                     <i class="bi bi-pencil-square"></i> Edit
                                </a>
                        </div>
                    `
                    }
                },
                "width": "25%"
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

