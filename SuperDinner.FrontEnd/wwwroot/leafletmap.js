export function LoadMap(createRestaurantDotNetRef) {

    let map = L.map('map').setView([51.505, -0.09], 16);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        })
        .addTo(map);

    var marker;

    map.on('click', function (e) {
        if (marker)
            map.removeLayer(marker);

        marker = L.marker([e.latlng.lat, e.latlng.lng])
            .addTo(map);

        createRestaurantDotNetRef.invokeMethodAsync("UpdateCoordinates", e.latlng.lat, e.latlng.lng);
    });
}
