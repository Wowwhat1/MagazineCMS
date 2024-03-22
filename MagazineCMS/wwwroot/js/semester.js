var dataTable;

$(document).ready(function () {
    $("#semester-table").dataTable().fnDestroy();
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#semester-table').DataTable({
        "ajax": { url: '/manager/semester/getall' },
        "columns": [
            { "data": "id", "width": "25%" },
            { "data": "name", "width": "25%" },
            { "data": "startDate", "width": "25%" },
            { "data": "endDate", "width": "25%" },
        ]
    });
}
