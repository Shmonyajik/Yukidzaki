function applyFilter() {
    console.log("Filter!!!")
    var filters = [];

    // Get all the checkboxes
    var checkboxes = document.querySelectorAll('input[type="checkbox"]:checked');

    // Iterate over the checkboxes and create filter objects
    checkboxes.forEach(function (checkbox) {
        var filter = {
            FilterId: parseInt(checkbox.dataset.filterId),
            Value: checkbox.value
        };
        filters.push(filter);
    });

    $.ajax({
        url: '/Gallery/Filter',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(filters) ,
        success: function (result) {
            // Update the view with the filtered tokens
            // You can update the tokens list, reload the page, or update specific elements as per your application's needs
        }
    });
}