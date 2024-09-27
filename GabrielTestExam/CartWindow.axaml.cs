using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GabrielTestExam.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Avalonia.Media.Imaging;
using Avalonia.Media;

namespace GabrielTestExam;

public partial class CartWindow : Window
{
    public List<Product> _cart = new List<Product>();

    public CartWindow()
    {
        InitializeComponent();
        downloadMessage.IsVisible = false;
    }

    /// <summary>
    /// Заполнение листбокса
    /// </summary>
    public void Fill()
    {
        cartList.ItemsSource = _cart.ToList().Select(x => new
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
    /// Удаление товара из корзины
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Delete(object sender, RoutedEventArgs routedEventArgs)
    {
        _cart.Remove(Helper.Database.Products.FirstOrDefault(x => x.ProductId == (int)(sender as Button).Tag));
        Fill();
        if (_cart.Count() == 0)
        {
            pointBox.IsEnabled = false; 
            orderButton.IsEnabled = false;
        }
    }

    /// <summary>
    /// Возвращение к каталогу товаров
    /// </summary>
    /// <param name="sender">кнопка</param>
    /// <param name="routedEventArgs">нажатие на кнопку</param>
    public void GoToCatalogue(object sender, RoutedEventArgs routedEventArgs)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    /// <summary>
    /// Оформление заказа
    /// </summary>
    /// <param name="sender">кнопка</param>
    /// <param name="routedEventArgs">нажатие на кнопку</param>
    public void MakeOrder(object sender, RoutedEventArgs routedEventArgs)
    {
        if (pointBox.SelectedIndex == 0)
        {
            downloadMessage.Text = "Выберите пункт выдачи";
            downloadMessage.Foreground = Brush.Parse("Red");
            downloadMessage.IsVisible = true;
        }
        else
        {
            string orderPoint = "";
            switch (pointBox.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    orderPoint = "Пункт-0";
                    break;
                case 2:
                    orderPoint = "Пункт-1";
                    break;
                case 3:
                    orderPoint = "Пункт-2";
                    break;
            }
            Random random = new Random();
            double orderPrice = 0;
            foreach (Product product in _cart)
            {
                if (product.ProductDiscount == 0)
                {
                    orderPrice += product.ProductPrice;
                }
                else
                {
                    orderPrice += product.ProductPrice - (product.ProductPrice * ((double)product.ProductDiscount / 100));
                }
            }
            Order order = new Order()
            {
                OrderStatus = "новый",
                OrderDate = DateTime.Now.Date,
                OrderDeliver = 6,
                OrderPoint = orderPoint,
                OrderCode = Convert.ToString(random.Next(0, 9)) + Convert.ToString(random.Next(0, 9)) + Convert.ToString(random.Next(0, 9)),
                OrderPrice = (float)(orderPrice)
            };
            Helper.Database.Orders.Add(order);
            Helper.Database.SaveChanges();
            foreach (Product product in _cart)
            {
                OrderAndProduct orderAndProduct = new OrderAndProduct()
                {
                    OrderId = Helper.Database.Orders.OrderBy(x => x.OrderId).Last().OrderId,
                    ProductId = product.ProductId,
                    PriceWithoutDiscount = product.ProductPrice,
                    PriceWithDiscount = (float)(product.ProductPrice - (product.ProductPrice * ((double)product.ProductDiscount / 100)))
                };
                Helper.Database.OrderAndProducts.Add(orderAndProduct);

            }
            Helper.Database.SaveChanges();
            StreamWriter streamWriter = new StreamWriter("Asset/Order" + Helper.Database.Orders.OrderBy(x => x.OrderId).Last().OrderId + ".txt");
            streamWriter.WriteLine("Заказ номер " + Helper.Database.Orders.OrderBy(x => x.OrderId).Last().OrderId);
            streamWriter.WriteLine("Статус: " + order.OrderStatus);
            streamWriter.WriteLine("Срок доставки (дни): " + order.OrderDeliver);
            streamWriter.WriteLine("Пункт выдачи: " + order.OrderPoint);
            streamWriter.WriteLine();
            foreach (Product p in _cart)
            {
                streamWriter.WriteLine((_cart.IndexOf(p) + 1) + ". " + p.ProductName);
                streamWriter.WriteLine("Без скидки: " + p.ProductPrice + "р. ");
                streamWriter.WriteLine("Со скидкой: " + (p.ProductPrice - (p.ProductPrice * ((double)p.ProductDiscount / 100))) + "р.");
                streamWriter.WriteLine();
            }
            streamWriter.WriteLine("Итого: " + order.OrderPrice + "р.");
            streamWriter.WriteLine("Код: " + order.OrderCode);
            streamWriter.Close();
            downloadMessage.Text = "Талон загружен в папку Asset";
            downloadMessage.Foreground = Brush.Parse("DarkBlue");
            downloadMessage.IsVisible = true;
            pointBox.IsEnabled = false;
            orderButton.IsEnabled = false;
        }
    }
}