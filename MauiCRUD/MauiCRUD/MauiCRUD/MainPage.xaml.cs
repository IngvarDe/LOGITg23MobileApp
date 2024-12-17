using MauiCRUD.ViewModels;

namespace MauiCRUD
{
    public partial class MainPage : ContentPage
    {
        private readonly ProductsViewModel _productsViewModel;

        public MainPage(ProductsViewModel productsViewModel)
        {
            InitializeComponent();
            BindingContext = productsViewModel;
            _productsViewModel = productsViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _productsViewModel.LoadProductAsync();
        }
    }

}
