var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#magazine-table').DataTable({
        "ajax": { url: '/manager/managetopic/getall' },
        "columns": [
            { "data": "name", "width": "20%", "className": "table-cell" },
            { "data": "description", "width": "25%", "className": "table-cell" },
            {
                "data": "startdate",
                "width": "12%",
                "className": "table-cell",
                "render": function (data) {
                    var date = new Date();
                    return date.toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit'});
                }
            },
            {
                "data": "enddate",
                "width": "12%",
                "className": "table-cell",
                "render": function (data) {
                    var date = new Date();
                    return date.toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit'});
                }
            },
            { "data": "faculty.name", "width": "15%", "className": "table-cell" },
            { "data": "semester.name", "width": "15%", "className": "table-cell" },
            {
                data: { id: "id" },
                "render": function (data) {
                   /* var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:80px;">
                                <i class="bi bi-lock-fill"></i>  Edit
                            </a> 
                            <button onclick=deleteMagazine('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:80px;">
                                <i class="bi bi-trash-fill"></i> Delete
                            </button>
                        </div>
                    `;
                    } else {*/
                        return `
                        <div class="text-center">
                            <button onclick="updateMagazine('${data.id}')" class="btn btn-warning text-white" style="cursor:pointer; width:80px;">
                                <i class="bi bi-lock-fill"></i>  Edit
                            </button> 
                            <button onclick=deleteMagazine('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:80px;">
                                <i class="bi bi-trash-fill"></i> Delete
                            </button>
                        </div>
                    `;
                    //}
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

function updateMagazine(magazineId) {
    console.log(magazineId);
    location.href = "https://localhost:7276/Manager/ManageTopic/updateMagazine/" + magazineId;
}

function deleteMagazine(magazineId) {
    console.log(magazineId);
    
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
            fetch(`/manager/managetopic/deleteMagazine/${magazineId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                
                .then(response => response.json())
                
                .then(data => {
                    console.log(data);
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