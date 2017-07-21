$(document).ready(function () {
    $('#validFormLogin').validate({
        rules: {
            Login: "required",
            Password: "required"
        },
        messages: {
            Login: "Введите логин",
            Password: "Введите пароль"
        },
        focusCleanup: true,
        focusInvalid: false
    });
    $('#Phone').mask("+7(999)999-99-99", { autoclear: false });

    jQuery.validator.addMethod("checkMask", function (value, element) {
        return /\+\d{1}\(\d{3}\)\d{3}-\d{2}-\d{2}/g.test(value);
    });

    $.validator.addMethod("lettersonly", function (value, element) {
        return /[а-яА-ЯёЁa-zA-Z]+/.test(value);
    });

    $('#validFormRegister').validate({
        errorPlacement: function (error, element) {
            if (element.attr("name") == "FirstName") {
                error.insertAfter("#forTwoFields");
            } else if (element.attr("name") == "MiddleName") {
                error.insertAfter("#FirstName-error");
            } else if (element.attr("name") == "Password") {
                error.insertAfter("#forTwoFields2");
            } else if (element.attr("name") == "PasswordConfirm") {
                error.insertAfter("#Password-error");
            } else {
                error.insertAfter(element);
            }
        },
        rules: {
            FirstName: {
                required: true,
                rangelength: [2, 15],
                lettersonly: true
            },
            MiddleName: {
                required: true,
                rangelength: [2, 15],
                lettersonly: true
            },
            LastName: {
                required: true,
                rangelength: [2, 15],
                lettersonly: true
            },
            Email: {
                required: true,
                email: true,
                remote: '/Account/IsLoginAvailable'
            },
            Phone: {
                required: true,
                checkMask: true,
                remote: '/Account/IsLoginAvailable'
            },
            Login: {
                required: true,
                rangelength: [3, 15],
                remote: '/Account/IsLoginAvailable'
            },
            Password: {
                required: true,
                minlength: 5
            },
            PasswordConfirm: {
                required: true,
                equalTo: "#Password"
            }
        },
        messages: {
            FirstName: {
                required: "Введите ваше Имя",
                rangelength: "Длина Имени, должна быть от 2 до 15 символов",
                lettersonly: "Имя не может содержать цифры"
            },
            MiddleName: {
                required: "Введите ваше Отчество",
                rangelength: "Длина Отчества, должна быть от 2 до 15 символов",
                lettersonly: "Фамилия не может содержать цифры"
            },
            LastName: {
                required: "Введите вашу Фамилию",
                rangelength: "Длина Фамилии, должна быть от 2 до 15 символов",
                lettersonly: "Отчество не может содержать цифры"
            },
            Email: {
                required: "Введите ваш e-mail адрес",
                email: "Неверно введен email",
                remote: "Введенный e-mail адрес уже используется"
            },
            Phone: {
                required: "Введите ваш номер телефона",
                checkMask: "Введите полный номер телефона",
                remote: "Введенный номер телефона уже используется"
            },
            Login: {
                required: "Введите логин",
                rangelength: "Длина логина, должна быть от 3 до 15 символов",
                remote: "Введенный логин уже используется"
            },
            Password: {
                required: "Введите пароль",
                minlength: "Минимальная длина пароля от 5 символов"
            },
            PasswordConfirm: {
                required: "Подтвердите пароль",
                equalTo: "Пароль не совпадает"
            }
        },
        focusCleanup: true,
        focusInvalid: false
    });

    $('#Price').on('change', function () {
        if ($('#Price').val()[0] == 0) {
            $('#Price').val(1);
        }
    });

    $('#NumberOfPersons').on('change', function () {
        if ($('#NumberOfPersons').val()[0] == 0) {
            $('#NumberOfPersons').val(1);
        }
    });

    $.validator.addMethod("numbersonly", function (value, element) {
        return /[0-9]+/.test(value);
    });

    $('#validCreateHouse').validate({
        errorPlacement: function (error, element) {
            if (element.attr("name") == "Price") {
                error.insertAfter("#forTwoFields");
            } else if (element.attr("name") == "NumberOfPersons") {
                error.insertAfter("#Price-error");
            } else if (element.attr("name") == "Region" || element.attr("name") == "Locality" || element.attr("name") == "Area") {
                error.insertAfter(".manualInput");
            } else {
                error.insertAfter(element);
            }
        },
        rules: {
            Name: {
                required: true,
                rangelength: [3, 50]
            },
            Region: {
                required: true
            },
            Locality: {
                required: true
            },
            Area: {
                required: true
            },
            Price: {
                required: true,
                rangelength: [3, 6]
            },
            NumberOfPersons: {
                required: true,
                rangelength: [1, 2]
            },
            Description: {
                required: true,
                minlength: 3
            },
            Food: {
                required: true,
                minlength: 3
            },
            Transfer: {
                required: true,
                minlength: 3
            },
            ServicesIncluded: {
                required: true,
                minlength: 3
            },
            AdditionalServices: {
                required: true,
                minlength: 3
            },
            Accomodations: {
                required: true,
                minlength: 3
            },
            BookingConditions: {
                required: true,
                minlength: 3
            }
        },
        messages: {
            Name: {
                required: "Введите название дома",
                rangelength: "Длина названия дома, должна быть от 3 до 50 символов"
            },
            Region: {
                required: "Неверно выбран адрес"
            },
            Locality: {
                required: "Неверно выбран адрес"
            },
            Area: {
                required: "Неверно выбран адрес"
            },
            Price: {
                required: "Введите цену дома",
                rangelength: "Длина поля 'цена' дома, должна быть от 3 до 6 символов",
                matches: "Используйте только цифры"
            },
            NumberOfPersons: {
                required: "Введите количество человек",
                rangelength: "Длина поля 'количество человек', должна быть от 1 до 2 символов",
                matches: "Используйте только цифры"
            },
            Description: {
                required: "Введите описание дома",
                minlength: "Минимальная длина описания дома, от 3 символов"
            },
            Food: {
                required: "Введите описание предоставляемого питания",
                minlength: "Минимальная длина описания предоставляемого питания, от 3 символов"
            },
            Transfer: {
                required: "Введите описание предоставляемого трансфера",
                minlength: "Минимальная длина описания предоставляемого трансфера, от 3 символов"
            },
            ServicesIncluded: {
                required: "Введите услуги, включенные в стоимость",
                minlength: "Минимальная длина услуг, включенных в стоимость, от 3 символов"
            },
            AdditionalServices: {
                required: "Введите услуги за отдельную плату",
                minlength: "Минимальная длина услуг за отдельную плату, от 3 символов"
            },
            Accomodations: {
                required: "Введите условия проживания",
                minlength: "Минимальная длина условий проживания, от 3 символов"
            },
            BookingConditions: {
                required: "Введите условия бронирования",
                minlength: "Минимальная длина условий бронирования, от 3 символов"
            }
        },
        focusCleanup: true,
        focusInvalid: false,
    });


    $('.validation-summary-errors').on('click', function () {
        $('.validation-summary-errors').css('display', 'none');
    });
    
});

