using ReciboMAUIApp.ViewModels;

namespace ReciboMAUIApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
