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
    var minVal = Number($('#minCost').attr('minVal')),
        maxVal = Number($('#maxCost').attr('maxVal')),
        maxPer = (maxVal / 100) * 5,
        $sliderRange = $("#slider-range");
        $input1 = $('.cost1');
        $input2 = $('.cost2');

      $sliderRange.slider({
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

    //Select
 /*   $('.mainField .area .select').focus(function () {
        $(this).parent('.area').addClass('actArea');
    });

    $('.area ul li').on('click', function () {
        $('.mainField .area .select').val($(this).text());
    });

    $('.mainField .area .select').blur(function () {
        $(this).parent('.area').removeClass('actArea');
    });



    $('.mainField .area .select').keyup(function () {
        var inputText = $(this).val();
        var inputTextReg = new RegExp(inputText, "gi");
        var areaSelItem = $(this).parent('.area').children('ul').children('li');

        areaSelItem.each(function () {
            var text = $(this).text();
            var searchText = text.search(inputTextReg);
            if (searchText != -1) {
                $(this).css('display', 'list-item');
            } else {
                $(this).css('display', 'none');
            }
        });

    });*/
    //Sorting
    $('.sortingBox').click(function(){
      $(this).parent('.addField').toggleClass('actSort');
    });
});

var mapStyles = [
    {
        "featureType": "water",
        "elementType": "all",
        "stylers": [
            {
                "hue": "#7fc8ed"
            },
            {
                "saturation": 55
            },
            {
                "lightness": -6
            },
            {
                "visibility": "on"
            }
        ]
    },
    {
        "featureType": "water",
        "elementType": "labels",
        "stylers": [
            {
                "hue": "#7fc8ed"
            },
            {
                "saturation": 55
            },
            {
                "lightness": -6
            },
            {
                "visibility": "off"
            }
        ]
    },
    {
        "featureType": "poi.park",
        "elementType": "geometry",
        "stylers": [
            {
                "hue": "#83cead"
            },
            {
                "saturation": 1
            },
            {
                "lightness": -15
            },
            {
                "visibility": "on"
            }
        ]
    },
    {
        "featureType": "landscape",
        "elementType": "geometry",
        "stylers": [
            {
                "hue": "#f3f4f4"
            },
            {
                "saturation": -84
            },
            {
                "lightness": 59
            },
            {
                "visibility": "on"
            }
        ]
    },
    {
        "featureType": "landscape",
        "elementType": "labels",
        "stylers": [
            {
                "hue": "#ffffff"
            },
            {
                "saturation": -100
            },
            {
                "lightness": 100
            },
            {
                "visibility": "off"
            }
        ]
    },
    {
        "featureType": "road",
        "elementType": "geometry",
        "stylers": [
            {
                "hue": "#ffffff"
            },
            {
                "saturation": -100
            },
            {
                "lightness": 100
            },
            {
                "visibility": "on"
            }
        ]
    },
    {
        "featureType": "road",
        "elementType": "labels",
        "stylers": [
            {
                "hue": "#bbbbbb"
            },
            {
                "saturation": -100
            },
            {
                "lightness": 26
            },
            {
                "visibility": "on"
            }
        ]
    },
    {
        "featureType": "road.arterial",
        "elementType": "geometry",
        "stylers": [
            {
                "hue": "#ffcc00"
            },
            {
                "saturation": 100
            },
            {
                "lightness": -35
            },
            {
                "visibility": "simplified"
            }
        ]
    },
    {
        "featureType": "road.highway",
        "elementType": "geometry",
        "stylers": [
            {
                "hue": "#ffcc00"
            },
            {
                "saturation": 100
            },
            {
                "lightness": -22
            },
            {
                "visibility": "on"
            }
        ]
    },
    {
        "featureType": "poi.school",
        "elementType": "all",
        "stylers": [
            {
                "hue": "#d7e4e4"
            },
            {
                "saturation": -60
            },
            {
                "lightness": 23
            },
            {
                "visibility": "on"
            }
        ]
    }
];

$.getJSON("/House/HouseObject", function (data) {
    var markers = [];

    function initMap() {

        var map = new google.maps.Map(document.getElementById('map_canvas'), {
            zoom: 4,
            scrollwheel: false,
            center: { lat: 61.52401, lng: 105.318756 },
            styles: mapStyles
        });

        infoWindow = new google.maps.InfoWindow();

        for (var i = 0; i < data.length; i++) {
            var name = data[i].Name;
            var id = data[i].Id;
            var locality = data[i].Locality;
            var area = data[i].Area;
            var persons = data[i].NumberOfPersons;
            var price = data[i].Price;
            var rating = data[i].Rating;
            var latlng = new google.maps.LatLng(data[i].lat, data[i].lng);
            var marker = new google.maps.Marker({
                position: latlng,
                title: data[i].Name,
                icon: {
                    url: "../../Content/img/mapMarkers/map_marker.png"
                }
            });
            configMarders(marker, name, id, locality, area, persons, price, rating, map);

        }

        var mcClusterIconFolder = "../../Content/img/mapMarkers";
        var mcOptions = {
            maxZoom: 13,
            styles: [
                {
                    textColor: '#fff',
                    height: 54,
                    url: mcClusterIconFolder + "/mm1.png",
                    width: 54,
                    textSize: 14
                },
                {
                    textColor: '#fff',
                    height: 56,
                    url: mcClusterIconFolder + "/mm2.png",
                    width: 56,
                    textSize: 14
                },
                {
                    textColor: '#fff',
                    height: 66,
                    url: mcClusterIconFolder + "/mm3.png",
                    width: 66,
                    textSize: 14
                },
                {
                    textColor: '#fff',
                    height: 78,
                    url: mcClusterIconFolder + "/mm4.png",
                    width: 78,
                    textSize: 14
                },
                {
                    textColor: '#fff',
                    height: 90,
                    url: mcClusterIconFolder + "/mm5.png",
                    width: 90,
                    textSize: 14
                }
            ]
        };

        markerCluster = new MarkerClusterer(map, markers, mcOptions);



    }

    function configMarders(marker, name, id, locality, area, persons, price, rating, map) {
        markers.push(marker);

        google.maps.event.addListener(marker, "click", function () {
            map.setZoom(16);
            map.panTo(this.position);
            var contentString =
                '<a href="/House/Index?houseId=' + id + '" class="infowindow">' +
                '<p class="titInfo">' + name + '</p>' +
                '<p class="addressInfo">' + locality + ', ' + area + '</p>' +
                '<ul class="charact">' +
                '<li class="personsInfo"><i class="mdi mdi-human-handsdown"></i>' + persons + '</li>' +
                '<li class="priceInfo"><i class="mdi mdi-currency-rub"></i>' + price + ' руб./сутки</li>' +
                '<li class="ratingInfo"><i class="mdi mdi-star-circle"></i>' + rating + '/5</li>' +
                '</ul>' +

                '</a>';
            infoWindow.setContent(contentString);
            infoWindow.open(map, marker);
        });
    }

    google.maps.event.addDomListener(window, "load", initMap);

});
