using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;


namespace WeatherApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ApiKey = "be80155ba28a0e203fed19986004b83c";
        private const string ApiBaseUrl = "https://api.openweathermap.org/data/2.5/weather?q=";
        public MainWindow()
        {
            InitializeComponent();
        }

        public class WeatherData
        {
            public MainInfo main { get; set; }
            public WeatherInfo[] weather { get; set; }
        }

        public class MainInfo
        {
            public float temp { get; set; }
        }

        public class WeatherInfo
        {
            public string description { get; set; }
            public string icon { get; set; }
        }

        private async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            string location = locationTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please enter a location.");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"{ApiBaseUrl}{location}&appid={ApiKey}&units=imperial";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON into your C# class
                    WeatherData weatherData = JsonSerializer.Deserialize<WeatherData>(json);

                    // Access weather information
                    float temperature = weatherData.main.temp;
                    string description = weatherData.weather[0].description;
                    string icon = weatherData.weather[0].icon;

                    // Update UI elements with weather information
                    weatherInfoText.Text = $"Temperature: {temperature}°F\nDescription: {description}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching weather data: {ex.Message}");
            }
        }
    }
}
