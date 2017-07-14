$(document).ready(function () {

    //AjaxReservation

    var redirectData = "";
    $('#reservation').submit(function () {
        $.post('/House/AddReservation', $('#reservation').serialize(), function (data) {
            $('.errorBoxDate').html(data);
            $('.errorBoxDate').css("display", "block");
        });
        return false;
    });

    ratingHouse();

    














    var DefName = $("#defItem");
    var DefCom = $("#defItem");
    var DefRate = $("#defItem");



    $('.owl-carousel').owlCarousel({
        items: 1,
        loop: true,
        nav: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        smartSpeed: 450
    });
    $('a[href^="#"]').click(function () {
        var el = $(this).attr('href');
        $('body').animate({
            scrollTop: $(el).offset().top
        }, 1000);
        return false;
    });

    var dt = new Date();
    var month = dt.getMonth() + 1;
    if (month < 10) month = '0' + month;
    var day = dt.getDate();
    if (day < 10) day = '0' + day;
    var year = dt.getFullYear();
    $("#firstDateId").min = year + '-' + month + '-' + (day + 1);
    $("#firstDateId").max = (year + 1) + '-' + month + '-' + (day + 1);
    $("#firstDateId").value = year + '-' + month + '-' + (day + 1);
    $("#secondDateId").min = year + '-' + month + '-' + (day + 1);
    $("#secondDateId").max = (year + 1) + '-' + month + '-' + (day + 1);
    $("#secondDateId").value = year + '-' + month + '-' + (day + 1);

    var geo = new google.maps.Geocoder();
    var map = new google.maps.Map(document.getElementById('map_canvas_house'), {
        zoom: 16,
        scrollwheel: false,
        styles: styleArray,
        mapTypeId: 'hybrid'
    });

    var Country = $("#Country").text();
    var Region = $("#Region").text();
    var Street = $("#Street").text();
    var City = $("#City").text();
    var House = $("#House").text();
    var address = Street + ' ' + House + ' + ' + City + ', ' + Region + ', ' + Country;

    console.log(address);
    geo.geocode({ 'address': address }, function (results, status) {
        console.log(results[0]);
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });
        }
        else {
            alert('Not valid address');
        }
    });

});


var styleArray = [
    {
        "featureType": "landscape",
        "elementType": "geometry",
        "stylers": [
            {
                "color": "#bff0ca"
            }
        ]
    },
    {
        "featureType": "water",
        "elementType": "geometry",
        "stylers": [
            {
                "color": "#81d4fa"
            }
        ]
    }
];

document.getElementById("firstDateId").onchange = function () {
    var input = document.getElementById("secondDateId");
    input.min = this.value;

    var dt = new Date();
    var month = dt.getMonth() + 1;
    if (month < 10) month = '0' + month;
    var day = dt.getDate();
    if (day < 10) day = '0' + day;
    var year = dt.getFullYear();

    if (this.value < (year + '-' + month + '-' + (day + 1))) {
        this.value = year + '-' + month + '-' + (day + 1);
        this.min = year + '-' + month + '-' + (day + 1);
        input.min = year + '-' + month + '-' + (day + 1);
    }

    if (this.value >= input.value) {
        input.value = this.value;
    }
};

document.getElementById("secondDateId").onchange = function () {
    var dt = new Date();
    var month = dt.getMonth() + 1;
    if (month < 10) month = '0' + month;
    var day = dt.getDate();
    if (day < 10) day = '0' + day;
    var year = dt.getFullYear();

    if (this.value < (year + '-' + month + '-' + (day + 1))) {
        this.value = year + '-' + month + '-' + (day + 1);
    }
};

var ratingHouse = function () {
    var a = $('#sliderRating');
    var b = $('#textSliderRating');
    var $c = $('#numberSliderRating');

    var c = parseFloat($c.text().replace(',', '.'));
    c = c.toFixed(1);
    $c.text(c);

    if (c >= 3.5) {
        $('.sliderRating').css('background', '#5EF199');
        if (c >= 4 && c <= 4.5) {
            b.text('Хорошо');
        }
        else {
            b.text('Отлично');
        }
    }
    else if (c > 2.5 && c < 3.5) {
        $('.sliderRating').css('background', '#FFDD00');
        b.text('Удовлетворительно');

    }
    else {
        $('.sliderRating').css('background', '#F31B13');
        if (c >= 1.5 && c <= 2.5) {
            b.text('Плохо');
        }
        else {
            b.text('Ужасно');
        }

    }
}
