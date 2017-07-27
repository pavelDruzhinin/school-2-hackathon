$(document).ready(function () {
    initMap();
    //AjaxReservation

    var redirectData = "";
    $('#reservation').submit(function () {
        $.post('/House/AddReservation', $('#reservation').serialize(), function (data) {
            $('.errorBoxDate').html(data);
            console.log($('.errorBoxDate').text());
            $('.errorBoxDate').css("display", "none");
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
        $(this).css("display", "none");
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

    //comment
    var actRat = false;
    var actCom = false;

    function actBtn(actRat, actCom) {
      if (actRat == true && actCom == true){
        $('.btnSendReview').removeAttr('disabled');
      }
      else{
        $('.btnSendReview').attr('disabled','true');
      }
    }

    $('.mainRat ul li').click(function(){
      $('.mainRat ul li').removeClass('actRatItem');
      actRat = true;
      actBtn(actRat, actCom);
      var itemRat = $(this).attr('rating');
      $('#selectRating').html('').append('<option selected value="'+itemRat+'"></option>');
      for (var i=1;i<=itemRat;i++){
          $('.mainRat ul li[rating="'+i+'"]').addClass('actRatItem');
      }
    });

    $('#comment').keyup(function(){
      if ( $(this).val() != '' ){
        actCom = true;
      }else{
        actCom = false;
      }
      console.log(actCom);
      actBtn(actRat, actCom);
    });

    $('textarea').autosize();


});
var map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map_canvas_house'), {
        zoom: 17,
        scrollwheel: false,
        styles: styleArray
    });

    var lat = $('#LatId').val();
    var lng = $('#LngId').val();
    var Latlng = new google.maps.LatLng(lat, lng);

    map.setCenter(Latlng);
    var marker = new google.maps.Marker({
        map: map,
        position: Latlng
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
