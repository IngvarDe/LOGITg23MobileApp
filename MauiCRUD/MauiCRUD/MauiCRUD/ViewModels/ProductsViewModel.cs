using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using MauiCRUD.Models;
using System.Collections.ObjectModel;

namespace MauiCRUD.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;

        public ProductsViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        [ObservableProperty]
        private Product _operatingProduct = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        public async Task LoadProductAsync()
        {
            await ExecuteAsync(async () =>
            {
                var products = await _context.GetAllAsync<Product>();
                if (products != null && products.Any())
                {
                    Products ??= new ObservableCollection<Product>();

                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                }
            }, "Fetching products...");
        }

        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                await operation?.Invoke();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }

        [RelayCommand]
        private void SetOperatingProduct(Product? product) => OperatingProduct = product ?? new();

        [RelayCommand]
        private async Task SaveProductAsync()
        {
            if (OperatingProduct != null)
                return;

            var (isValid, errorMessage) = OperatingProduct.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                return;
            }

            var busyText = OperatingProduct.Id == 0 ? "Createing product..." : "Updating product...";
            await ExecuteAsync(async () =>
            {
                if (OperatingProduct.Id == 0)
                {
                    await _context.AddItemAsync<Product>(OperatingProduct);
                    Products.Add(OperatingProduct);
                }
                else
                {
                    if (await _context.UpdateItemAsync<Product>(OperatingProduct))
                    {
                        var productCopy = OperatingProduct.Clone();

                        var index = Products.IndexOf(OperatingProduct);
                        Products.RemoveAt(index);

                        Products.Insert(index, productCopy);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Product updating error", "OK");
                        return;
                    }
                }
                SetOperatingProductCommand.Execute(new());
            }, busyText);
        }

        [RelayCommand]
        private async Task DeleteProductAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteItemByKeyAsync<Product>(id))
                {
                    var product = Products.FirstOrDefault(p => p.Id == id);
                    Products.Remove(product);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Product was not deleted", "OK");
                }
            }, "Deleting product...");
        }
    }
}
