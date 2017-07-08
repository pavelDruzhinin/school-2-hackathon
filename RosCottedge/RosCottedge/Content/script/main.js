$(document).ready(function(){

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
