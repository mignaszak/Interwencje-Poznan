using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Interwencje___Poznań.WebService;
using Interwencje___Poznań.Helpers;

namespace Interwencje___Poznań.Pages
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        private Categories categories;

        public DetailsPage()
        {
            InitializeComponent();
            GetAndSetCategories();

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

        }



        private Categories GetAndSetCategories()
        {
            Categories cats = DataMemory.LastCategories;
            try
            {
                if (WSMethods.CheckNetworkConnection())
                {
                    WSMethods.GetMethodNoAuth("http://www.poznan.pl/mim/api/query.html?service=fixmycity", HandlerSetCategories);            
                }
                else if (cats.categories == null)
                    MessageBox.Show("Nie można pobrać kategorii. Połącz się z internetem");
                else
                {
                    SetCategories(cats);
                }
            }
            catch (Exception)
            {
                    MessageBox.Show("Nie można pobrać kategorii.");
            }
            return cats;
        }

        private void HandlerSetCategories(object sender, DownloadStringCompletedEventArgs e)
        {
            Categories cats = Deserializers.DesrielizeCategories(e.Result.ToString());
            SetCategories(cats);
            DataMemory.LastCategories = cats;
            DataMemory.SaveCategories(delegate
            {
                MessageBox.Show(@"Za mało pamięci na telefonie, aby zapisać dane.
                                      Zwolnij trochę miejsca i spróbuj ponownie.");
            });
        }

        private void SetCategories(Categories cats)
        {
            categories = cats;
            List<Category> sss = cats.categories.ToList();
            PickerCategory.ItemsSource = cats.categories.ToList();
        }
        private void SetSubcategories(Category[] cats)
        {
            PickerSubcategory.ItemsSource = cats.ToList();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SenderPage.xaml", UriKind.Relative));
        }

        private void PickerCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Category selectedCat = (Category)PickerCategory.SelectedItem;
                if(selectedCat != null)
                    SetSubcategories(selectedCat.subcategories);
            }
            catch(Exception sdfsdfe)
            {
                
               // throw;
            }

        }
    }
    
}