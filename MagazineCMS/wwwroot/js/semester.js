var dataTable;
var i = 1;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#semester-table').DataTable({
        "ajax": { url: '/manager/semester/getall' },
        "columns": [
            {
                "data": "null", "width": "5%", "render": function () { return i++; }    
            },
            { "data": "name", "width": "15%" },
            {
                "data": "startDate", "width": "15%", "render": function (data) {
                    var date = new Date();
                    return date.toLocaleString({ year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' });
                }
            },
            {
                "data": "endDate", "width": "15%", "render": function (data) {
                    var date = new Date();
                    return date.toLocaleString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' });
                }
            },
        ]
    });
}
