using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuestPDF.Fluent;

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
                IEnumerable<byte[]> images = acquittance.GenerateImages();
                var imgB = images.First();
                File.WriteAllBytes($"{path}\\{acquittance.ToString()}.png", imgB);

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
                acquittance.GeneratePdfAndShow();
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
