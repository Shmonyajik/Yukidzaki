$(function () {

    $('div#loading').hide();
    //tokenCardGalleryCount
    var page = 0;
    var _inCallback = false;
    function loadItems() {
        if (page > -1 && !_inCallback) {
            _inCallback = true;
            page++;
            $('div#loading').show();
            var filtersPageVM = {
                tokensFilters = null,
                page = page
            };
            $.ajax({
                type: 'POST',
                url: '/Home/Index/',
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
    // обработка события скроллинга
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {

            loadItems();
        }
    });
})