using SQLite;
using System.Collections.ObjectModel;

namespace TruthOrDrink;

public partial class PicturePage : ContentPage
{
    private ObservableCollection<Product> _products;
    private SQLiteConnection database;

    private string savedImagePath;
    public PicturePage()
    {
        InitializeComponent();
        

        ProductList.ItemsSource = _products;
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "products.db");
        database = new SQLiteConnection(dbPath);
        database.CreateTable<Product>();
        LoadProducts();
    }



    private async void AddPictureButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var directoryPath = Path.Combine(FileSystem.Current.AppDataDirectory, "images");
                Directory.CreateDirectory(directoryPath);

                savedImagePath = Path.Combine(directoryPath, photo.FileName);

                using (var stream = await photo.OpenReadAsync())
                using (var fileStream = File.OpenWrite(savedImagePath))
                {
                    await stream.CopyToAsync(fileStream);
                }


            }
            else
            {
                await DisplayAlert("Error", "No photo was captured.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


    private void SaveProductButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(savedImagePath))
        {
            DisplayAlert("Error", "No image captured", "OK");
            return;
        }

        var product = new Product
        {
            Title = TitleEntry.Text,
            Description = DescriptionEditor.Text,
            ImagePath = savedImagePath
        };

        database.Insert(product);

        TitleEntry.Text = "";
        DescriptionEditor.Text = "";
        savedImagePath = "";

        LoadProducts();
    }

    private void LoadProducts()
    {
        var products = database.Table<Product>().ToList();
        ProductList.ItemsSource = products;

    }

    private readonly List<string> images = new List<string>();

    private async void DeleteProductButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var selectedProduct = (Product)ProductList.SelectedItem;

            if (selectedProduct != null)
            {
                var result = await DisplayAlert("Delete", $"Are you sure you want to delete '{selectedProduct.Title}' ?", "Yes", "No");
                if (result)
                {
                    database.Delete(selectedProduct);

                    LoadProducts();
                }
            }
            else
            {
                await DisplayAlert("Error", "Please select a product to delete.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fout", ex.Message, "OK");
        }
        ProductList.SelectedItem = null;
        LoadProducts();


    }

    private async void EditProductButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (_selectedProduct == null)
            {
                await DisplayAlert("Error", "Please select a product to update.", "OK");
                return;
            }


            var result = await DisplayAlert("Update", $"Are you sure you want to update '{_selectedProduct.Title}'?", "Yes", "No");
            if (result)
            {

                _selectedProduct.Title = TitleEntry.Text;
                _selectedProduct.Description = DescriptionEditor.Text;
                _selectedProduct.ImagePath = savedImagePath;


                database.Update(_selectedProduct);

                await DisplayAlert("Success", "Product updated!", "OK");


                LoadProducts();


                ClearInputs();
                ProductList.SelectedItem = null;
                _selectedProduct = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private void ClearInputs()
    {
        TitleEntry.Text = string.Empty;
        DescriptionEditor.Text = string.Empty;
        savedImagePath = null;
    }

    private Product _selectedProduct;

    private async void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {

            _selectedProduct = (Product)e.CurrentSelection[0];


            TitleEntry.Text = _selectedProduct.Title;
            DescriptionEditor.Text = _selectedProduct.Description;
            savedImagePath = _selectedProduct.ImagePath;


            if (EnableNavigation.IsChecked)
            {
                await Navigation.PushAsync(new FullScreenImagePage(_selectedProduct.ImagePath));
            }
        }
        else
        {

            ClearInputs();
            _selectedProduct = null;
        }
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender == EnableNavigation && e.Value)
        {
            EnableNavigation.IsChecked = true;
            DisableNavigation.IsChecked = false;
        }
        else if (sender == DisableNavigation && e.Value)
        {
            EnableNavigation.IsChecked = false;
            DisableNavigation.IsChecked = true;
        }
    }
}