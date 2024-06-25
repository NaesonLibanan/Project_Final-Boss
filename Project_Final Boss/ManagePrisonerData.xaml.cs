using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Project_Final_Boss
{
    /// <summary>
    /// Interaction logic for ManagePrisonerData.xaml
    /// </summary>
    public partial class ManagePrisonerData : Window
    {
        DataClasses1DataContext _PrisonDB = null;
        Prisoner selectedPrisoner = null;

        public ManagePrisonerData()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);

            LoadPrisoners();
        }

        private void LoadPrisoners()
        {
            try
            {
                var prisoners = _PrisonDB.Prisoners.ToList();
                Prisoners.ItemsSource = prisoners;
                Prisoners.DisplayMemberPath = "Given_Name";
                Prisoners.SelectedValuePath = "Prisoner_ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading prisoners: {ex.Message}");
            }
        }

        private void Prisoners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Prisoners.SelectedItem != null)
            {
                string selectedPrisonerId = (string)Prisoners.SelectedValue;
                DisplayPrisonerDetails(selectedPrisonerId);
                DisplayPrisonerPhoto(selectedPrisonerId);
            }
        }

        private void DisplayPrisonerDetails(string prisonerId)
        {
            var prisoner = _PrisonDB.Prisoners.FirstOrDefault(p => p.Prisoner_ID == prisonerId);
            if (prisoner != null)
            {
                FirstName.Text = prisoner.Prisoner_GivenName;
                MiddleName.Text = prisoner.Prisoner_MiddleName;
                LastName.Text = prisoner.Prisoner_Surname;
                Sex.SelectedItem = prisoner.Prisoner_Sex;
                DateOfBirthPicker.SelectedDate = prisoner.Date_Of_Birth;
                SentenceInPrision.Text = prisoner.Sentence_Years.ToString();
                Crime.SelectedItem = prisoner.Crime_Desc;
            }
        }

        private void DisplayPrisonerPhoto(string prisonerId)
        {
            try
            {
                // Fetch prisoner's photo path from database
                var prisoner = _PrisonDB.Prisoners.FirstOrDefault(p => p.Prisoner_ID == prisonerId);
                if (prisoner != null && !string.IsNullOrEmpty(prisoner.Mugshot_Path) && File.Exists(prisoner.Mugshot_Path))
                {
                    // Load image from file path
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(prisoner.Mugshot_Path, UriKind.Absolute);
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    ImageCapture.Source = bi;
                }
                else
                {
                    ImageCapture.Source = null; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying prisoner photo: {ex.Message}");
            }
        }

        private void Photochange_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Camera functionality not implemented yet.");
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update selected prisoner's details
                selectedPrisoner.Prisoner_GivenName = FirstName.Text.Trim();
                selectedPrisoner.Prisoner_MiddleName = MiddleName.Text.Trim();
                selectedPrisoner.Prisoner_Surname = LastName.Text.Trim();
                selectedPrisoner.Prisoner_Sex = (char)Sex.SelectedValue;
                selectedPrisoner.Date_Of_Birth = DateOfBirthPicker.SelectedDate.Value;
                selectedPrisoner.Crime_Desc = Crime.SelectedValue.ToString();
                selectedPrisoner.Mugshot_Path = ""; 
                _PrisonDB.SubmitChanges();
                MessageBox.Show("Prisoner details updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating prisoner details: {ex.Message}");
            }
        }

        // Event handlers for text changed and selection changed can remain as they are, unless you need to add specific functionality.

         private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MiddleName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LastName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Sex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SentenceInPrision_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Crime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}