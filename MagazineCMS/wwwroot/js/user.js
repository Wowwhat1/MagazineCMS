$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#user-table').DataTable({
        "ajax": { url: '/admin/manageuser/getall' },
        "columns": [
            { data: 'firstname', "width": "15%" },
            { data: 'lastname', "width": "15%" },
            { data: 'avatarUrl', "width": "10%" },
            { data: 'faculty.name', "width": "5%" },
            { data: 'email', "width": "55%" }
        ]
    });
}