
var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess")) {

        loadDataTable("inprocess")

    } else if (url.includes("completed")) {

         
           loadDataTable("completed")

    } else if (url.includes("pending")) {

           loadDataTable("pending")

    } else if (url.includes("approved")) {

        loadDataTable("approved")


    }else {

           loadDataTable("all")


    }
        


    

});

function loadDataTable(status) {
    dataTable = $("#dataTable").DataTable({
        "ajax": {
            "url": "/admin/Order/GetAll?status=" + status
        },
/*add columns is case sensitive must match incoming json */
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "20%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "10%" },
            {
                "data": "id",
                // Render html the data in the function is the id
                "render": function (data) {
                    return `
                    <a class="btn btn-sm btn-primary mx-3" href="/Admin/Order/Details?orderid=${data}">
                        Details
                    </a> 
             

                            `
                },
                "width": "10%"
            }


   

        ]
        })

}

