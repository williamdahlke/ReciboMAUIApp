using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuestPDF.Fluent;
using ReciboMAUIApp.Model;

namespace ReciboMAUIApp.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        private const string FILE_NAME = "recibo.pdf";

        [ObservableProperty]
        private string? _customerName;

        [ObservableProperty]
        private double? _price;

        [ObservableProperty]
        private string? _serviceDescription;

        [RelayCommand]
        private void GeneratePDF()
        {
            try
            {
                var acquittance = new AcquittanceDocument
                {
                    Id = 155,
                    Price = _price.GetValueOrDefault(),
                    CustomerName = _customerName,
                    Description = _serviceDescription,
                    City = $"{Factory.City}, {Factory.State}, {DateTime.Now.ToLongDateString()}",
                    FactoryName = Factory.Name,
                    FactoryAddress = Factory.Address,
                    FactoryDocument = Factory.Document,
                    FactoryCellPhone = Factory.CellPhone
                };
                var path = Path.Combine(FILE_NAME);
                acquittance.GeneratePdfAndShow();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
