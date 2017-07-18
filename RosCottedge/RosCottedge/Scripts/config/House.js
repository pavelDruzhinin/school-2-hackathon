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

    $('#formReviewId').submit(function () {
        $.post('/House/AddReview', $('#formReviewId').serialize(), function (data) {
            $('#comments').html(data);
            $('#totalCount').text(parseInt($('#totalCount').text()) + 1 + ' отзывов');
            $('#numberSliderRating').text($('#tempRating2').val());
            ratingHouse();    
            $('#formReviewId').each(function () {
                this.reset();
            });
            $('#formReviewId').css('display', 'none');
        });
        return false;
    });

    $('.errorBoxDate').on('click', function () {
        $('.errorBoxDate').css("display", "none");
    });

    ratingHouse();

    
    var totalItems = $('.item').length;
    if (totalItems == 0) {
        $('.slider').css('height', '77px');
    } else if (totalItems == 1) {
        var isLooped = false;
        var isNav = false;
    } else {
        var isLooped = true;
        var isNav = true;
    }

    $('.owl-carousel').owlCarousel({
        items: 1,
        loop: isLooped,
        nav: isNav,
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

    var geo = new google.maps.Geocoder();
    

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
            $('#MapAddressNotFound').css('display', 'block');
            $('#map_canvas_house').css('display', 'none');
            $('#SlideMap').attr('href','#MapAddressNotFound');
        }
    });

});
var map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map_canvas_house'), {
        zoom: 17,
        scrollwheel: false,
        styles: styleArray
    });
}

var styleArray = [
    {
        "featureType": "landscape.natural",
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

var ratingHouse = function () {
    var $colorav = $('.sliderRating');
    var b = $('#textSliderRating');
    var $c = $('#numberSliderRating');

    if (parseInt($c.text()) != 0) {
    var c = parseFloat($c.text().replace(',', '.'));
    c = c.toFixed(1);
    $c.text(c);


    if (c >= 3.5) {
        $colorav.css('background-color', '#5EF199');
        if (c >= 4 && c <= 4.5) {
            b.text('Хорошо');
        }
        else {
            b.text('Отлично');
        }
    }
    else if (c > 2.5 && c < 3.5) {
        $colorav.css('background-color', '#FFD54F');
        b.text('Удовлетворительно');

    }
    else {
        $colorav.css('background-color', '#E32444');
        if (c >= 1.5 && c <= 2.5) {
            b.text('Плохо');
        }
        else {
            b.text('Ужасно');
        }

    }
    }
    else {
        $colorav.css('background', '#AAAAAA');
        $c.text('0');
        b.text('Нет оценки');
    }
};
