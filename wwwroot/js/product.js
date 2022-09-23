
var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/admin/Product/GetAll"
        },
/*add columns is case sensitive must match incoming json */
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category", "width": "15%" }

   

        ]
        })

}