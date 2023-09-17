function OpenModalWallets(parameters) {
    console.log("Click");
    
    const modal = $('#popupID')
   
        $.ajax({
        url: parameters.url,
        type: 'GET',
        success: function (response) {
            console.log("success")
            modal.find(".modalbody").html(response);
            modal.modal('show')
        },
        failure: function () {
            console.log("failure");
            modal.modal('hide')
        },
        error: function (response) {
            console.log("error")
            alert(response.responseText)
            }
        });
       /* return false;*/
    }