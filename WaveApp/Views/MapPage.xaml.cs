using System.Text;
using WaveApp.ViewModels;
using System.Collections.ObjectModel;
using WaveApp.Models;

namespace WaveApp.Views;

public partial class MapPage : ContentPage
{
    private const string GoogleMapsApiKey = "";
    private readonly MapViewModel _viewModel;

    public MapPage()
    {
        InitializeComponent();

        // Vincular el ViewModel
        _viewModel = new MapViewModel();

        // Generar el HTML para Google Maps
        var html = GenerateGoogleMapsHtml(_viewModel.Buoys);
        GoogleMapView.Source = new HtmlWebViewSource
        {
            Html = html
        };
    }

    private static string GenerateGoogleMapsHtml(ObservableCollection<Buoy> buoys)
    {
        var htmlBuilder = new StringBuilder();

        htmlBuilder.AppendLine("<!DOCTYPE html>");
        htmlBuilder.AppendLine("<html>");
        htmlBuilder.AppendLine("<head>");
        htmlBuilder.AppendLine("<meta name='viewport' content='initial-scale=1.0, user-scalable=no' />");
        htmlBuilder.AppendLine("<style>html, body, #map { height: 100%; margin: 0; padding: 0; }</style>");
        htmlBuilder.AppendLine($"<script src='https://maps.googleapis.com/maps/api/js?key={GoogleMapsApiKey}'></script>");
        htmlBuilder.AppendLine("<script>");
        htmlBuilder.AppendLine("let map;");
        htmlBuilder.AppendLine("function initMap() {");
        htmlBuilder.AppendLine("    map = new google.maps.Map(document.getElementById('map'), {");
        htmlBuilder.AppendLine("        center: { lat: 37.7749, lng: -122.4194 },"); // Initial coordinates of San Francisco
        htmlBuilder.AppendLine("        zoom: 12");
        htmlBuilder.AppendLine("    });");

        // Add pins (buoys) to the map
        htmlBuilder.AppendLine("    var buoyPins = [");

        for (int i = 0; i < buoys.Count; i++)
        {
            var buoy = buoys[i];
            htmlBuilder.AppendLine($"        {{ position: {{ lat: {buoy.Latitude}, lng: {buoy.Longitude} }}, title: '{buoy.Name}' }}{(i < buoys.Count - 1 ? "," : "")}");
        }

        htmlBuilder.AppendLine("    ];");

        htmlBuilder.AppendLine("    buoyPins.forEach(function (buoy) {");
        htmlBuilder.AppendLine("        var marker = new google.maps.Marker({");
        htmlBuilder.AppendLine("            position: buoy.position,");
        htmlBuilder.AppendLine("            map: map,");
        htmlBuilder.AppendLine("            title: buoy.title");
        htmlBuilder.AppendLine("        });");

        htmlBuilder.AppendLine("        var infoWindow = new google.maps.InfoWindow({");
        htmlBuilder.AppendLine("            content: `<div><strong>${buoy.title}</strong><br></div>`");
        htmlBuilder.AppendLine("        });");

        htmlBuilder.AppendLine("        marker.addListener('click', function () {");
        htmlBuilder.AppendLine("            infoWindow.open(map, marker);");
        htmlBuilder.AppendLine("        });");
        htmlBuilder.AppendLine("    });");
        htmlBuilder.AppendLine("}");

        // Add a function to center the map
        htmlBuilder.AppendLine("function centerMap(lat, lng) {");
        htmlBuilder.AppendLine("    if (map) {");
        htmlBuilder.AppendLine("        map.setCenter(new google.maps.LatLng(lat, lng));");
        htmlBuilder.AppendLine("    }");
        htmlBuilder.AppendLine("}");
        htmlBuilder.AppendLine("</script>");
        htmlBuilder.AppendLine("</head>");
        htmlBuilder.AppendLine("<body onload='initMap()'>");
        htmlBuilder.AppendLine("<div id='map'></div>");
        htmlBuilder.AppendLine("</body>");
        htmlBuilder.AppendLine("</html>");

        return htmlBuilder.ToString();
    }

    private void OnCenterMapClicked(object sender, EventArgs e)
    {
        // Encuentra la primera boya activa
        var activeBuoy = _viewModel.Buoys.FirstOrDefault(b => b.Status == "Activa");

        if (activeBuoy != null)
        {
            // Invoca la función centerMap en el HTML
            var script = $"centerMap({activeBuoy.Latitude}, {activeBuoy.Longitude});";
            GoogleMapView.Source = new HtmlWebViewSource
            {
                Html = GenerateGoogleMapsHtml(_viewModel.Buoys),
                BaseUrl = null
            };
            GoogleMapView.Eval(script);
        }
        else
        {
            // Centra el mapa en una ubicación predeterminada
            var defaultLatitude = 37.7749; // San Francisco
            var defaultLongitude = -122.4194;
            var script = $"centerMap({defaultLatitude}, {defaultLongitude});";
            GoogleMapView.Eval(script);
        }
    }

    private void OnChangeMapTypeClicked(object sender, EventArgs e)
    {
        // Logic to change the map type (e.g., satellite, terrain)
    }
}