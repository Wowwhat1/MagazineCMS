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
            { "data": "description", "width": "25%", "className": "table-cell" },
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
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-info text-white" style="cursor:pointer; width:80px;">
                                <i class="bi bi-lock-fill"></i>  Edit
                            </a> 
                            <button onclick=deleteUser('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:80px;">
                                <i class="bi bi-trash-fill"></i> Delete
                            </button>
                        </div>
                    `
                },
                "width": "10%",
                "className": "table-cell"
            }
        ]
    });

    $('#SemesterId').change(function () {
        var semesterId = $(this).val();
        $.ajax({
            url: '/manager/managemagazine/getsemester/' + semesterId,
            method: 'GET',
            success: function (data) {
                console.log(data.data);
                var startDate = new Date(data.data).toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit' });
                var endDate = new Date(data.data).toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit' });
                $('#startDate').text(startDate);
                $('#endDate').text(endDate);
                
            }
        });
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