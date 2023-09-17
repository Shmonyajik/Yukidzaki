$(document).ready(function () {
    console.log("render!")
    var filterPageVM = {
        tokensFilters: null,
        page: 0
    };
    const topScroll = document.getElementById("tokensCount");
    localStorage.setItem("page", 0);
    GetData(filterPageVM);
});
const tokenCardGalleryContainer = $('#tokenCardGalleryContainer')

function ClearAll(){
    const inputs = document.querySelectorAll('input[type="checkbox"]:checked');
    const off = () => inputs.forEach(item => item.checked = false);
    console.log(off);
    var filterPageVM = {
        tokensFilters :off(),
        page : 0
    };
    const topScroll = document.getElementById("tokensCount");
    topScroll.scrollIntoView();
    localStorage.setItem("page", 0);
    GetData(filterPageVM);
}

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
    var filterPageVM = {
        tokensFilters: filters,
        page : 0
    };
    console.log(filterPageVM);
    const topScroll = document.getElementById("tokensCount");
    topScroll.scrollIntoView();
    localStorage.setItem("page", 0);

    //if (checkboxes) {
    //    GetData(filterPageVM)
    //}
    //else {
    //    GetData(null)
    //}
    GetData(filterPageVM);


    return false;
}

function GetData(filtersPageVM) {
    $.ajax({
        url: '/Gallery/Filter',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(filtersPageVM),
        success: function (response) {
            console.log("success")
            tokenCardGalleryContainer.find(".tokenCardGallery").html(response);

        },
        complete: function (xhr, textStatus) {
            console.log("complete!");
            var tokensCount = document.getElementById("tokenCardGalleryCount").getAttribute('value');
            console.log(tokensCount);
            localStorage.setItem("tokensCount", tokensCount);
            $('#tokensCount').val(tokensCount);

           

            
        },
        //failure: function () {
        //    console.log("failure");
        //    modal.modal('hide')
        //},
        error: function (response) {
            console.log("error")
            alert(response.responseText)
        }
    });
}

