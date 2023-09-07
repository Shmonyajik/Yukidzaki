$('.form-contact,.form-contact').submit(function (e) {
    e.preventDefault();
    let th = $(this);
    let mess = $('.mess');
    let btn = th.find('.btn');


    $.ajax({
        url: 'feedback/JsonPostQuestionSend',
        type: 'POST',
        dataType: 'json',
        data: th.serialize(),
        success: function (data) {
            if (data == 1) {
                mess.html('<div class="alert alert-danger mt-3">Incorrect email</div>');
                return false;
            } else {
                mess.html('<div class="alert alert-success mt-3 ">Message sent successfully</div>');
                th.trigger('reset');
                setTimeout(function () {
                    mess.html('<div class="alert alert-success"></div>');
                }, 3000)

            }
        }, error: function () {
            mess.html('<div class="alert alert-danger mt-3">Message not sent.</div>');
            th.trigger('reset');
            setTimeout(function () {
                mess.html('<div></div>');
            }, 3000)
        }
    })
})