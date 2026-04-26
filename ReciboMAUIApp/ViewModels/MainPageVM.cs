using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

namespace ReciboMAUIApp.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string? _customerName;

        [ObservableProperty]
        private double? _price;

        [ObservableProperty]
        private string? _serviceDescription;

        [RelayCommand]
        private async Task GenerateImage()
        {
            string path = $"{FileSystem.AppDataDirectory}\\Output";
            try
            {
                CreateFolder(path);
                var acquittance = new AcquittanceDocument(_id, _price, _customerName, _serviceDescription);
                var httpClient = new HttpClient();
         
                var response = await httpClient.PostAsJsonAsync("https://reciboapi.onrender.com/acquittance/image", acquittance);
                //var response = await httpClient.PostAsJsonAsync("https://localhost:44327/acquittance/image", acquittance);
                response.EnsureSuccessStatusCode();

                var img = await response.Content.ReadAsByteArrayAsync();

                File.WriteAllBytes($"{path}\\{acquittance.ToString()}.png", img);

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile($"{path}\\{acquittance.ToString()}.png")
                });

                await Application.Current.MainPage.DisplayAlert("Aviso", "Operação realizada com sucesso!", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível gerar as imagens!", "OK");
            }
        }

        [RelayCommand]
        private async Task GeneratePDF()
        {
            try
            {
                var acquittance = new AcquittanceDocument(_id, _price, _customerName, _serviceDescription);
                var httpClient = new HttpClient();
            
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

                await Application.Current.MainPage.DisplayAlert("Aviso", "Operação realizada com sucesso!", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível gerar o PDF!", "OK");
            }
        }

        private void CreateFolder(string path)
        {
            if (!File.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
