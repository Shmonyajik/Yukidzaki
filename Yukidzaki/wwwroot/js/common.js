//$(document).ready(function() {

//	$("#form").submit(function() {
//		$.ajax({
//			type: "POST",
//			url: "HomeController/IndexPost",
//			data: $(this).serialize()
//		}).done(function() {
//			$(this).find("input").val("");
//			alert("Спасибо за заявку! Скоро мы с вами свяжемся.");
//			$("#form").trigger("reset");
//		});
//		return false;
//	});
	
//});
//$(document).ready(function () {

//	$("#form").submit(function () {
//        var formData = new FormData();
//        formData.append("email", $("#email").val());
//        $.ajax({
//            type: 'POST',
//            url: '@Url.Action("JsonWriteEmail", "Home")',
//            contentType: false,
//            processData: false,
//            cache: false,
//            data: formData,
//            success: successCallback,
//            error: errorCallback
//	});

//});
$(document).ready(function () {
    
    $('#subscribe-form').submit(function (q) {
        q.preventDefault();
        let th = $(this);
        let messi = $('.mes');
        $.ajax({
            url: th.action,
            type: 'POST',
            dataType: 'json',
            data: th.serialize(),
            success: function (data) {
                if (data == 1) {
                    messi.html('<div class="messf"></div>');
                    return false;
                } else {
                    messi.html('<div class="messt"></div>');
                    th.trigger('reset');
                    setTimeout(function () {
                        messi.html('<div"></div>');
                    }, 3000)
                }
            }, error: function () {
                messi.html('<div class="messf"></div>');
                th.trigger('reset');
                setTimeout(function () {
                    messi.html('<div></div>');
                }, 3000)
            }


        })
    })
	
});