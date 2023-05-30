$(document).ready(function () {
    console.log("render!")
    GetData(null)
});
const tokenCardGalleryContainer = $('#tokenCardGalleryContainer')
function applyFilter() {
    console.log("Filter!!!")
    var filters = [];
    var search = document.getElementById("elastic_clothig");
    var tokensCount = document.getElementById("tokensCount");
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
    if (search.value !== null && typeof search.value !== "undefined" && search.value !== "") {
        filters.push({ FilterId: 0, Value: search.value })
    }
    
    if (checkboxes) {
        GetData(filters)
    }
    else {
        GetData(null)
    }
    
    
    return false;
}

function GetData(filters) {
    $.ajax({
        url: '/Gallery/Filter',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(filters),
        success: function (response, responseData, data) {
            console.log("success")
            tokensCount.value = 10;
            tokenCardGalleryContainer.find(".tokenCardGallery").html(response);
            
        },
        //failure: function () {
        //    console.log("failure");
        //    modal.modal('hide')
        //},
        //error: function (response) {
        //    console.log("error")
        //    alert(response.responseText)
        //}
    });
}
