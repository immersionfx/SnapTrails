
var map = null;
const image = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png";

function initMap() {

  map = new google.maps.Map(document.getElementById("map"), {
    zoom: 4,
    center: { lat: -33, lng: 151 },
  });

  const beachMarker = new google.maps.Marker({
    position: { lat: -33.89, lng: 151.274 },
    map,
    icon: image,
  });
}

function addMarker() {

    var marker = new google.maps.Marker({
      position: { lat: -30.89, lng: 121.274 },
      icon: image,
      map: map
    });
  }

window.initMap = initMap;