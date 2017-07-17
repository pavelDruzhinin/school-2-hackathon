$(document).ready(function () {
    $('#signupform').validate({
        rules: {
            LoginName: "required",
            Password: "required"
        },
        messages: {
            LoginName: "Обязательное поле",
            Password: "Обязательное поле 2"
        }
    });
});