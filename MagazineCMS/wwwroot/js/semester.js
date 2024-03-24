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
            {   "data": "name", "width": "15%" },
            {
                "data": "startDate", "width": "15%", 
                "render": function (data) {
                    var startDate = new Date(data);
                    var formattedStartDate = startDate.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' }) + ' ' + startDate.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
                    return formattedStartDate;
                }
            },
            {
                "data": "endDate", "width": "15%",
                "render": function (data) {
                    var endDate = new Date(data);
                    var formattedStartDate = startDate.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' }) + ' ' + startDate.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
                    return formattedStartDate;
                }
            },
        ]
    });
}
