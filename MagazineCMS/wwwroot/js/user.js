$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#user-table').DataTable({
        "ajax": { url: '/admin/manageuser/getall' },
        "columns": [
            { data: 'firstname', "width": "25%" },
            { data: 'lastname', "width": "15%" },
            { data: 'avatarUrl', "width": "10%" },
            { data: 'facultyId', "width": "20%" },
            { data: 'Role.name', "width": "15%" }
        ]
    });
}