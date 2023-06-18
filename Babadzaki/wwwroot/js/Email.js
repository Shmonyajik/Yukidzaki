$(document).ready(function () {
    $("form").submit(function (event) {
        console.log("Email")
        var formData = {           
            email: $("#subscribe-email").val(), 
        };
        var actionPath = $(this).attr("action");
        console.log(actionPath)
        $.ajax({
            type: "POST",
            url: actionPath,
            data: formData,
            contentType: "‘application/x-www-form-urlencoded",
        }).done(function (data) {
            console.log(data);
        });

        event.preventDefault();
    });
});