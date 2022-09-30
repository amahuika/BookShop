
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/admin/Company/GetAll"
        },
/*add columns is case sensitive must match incoming json */
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                // Render html the data in the function is the id
                "render": function (data) {
                    return `
                    <a class="btn btn-sm btn-primary mx-3" href="/Admin/Company/Upsert?id=${data}">
                        Edit
                    </a> 
                    <a class="btn btn-sm btn-danger mx-3"  onClick="Delete('/Admin/Company/DeleteConfirmed?id=${data}')" >
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