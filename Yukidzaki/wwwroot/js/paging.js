

    pageLimit = Math.floor(localStorage.getItem("tokensCount") / 50);
    console.log(pageLimit);
    var page = 0;

    var _inCallback = false;
    function loadItems() {
        if (localStorage.getItem("page")) {
            console.log(localStorage.getItem("page"));
            page = localStorage.getItem("page");
            localStorage.removeItem("page");
        }
        if (page > -1 && !_inCallback) {
            _inCallback = true;
            page++;
            if (page <= pageLimit) {


                console.log(page);
                var filtersPageVM = {
                    tokensFilters: null,
                    page: page
                };

                $.ajax({
                    type: 'POST',
                    url: '/Gallery/Filter/',
                    contentType: 'application/json',
                    data: JSON.stringify(filtersPageVM),
                    success: function (data, textstatus) {
                        if (data != '') {
                            $("#tokenCardGalleryCount").append(data);
                        }
                        else {
                            page = -1;
                        }
                        _inCallback = false;
                        $("div#loading").hide();
                    }
                });

            }
        }
    }


$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            console.log("scroll");
            loadItems();
        }
    });
});



