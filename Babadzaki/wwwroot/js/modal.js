function openModal(parameters) {
    console.log("Click");
    const id = parameters.data;
    const url = parameters.url;
    const modal = $('#nftTOKEN')
    if (id === undefined || url === undefined) {
        alert('Ошибка')
        return;
    }
        $.ajax({
        url: url,
        type: 'GET',
        data: { "id": id },
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
        return false;
    });
}