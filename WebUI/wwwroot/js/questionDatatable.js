﻿$(document).ready(function () {
    $("#questionDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "https://localhost:44355/api/question/getquestions",
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
            { "data": "id", "name": "WMLMS Code", "autoWidth": true },
            {
                "data": null, "sortable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "questionText", "name": "Question Text", "autoWidth": true },
            { "data": "points", "name": "Points", "autoWidth": true },
            { "data": "moduleName", "name": "Module", "autoWidth": true },
            {
                "render": function (data, row) { return "<a href='#' style='color:white' class='btn btn-Success' onclick=ViewInstructor('" + row.id + "'); >View</a>"; }
            },
            {
                "render": function (data, row) { return "<a href='#' style='color:white' class='btn btn-danger' onclick=DeleteInstructor('" + row.id + "'); >Delete</a>"; }
            },
        ]
    });
});