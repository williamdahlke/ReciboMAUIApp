using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

namespace ReciboMAUIApp.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string? _customerName;
        public string? CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }

        private double? _price;
        public double? Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private string? _serviceDescription;
        public string? ServiceDescription
        {
            get => _serviceDescription;
            set => SetProperty(ref _serviceDescription, value);
        }

        [RelayCommand]
        private async Task GenerateImage()
        {
            string path = $"{FileSystem.AppDataDirectory}\\Output";
            try
            {
                CreateFolder(path);
                var acquittance = new AcquittanceDocument(Id, Price, CustomerName, ServiceDescription);
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                var response = await httpClient.PostAsJsonAsync("https://reciboapi.onrender.com/acquittance/image", acquittance);
                //var response = await httpClient.PostAsJsonAsync("https://localhost:44327/acquittance/image", acquittance);
                response.EnsureSuccessStatusCode();

                var img = await response.Content.ReadAsByteArrayAsync();
                string imagePath = Path.Combine(path, $"{acquittance}.png");
                File.WriteAllBytes(imagePath, img);

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(imagePath)
                });

                await Shell.Current.DisplayAlert("Aviso", "Operação realizada com sucesso!", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Shell.Current.DisplayAlert("Erro", "Não foi possível gerar as imagens!", "OK");
            }
        }

        [RelayCommand]
        private async Task GeneratePDF()
        {
            try
            {
                var acquittance = new AcquittanceDocument(Id, Price, CustomerName, ServiceDescription);
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                //var response = await httpClient.PostAsJsonAsync("https://localhost:44327/acquittance/pdf", acquittance);
                var response = await httpClient.PostAsJsonAsync("https://reciboapi.onrender.com/acquittance/pdf", acquittance);
                response.EnsureSuccessStatusCode();

                var bytes = await response.Content.ReadAsByteArrayAsync();
                var filePath = Path.Combine(FileSystem.CacheDirectory, acquittance.ToString());
                File.WriteAllBytes($"{filePath}.pdf", bytes);

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile($"{filePath}.pdf")
                });

                await Shell.Current.DisplayAlert("Aviso", "Operação realizada com sucesso!", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Shell.Current.DisplayAlert("Erro", "Não foi possível gerar o PDF!", "OK");
            }
        }

        private static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
