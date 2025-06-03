using System.Collections.ObjectModel;
using System.Text;
using WaveApp.ViewModels;

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
        htmlBuilder.AppendLine("function initMap() {");
        htmlBuilder.AppendLine("    var map = new google.maps.Map(document.getElementById('map'), {");
        htmlBuilder.AppendLine("        center: { lat: 37.7749, lng: -122.4194 },"); // Coordenadas iniciales de San Francisco
        htmlBuilder.AppendLine("        zoom: 12");
        htmlBuilder.AppendLine("    });");

        // Agregar pines (boyas) al mapa
        htmlBuilder.AppendLine("    var buoyPins = [");

        for (int i = 0; i < buoys.Count; i++)
        {
            var buoy = buoys[i];
            htmlBuilder.AppendLine($"        {{ position: {{ lat: {buoy.Latitude}, lng: {buoy.Longitude} }}, title: '{buoy.Name}', description: '{buoy.Description}' }}{(i < buoys.Count - 1 ? "," : "")}");
        }

        htmlBuilder.AppendLine("    ];");

        htmlBuilder.AppendLine("    buoyPins.forEach(function (buoy) {");
        htmlBuilder.AppendLine("        var marker = new google.maps.Marker({");
        htmlBuilder.AppendLine("            position: buoy.position,");
        htmlBuilder.AppendLine("            map: map,");
        htmlBuilder.AppendLine("            title: buoy.title");
        htmlBuilder.AppendLine("        });");

        htmlBuilder.AppendLine("        var infoWindow = new google.maps.InfoWindow({");
        htmlBuilder.AppendLine("            content: `<div><strong>${buoy.title}</strong><br>${buoy.description}</div>`");
        htmlBuilder.AppendLine("        });");

        htmlBuilder.AppendLine("        marker.addListener('click', function () {");
        htmlBuilder.AppendLine("            infoWindow.open(map, marker);");
        htmlBuilder.AppendLine("        });");
        htmlBuilder.AppendLine("    });");
        htmlBuilder.AppendLine("}");
        htmlBuilder.AppendLine("</script>");
        htmlBuilder.AppendLine("</head>");
        htmlBuilder.AppendLine("<body onload='initMap()'>");
        htmlBuilder.AppendLine("<div id='map'></div>");
        htmlBuilder.AppendLine("</body>");
        htmlBuilder.AppendLine("</html>");

        return htmlBuilder.ToString();
    }
}