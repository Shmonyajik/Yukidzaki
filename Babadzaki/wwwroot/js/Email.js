$(document).ready(function () {
    $("#subscribe-form").submit(function (event) {
        console.log("Email")
        var formData = {           
            emailName: $("#subscribe-email").val(), 
        };
        var actionPath = $(this).attr("action");
        console.log(actionPath)
        $.ajax({
            type: "POST",
            url: actionPath,
            data: formData,
            contentType: "application/x-www-form-urlencoded",
            success: function (response) {
                switch (response.StatusCode) {
                    case 200:
                        console.log("Success");
                        break;
                    case 400:
                        console.log("Model state is invalid");
                        break;

                    case 498:  
                        console.log("No Mx Records");
                        break;
                    
                    default:    
                        console.log("Internal server Error");
                        break;
                }
            },
            error: function (xhr, status, error) {
                // Handle AJAX error
                console.error(error);
            }
        
        }).done(function (data) {
            console.log(data);
        });

        event.preventDefault();
    });
});