// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    var detailsList = []; // Initialize detailsList as an empty array
    var _Id = -1;
    extractFromTable()
    $("#AddItemE").on("click", function () {
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


    function extractFromTable() {
        _Id =- 1;
        debugger
        var extractedData = [];

        // Iterate over each row of the table
        $("#dataTable tr").each(function (index, row) {
            var rowData = [];

            // Iterate over each cell (td) of the row
            $(row).find("td").each(function (cellIndex, cell) {
                // Extract the data from the cell and add it to the rowData array
                var cellData = $(cell).text().trim();
                rowData.push(cellData);
                $(this).remove();
            });

            // Add the rowData array to the extractedData array
            extractedData.push(rowData);
        });
        for (var i = 0; i < extractedData.length; i++) {
            var item = {
                Id: _Id,
                ItemName: extractedData[i][0],
                ItemPrice: extractedData[i][1],
                ItemCount: extractedData[i][2]

            }
            _Id -= 1;

            detailsList.push(item); // Use push method to add obj to detailsList

            appendToTable(item);
        }
        console.log(extractedData);
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


    $("#EditInvoice").on("click", function () {
        debugger


        console.log($("#Invoicedate").val())
        let invoiceHeader = {
            Id: $("#InvoiceId").val(),
            CustomerName: $("#CustomerName").val(),
            Invoicedate: $("#Invoicedate").val().toString(), // Serialize date to ISO string
            CashierId: $("#CashierId").val(), // Assuming this is obtained from some input field
            BranchId: $("#BranchId").val(), // Assuming this is obtained from some input field
            InvoiceDetails: detailsList
        };

        $.ajax({
            url: '/Invoices/Edit', // Specify your server endpoint URL
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












