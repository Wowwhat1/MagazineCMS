var dataTable;

$(document).ready(function () {
    loadDataTable();
});

//Display table magazine
function loadDataTable() {
    dataTable = $('#magazine-table').DataTable({
        "ajax": { url: '/manager/managemagazine/getall' },
        "columns": [
            { "data": "name", "width": "25%", "className": "table-cell" },
            { "data": "description", "width": "20%", "className": "table-cell" },
            {
                "data": "startDate",
                "width": "12%",
                "className": "table-cell",
                "render": function (data) {
                    var date = new Date(data);
                    return date.toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit'});
                }
            },
            {
                "data": "endDate",
                "width": "12%",
                "className": "table-cell",
                "render": function (data) {
                    var date = new Date(data);
                    return date.toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit'});
                }
            },
            { "data": "faculty.name", "width": "15%", "className": "table-cell" },
            { "data": "semester.name", "width": "15%", "className": "table-cell" },
            {
                data: { id: "id" },
                "render": function (data) {
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
                },
                "width": "10%",
                "className": "table-cell"
            }
        ]
    });

    /*Table data contribution download of magazine*/
    dataTable = $('#contribution-magazine-table').DataTable({
        "ajax": { url: '/manager/managemagazine/getall' },
        "columns": [
            { "data": "name", "width": "25%", "className": "table-cell" },
            { "data": "semester.name", "width": "15%", "className": "table-cell" },
            { "data": "contributionCount", "width": "15%", "className": "table-cell" },
            { "data": "documentCount", "width": "15%", "className": "table-cell" },
            {
                data: { id: "id" },
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <button onclick="download('${data.id}')" class="btn btn-primary text-white" style="cursor:pointer; width:150px;">
                            Download
                        </button>
                `;
                },
                "width": "10%",
                "className": "table-cell"
            }
        ]
    });

    //Display semester table
    $(document).ready(function () {
        $('#SemesterId').change(function () {
            var semesterId = $(this).val();
            $.ajax({
                url: '/manager/managemagazine/getsemester',
                type: 'GET',
                success: function (data) {
                    var semester = data.data.find(s => s.id == semesterId);
                    if (semester) {
                        $('#startDate').text(semester.startDate.substring(0, 10));
                        $('#endDate').text(semester.endDate.substring(0, 10));
                    }
                }
            });
        });
    });

    //CSS to shorten data when it's too long
    var css = '.table-cell { max-width: 160px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }',
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

function download(magazineId) {
    window.location.href = '/manager/managemagazine/downloadalldocuments?magazineId=' + magazineId;
}


function updateMagazine(magazineId) {
    console.log(magazineId);
    location.href = "https://localhost:7276/Manager/ManageMagazine/updateMagazine/" + magazineId;
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
            fetch(`/manager/managemagazine/deleteMagazine/${magazineId}`, {
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