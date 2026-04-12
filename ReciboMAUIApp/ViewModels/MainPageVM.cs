using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuestPDF.Fluent;

namespace ReciboMAUIApp.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        [ObservableProperty]
        private string? _customerName;

        [ObservableProperty]
        private double? _price;

        [ObservableProperty]
        private string? _serviceDescription;

        [RelayCommand] 
        private async Task GenerateImage()
        {
            try
            {
                CreateFolder();
                var acquittance = new AcquittanceDocument(155, _price, _customerName, _serviceDescription);
                IEnumerable<byte[]> images = acquittance.GenerateImages();
                
                int i = 0;
                foreach (var imgBytes in images)
                {
                    File.WriteAllBytes($"{acquittance.ToString()}.png", imgBytes);
                    i++;
                }

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
        private async Task GeneratePDF(object hasToOpenObject)
        {
            try
            {
                CreateFolder();
                bool hasToOpen = (bool)hasToOpenObject;
                var acquittance = new AcquittanceDocument(155, _price, _customerName, _serviceDescription);
                if (hasToOpen)
                    acquittance.GeneratePdfAndShow();
                else
                    acquittance.GeneratePdf($"{acquittance.ToString()}.pdf");
                await Application.Current.MainPage.DisplayAlert("Aviso", "Operação realizada com sucesso!", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível gerar o PDF!", "OK");
            }
        }

        private void CreateFolder()
        {
            const string FOLDER_NAME = "Output";
            if (!File.Exists(FOLDER_NAME))
                Directory.CreateDirectory(FOLDER_NAME);
        }
    }
}
