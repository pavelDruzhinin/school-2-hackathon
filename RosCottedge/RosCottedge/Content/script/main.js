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
      dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
      dateFormat: 'dd.mm.yy',
      firstDay: 1,
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

    //Slider-range
    var minVal = Number($('#minCost').attr('minVal'));
    var maxVal = Number($('#maxCost').attr('maxVal'));
    var maxPer = (maxVal/100)*5;
    $( "#slider-range" ).slider({
      range: true,
      min: minVal,
      max: maxVal,
      values: [ minVal+maxPer, maxVal-maxPer ],
      slide: function( event, ui ) {
        $( "#minCost" ).val(ui.values[ 0 ] + " руб");
        $( "#maxCost" ).val(ui.values[ 1 ] + " руб");
      }
    });
    $( "#slider-range span.ui-state-default:first").css('margin-left','-15px');
    //$( "#slider-range span.ui-state-default:last").css('margin-left','-1px');

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
