// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    var detailsList = []; // Initialize detailsList as an empty array
    var _Id = -1;

    $("#AddItem").on("click", function () {

        event.preventDefault(); // Prevent the default form submission behavior

        debugger;
        var _ItemName = $("#dtlItemName").val();
        var _ItemPrice = $("#ItemPrice").val();
        var _ItemCount = $("#ItemCount").val();

        if (_ItemName == '' || _ItemPrice == '' || _ItemCount == '') {

        }

        else {


            var obj = {
                Id: _Id,
                ItemName: _ItemName,
                ItemPrice: _ItemPrice,
                ItemCount: _ItemCount
            };

            detailsList.push(obj); // Use push method to add obj to detailsList
            appendToTable(obj); // Call the function to append the new row to the table
            _Id -= 1;

            _ItemName = $("#dtlItemName").val('');
            _ItemPrice = $("#ItemPrice").val('');
            _ItemCount = $("#ItemCount").val('');
            console.log(detailsList); // Optionally log the detailsList to verify
        }
    });


    function appendToTable(item) {
        // Generate HTML for the new table row
        var newRow = `<tr id="${item.Id}">` +
            "<td class='text-center'>" + item.ItemName + "</td>" +
            "<td class='text-center'>" + item.ItemPrice + "</td>" +
            "<td class='text-center'>" + item.ItemCount + "</td>" +
            `<td class="text-center"><button class="btn btn-danger DeleteRow" data-name="${item.Id}">Delete Item</button></td>` +
            "</tr>";

        // Append the new row to the table body
        $("#dataTable").append(newRow);
    }





    $(document).on("click", ".DeleteRow", function () {
        debugger;
        let rowID = $(this).data('name');
        $(`#${rowID}`).remove();
        var indexToRemove = detailsList.findIndex(item => item.Id === rowID);

        // Remove the object from the detailsList array if found
        if (indexToRemove !== -1) {
            detailsList.splice(indexToRemove, 1);
        }
    });




    $("#AddInvoice").on("click", function () {
        event.preventDefault(); // Prevent the default form submission behavior

        debugger
        //let model = {
        //    CustomerName: $("#CustomerName").val(),
        //    BranchId: $("#BranchId").val(),
        //    CashierId: $("#CashierId").val(),
        //    Invoicedate: $("#Invoicedate").val(),
        //    InvoiceDetails: detailsList
        //}
        console.log($("#Invoicedate").val())
        let invoiceHeader = {
            CustomerName: $("#CustomerName").val(),
            Invoicedate: $("#Invoicedate").val().toString(), // Serialize date to ISO string
            CashierId: $("#CashierId").val(), // Assuming this is obtained from some input field
            BranchId: $("#BranchId").val(), // Assuming this is obtained from some input field
            InvoiceDetails: detailsList
        };

        $.ajax({
            url: '/Invoices/Create', // Specify your server endpoint URL
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(invoiceHeader),
            success: function (response) {
                // Handle success response
                console.log(response);
                window.location.href = response.url;

            },
            error: function (xhr, status, error) {
                // Handle error response
                console.error(error);
            }
        });


     
    }
    )


    







});












