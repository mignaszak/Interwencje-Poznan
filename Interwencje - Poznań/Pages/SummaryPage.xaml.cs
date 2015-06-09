﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Interwencje___Poznań.Helpers;
using Interwencje___Poznań.WebService;

namespace Interwencje___Poznań.Pages
{
    public partial class SummaryPage : PhoneApplicationPage
    {
        public SummaryPage()
        {
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private string GetCategoryName(string categoryId)
        {
            foreach (Category c in DataMemory.LastCategories.categories)
                if (c.id == categoryId)
                    return c.title;
            return "";
        }
        private string GetSubcategoryName(string subcategoryId)
        {
            foreach (Category c in DataMemory.LastCategories.categories)
                foreach(Category s in c.subcategories)
                if (s.id == subcategoryId)
                    return s.title;
            return "";
        }

        private void FillInterventionInfos()
        {
            TextBlockSubject.Text = Intervention.GetCurrentIntervention().Subject;
            TextBlockDescription.Text = Intervention.GetCurrentIntervention().Text;
            TextBlockCategory.Text = GetCategoryName(Intervention.GetCurrentIntervention().Category);
            TextBlockSubcategory.Text = GetCategoryName(Intervention.GetCurrentIntervention().Subcategory);

            TextBlockAddress.Text = Intervention.GetCurrentIntervention().Address;
            TextBlockCoordinates.Text = Intervention.GetCurrentIntervention().Latitude + ";" + Intervention.GetCurrentIntervention().Longitude;

            Photo.Source = Intervention.GetCurrentIntervention().Picture;

            TextBlockFirstname.Text = Intervention.GetCurrentIntervention().Name;
            TextBlockSecondname.Text = Intervention.GetCurrentIntervention().Secondname;
            TextBlockMail.Text = Intervention.GetCurrentIntervention().Email;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/Pages/SummaryPage.xaml", UriKind.Relative));
        }

        private void SaveIntervention()
        {
            DataMemory.LastIntervention = Intervention.GetCurrentIntervention();
            
            DataMemory.SaveIntervention(delegate
            {
                MessageBox.Show(@"Za mało pamięci na telefonie, aby zapisać dane.
                                      Zwolnij trochę miejsca i spróbuj ponownie.");
            });
        }

        private void EndOfSaving(bool success)
        {
            if (success)
                MessageBox.Show(@"Pomyślnie zapisano");
            else
                MessageBox.Show(@"Za mało pamięci na telefonie, aby zapisać dane.
                                      Zwolnij trochę miejsca i spróbuj ponownie.");
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}