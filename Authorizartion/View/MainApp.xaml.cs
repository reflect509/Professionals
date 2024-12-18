﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DishesCompany
{
    /// <summary>
    /// Логика взаимодействия для MainAppAdmin.xaml
    /// </summary>
    public partial class MainApp : Page
    {
        public MainWindow mainWindow;
        private Users user;
        public MainApp(MainWindow mainwindow, Users user, ProductViewModel productViewModel)
        {
            InitializeComponent();

            this.mainWindow = mainwindow;
            this.DataContext = productViewModel;
            this.user = user;

            FullName.Text = user.Full_name;

            if (user.Role_id != 3)
            {
                AddProductButton.Visibility = Visibility.Collapsed;
                EditProductButton.Visibility = Visibility.Collapsed;
                DeleteProductButton.Visibility = Visibility.Collapsed;
            }
            RemainingsCheck();
        }


        private void ExitClick(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.Pages.Login);
        }

        private void RemainingsCheck()
        {
            foreach(ItemCollection item in ProductListView.Items)
            {
                ProductListView.Items.IndexOf(item);
                
            }

        }

        private void SortAscButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            string selectedManufacturer = ManufacturerComboBox.SelectedItem?.ToString();

            List<Products> sortedAndFilteredProducts =
                SearchAndFilterProducts(searchText, selectedManufacturer);

            ProductViewModel productViewModel = (ProductViewModel)this.DataContext;
            productViewModel.Products = new ObservableCollection<Products>(DatabaseControl.SortingProducts("asc", sortedAndFilteredProducts));
            productViewModel.CurrentProductCount = DatabaseControl.GetCount(productViewModel.Products.ToList());
        }

        private void SortDescButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            string selectedManufacturer = ManufacturerComboBox.SelectedItem?.ToString();

            List<Products> sortedAndFilteredProducts =
                SearchAndFilterProducts(searchText, selectedManufacturer);

            ProductViewModel productViewModel = (ProductViewModel)this.DataContext;
            productViewModel.Products = new ObservableCollection<Products>(DatabaseControl.SortingProducts("desc", sortedAndFilteredProducts));
            productViewModel.CurrentProductCount = DatabaseControl.GetCount(productViewModel.Products.ToList());
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedItem != null)
            {
                Products product = (Products)ProductListView.SelectedItem;
                string articul = product.Articul;

                if (DatabaseControl.ValidProduct(articul))
                {
                    ProductViewModel productViewModel = (ProductViewModel)this.DataContext;
                    productViewModel.Products.Remove(product);

                    DatabaseControl.DeleteProduct(product);

                    productViewModel.CurrentProductCount = DatabaseControl.GetCount(productViewModel.Products.ToList());
                    productViewModel.TotalProductCount = DatabaseControl.GetCount();
                    MessageBox.Show("Удаление товара успешно");
                }
                else
                {
                    MessageBox.Show("Данный товар присутствует в заказе");
                }

            }
            else
            {
                MessageBox.Show("Товар не выбран");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedItem != null)
            {
                Products product = (Products)ProductListView.SelectedItem;
                mainWindow.OpenPage(MainWindow.Pages.EditProduct, product, user);
            }
            else
            {
                MessageBox.Show("Товар не выбран");
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.Pages.AddProduct, user);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            string selectedManufacturer = ManufacturerComboBox.SelectedItem?.ToString();


            ProductViewModel productViewModel = (ProductViewModel)this.DataContext;
            productViewModel.Products = new ObservableCollection<Products>(SearchAndFilterProducts(searchText, selectedManufacturer));
            productViewModel.CurrentProductCount = DatabaseControl.GetCount(productViewModel.Products.ToList());
        }

        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            string selectedManufacturer = ManufacturerComboBox.SelectedItem?.ToString();

            ProductViewModel productViewModel = (ProductViewModel)this.DataContext;
            productViewModel.Products = new ObservableCollection<Products>(SearchAndFilterProducts(searchText, selectedManufacturer));
            productViewModel.CurrentProductCount = DatabaseControl.GetCount(productViewModel.Products.ToList());
        }

        private List<Products> SearchAndFilterProducts(string searchText, string selectedManufacturer)
        {
            List<Products> searchedProducts = SearchProducts(searchText);

            if (selectedManufacturer != null && selectedManufacturer != "Все производители")
            {
                searchedProducts = DatabaseControl.FilteredProducts(selectedManufacturer, searchedProducts);

                return searchedProducts;
            }
            else
            {
                return searchedProducts;
            }
        }

        private List<Products> SearchProducts(string searchText)
        {
            List<Products> searchedProducts = new List<Products>();

            List<Products> products = DatabaseControl.GetProducts();

            foreach (Products product in products)
            {
                if (product.Product_name.ToLower().Contains(searchText) ||
                    product.Description.ToLower().Contains(searchText) ||
                    product.Manufacturer.ToLower().Contains(searchText))
                {
                    searchedProducts.Add(product);
                }
            }

            return searchedProducts;
        }
    }
}