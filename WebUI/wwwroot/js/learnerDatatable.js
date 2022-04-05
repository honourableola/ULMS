$(document).ready(function () {
    $("#learnerDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "https://localhost:44355/api/learner/getlearners",
            "type": "POST",
            "datatype": "json",
            headers: { 'Tenant': 'delta' }
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "learnerLMSCode", "name": "WMLMS Code", "autoWidth": true },
            {
                "data": null, "sortable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "firstName", "name": "First Name", "autoWidth": true },
            { "data": "lastName", "name": "Last Name", "autoWidth": true },
            { "data": "email", "name": "Email", "autoWidth": false },
            {
                "render": function (data, row) { return "<a href='#' style='color:white' class='btn btn-Success' onclick=ViewInstructor('" + row.id + "'); >View</a>"; }
            },
            {
                "render": function (data, row) { return "<a href='#' style='color:white' class='btn btn-danger' onclick=DeleteInstructor('" + row.id + "'); >Delete</a>"; }
            },
        ]
    });
});