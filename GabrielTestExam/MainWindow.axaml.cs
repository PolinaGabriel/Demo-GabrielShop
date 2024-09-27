using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using GabrielTestExam.Models;
using System.Collections.Generic;
using System.Linq;

namespace GabrielTestExam
{
    public partial class MainWindow : Window
    {
        public List<Product> _products = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();
            cartButton.IsVisible = false;
            _products = Helper.Database.Products.ToList();
            productList.ItemsSource = _products.Select(x => new
            {
                x.ProductId,
                Photo = new Bitmap(x.ProductPhoto),
                x.ProductName,
                x.ProductDescription,
                x.ProductManufacturer,
                x.ProductPrice,
                x.ProductDiscount
            });
        }

        /// <summary>
        /// Появление кнопки перехода к корзине только при выборе товара
        /// </summary>
        /// <param name="sender">листбокс</param>
        /// <param name="selectionChangedEventArgs">выбор элемента</param>
        public void CartButtonVisibility(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (productList.SelectedItems.Count != 0)
            {
                cartButton.IsVisible = true;
            }
            else
            {
                cartButton.IsVisible = false;
            }
        }

        /// <summary>
        /// Переход к корзине
        /// </summary>
        /// <param name="sender">кнопка</param>
        /// <param name="routedEventArgs">нажатие на кнопку</param>
        public void GoToCart(object sender, RoutedEventArgs routedEventArgs)
        {
            CartWindow cartWindow = new CartWindow();
            foreach (Product product in _products)
            {
                foreach (object select in productList.Items)
                {
                    if (_products.IndexOf(product) == productList.Items.IndexOf(select) && productList.SelectedItems.IndexOf(select) >= 0)
                    {
                        cartWindow._cart.Add(product);
                    }
                }
            }
            cartWindow.Fill();
            cartWindow.Show();
            this.Close();
        }
    }
}