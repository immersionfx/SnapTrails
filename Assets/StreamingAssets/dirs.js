
var map = null;
const image = "walking.png";
var walkingMarker;
var waypts = [];
var distance = 0.0;
var directionsService = null;
var directionsRenderer = null;

    function initMap() {	
        directionsService = new google.maps.DirectionsService();
        directionsRenderer = new google.maps.DirectionsRenderer();
        map = new google.maps.Map(document.getElementById("map"), {
            zoom: 6,
            disableDefaultUI: true,
            center: { lat: 37.91, lng: 23.75 },
        });
    
        directionsRenderer.setMap(map);
        calculateAndDisplayRoute("37.91357453450387", "23.75631038934201");        
    }
   
    //37.91357453450387, 23.75631038934201
    function calculateAndDisplayRoute(lat, lng) {
      
        directionsService
        .route({
            origin: new google.maps.LatLng(lat, lng),
            destination: new google.maps.LatLng(37.91145635457938, 23.754509238340518),        
            waypoints: waypts,
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.WALKING,
        })
        .then((response) => {

            directionsRenderer.setDirections(response);
    
            const route = response.routes[0];
    
            // For each route, compute distance
            distance = 0.0;
            for (let i = 0; i < route.legs.length; i++) {
                distance += route.legs[i].distance.value;
            } 
            
            return "OK";
        })
        .catch((e) => { 
            window.alert("Directions request failed"); 
            return "N/A"; 
        });
    }
  
  // Callable functions //////////////////////////////////

    function updateMap(lat, lng) {	
        calculateAndDisplayRoute(lat, lng); 
    }

    function getDistance()
    {     
        //console.log("Distance: " + distance / 1000 + "km");
        return distance / 1000 + " km";
    }

    //Note: you must add &libraries=geometry to your script source:
    //<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false&libraries=geometry"></script>
    function getSphericalDistance(origLat, origLon, destLat, destLon) {
          //var loc1 = new google.maps.LatLng(37.91357453450387, 23.75631038934201);  
          //var loc2 = new google.maps.LatLng(37.91145635457938, 23.754509238340518);
          //console.log((google.maps.geometry.spherical.computeDistanceBetween(loc1, loc2) / 800).toFixed(3));  
          var loc1 = new google.maps.LatLng(origLat, origLon);  
          var loc2 = new google.maps.LatLng(destLat, destLon);
          return (google.maps.geometry.spherical.computeDistanceBetween(loc1, loc2) / 800).toFixed(3)
      }

    function addWaypoint(lat, lng) 
    {
        waypts.push({
            location: new google.maps.LatLng(lat, lng),
            stopover: true,
        });     
        return "OK";
    }

    function addMarker(lat, lng) 
    {
        walkingMarker = new google.maps.Marker({
            position: new google.maps.LatLng( lat, lng ), 
            icon: image,
            map: map
        });
        return "OK " + distance.text;
    }


    function moveMarker(lat, lng) 
    {
        walkingMarker.setPosition( new google.maps.LatLng( lat, lng ) );
        return "OK";
    }
  
    window.initMap = initMap;