﻿using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(Pages.Login);
        }

        public enum Pages
        {
            Login,
            Regin,
            MainApp,
            EditProduct,
            AddProduct
        }

        public void OpenPage(Pages page)
        {
            if (page == Pages.Login)
            {
                frame.Navigate(new Login(this));
            }
            else if (page == Pages.Regin)
            {
                frame.Navigate(new Regin(this));
            }
        }
        public void OpenPage(Pages page, Users user)
        {
            if (page == Pages.MainApp)
            {
                ProductViewModel productViewModel = new ProductViewModel();
                frame.Navigate(new MainApp(this, user, productViewModel));
            }
            else if (page == Pages.AddProduct)
            {
                frame.Navigate(new AddProduct(this, user));
            }
        }
        public void OpenPage(Pages page, Products product, Users user)
        {
            frame.Navigate(new EditProduct(this, product, user));
        }
    }
}