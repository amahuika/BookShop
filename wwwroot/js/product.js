
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/admin/Product/GetAll"
        },
/*add columns is case sensitive must match incoming json */
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                // Render html the data in the function is the id
                "render": function (data) {
                    return `
                    <a class="btn btn-sm btn-primary mx-3" href="/Admin/Product/Upsert?id=${data}">
                        Edit
                    </a> 
                    <a class="btn btn-sm btn-danger mx-3"  onClick="Delete('/Admin/Product/DeleteConfirmed?id=${data}')" >
                        Delete
                    </a>

                            `
                }
            }


   

        ]
        })

}

function Delete(url) {

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

            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.success(data.message);
                    }
                }

                })





        }
    })

}