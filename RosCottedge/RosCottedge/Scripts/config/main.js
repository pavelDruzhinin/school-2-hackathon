$(document).ready(function () {

    //Datepicker
    $('.date').attr('readOnly', 'true');
    var dates = $('#date1, #date2').datepicker({
        dateFormat: 'dd-mm-yy',
        monthNames: [
            "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август",
            "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
        ],
        dayNamesMin: ['ВС', 'ПН', 'ВТ', 'СР', 'ЧТ', 'ПТ', 'СБ'],
        dateFormat: 'dd.mm.yy',
        firstDay: 1,
        minDate: '+1d',
        maxDate: '+1y',
        onSelect: function (selectedDate) {
            var option = this.id == "date1" ? "minDate" : "maxDate",
                instance = $(this).data("datepicker"),
                date = $.datepicker.parseDate(
                    instance.settings.dateFormat || $.datepicker._defaults.dateFormat,
                    selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
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
        $( "#minCost" ).val(ui.values[ 0 ]);
        $( "#maxCost" ).val(ui.values[ 1 ]);
      }
    });
    $( "#slider-range span.ui-state-default:first").css('margin-left','-7.5px');
    $( "#slider-range span.ui-state-default:last").css('margin-left','-7.5px');

    //RatingItemIndex
    $('.ratingHouse').each(function () {
        var rating = Math.round(+$(this).attr('rating').replace(',', '.'));
        for (var i = 1; i <= rating; i++) {
            $(this).children('ul').children('li[ratingItem=' + i + ']').css('opacity', '1');
        }
    });

});


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
