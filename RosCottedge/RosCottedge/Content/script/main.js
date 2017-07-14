$(document).ready(function () {

    //AjaxReservation
    var redirectData = "";
    $('#reservation').submit(function () {
        $.post('/House/AddReservation', $('#reservation').serialize(), function (data) {
            $('.errorBox').html(data);
        });
        return false;
    });

    //Datepicker
    $('.date').attr('readOnly','true');
    $('.date').datepicker({
      dateFormat: 'dd-mm-yy',
      monthNames: [
        "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август",
        "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
      ],
      dayNamesMin: [ "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" ],
      minDate: '+1d',
      maxDate: '+1y'
    });

    var $date1 = $('.date1');
    var $date2 = $('.date2');
    var val1_d, val2_d,val1_m, val2_m, val1_y, val2_y;
    function validDate(val, date) {
      val1_d = Number($date1.val().slice(0,2)); val2_d = Number($date2.val().slice(0,2));
      val1_m = Number($date1.val().slice(3,5)); val2_m = Number($date2.val().slice(3,5));
      val1_y = Number($date1.val().slice(6,10)); val2_y = Number($date2.val().slice(6,10));
      if (val+"_d" != '' && val1_d > val2_d && val1_m == val2_m && val1_y == val2_y){
        date.val('');
      }else
      if (val+"_d" != '' && val1_m > val2_m && val1_y == val2_y){
        date.val('');
      }else
      if (val+"_d"!= '' && val1_y > val2_y){
        date.val('');
      }

    }
    $date1.change(function(){
      validDate("val2", $date2);
    });
    $date2.change(function(){
      validDate("val1", $date1);
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


var map;
function initMap() {
  var myLatLng = {lat: 61.7930796, lng: 34.3755158};
  map = new google.maps.Map(document.getElementById('map_canvas'), {
    zoom: 15,
    scrollwheel: false,
    center: myLatLng,
    styles: styleArray
  });
}
