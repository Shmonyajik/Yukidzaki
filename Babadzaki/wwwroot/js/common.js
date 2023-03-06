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

	$("#form").submit(function() {
		$.ajax({
			type: "POST",
			url: "HomeController.cs",
			data: $(this).serialize()
		}).done(function() {
			$(this).find("input").val("");
			alert("Спасибо за заявку! Скоро мы с вами свяжемся.");
			$("#form").trigger("reset");
		});
		return false;
	});
	
});