using System;
using System.Collections.Generic;
using System.Data.Linq;
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
        string[] statusDescriptions = {
            "Admitted", "Under Investigation", "Sentenced", "Released",
            "Transferred", "On Parole", "Escaped"
        };

        public ManagePrisonerData()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);
            LoadPrisoners();

            char[] sex = { 'M', 'F' };
            Sex.ItemsSource = sex;
            string[] crimeDescriptions = { "Assault", "Theft", "Fraud", "Drug Possession", "Vandalism", "Other" };
            Crime.ItemsSource = crimeDescriptions;
            Status.ItemsSource = statusDescriptions;
           
            Crime.IsEnabled = false;
        }

        private void LoadPrisoners()
        {
            var prisoners = _PrisonDB.Prisoners.ToList();
            Prisoners.ItemsSource = prisoners;
        }

        private void ComboBox_Prisoners(object sender, SelectionChangedEventArgs e)
        {
            Prisoner selectedPrisoner = (Prisoner)Prisoners.SelectedItem;

            if (selectedPrisoner != null)
            {
                FirstName.Text = selectedPrisoner.Prisoner_GivenName;
                MiddleName.Text = selectedPrisoner.Prisoner_MiddleName;
                LastName.Text = selectedPrisoner.Prisoner_Surname;
                SentenceInPrision.Text = selectedPrisoner.Sentence_Years.ToString();

                Crime.SelectedItem = selectedPrisoner.Crime_Desc;
                Status.SelectedItem = selectedPrisoner.PrisonerStatus.Status_Desc;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged4(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_CrimeChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Prisoner selectedPrisoner = (Prisoner)Prisoners.SelectedItem;

            if (selectedPrisoner == null)
            {
                MessageBox.Show("Please select a prisoner to edit.");
                return;
            }

            // Update Prisoner Details
            selectedPrisoner.Prisoner_GivenName = FirstName.Text;
            selectedPrisoner.Prisoner_MiddleName = MiddleName.Text;
            selectedPrisoner.Prisoner_Surname = LastName.Text;

            if (int.TryParse(SentenceInPrision.Text, out int years))
            {
                selectedPrisoner.Sentence_Years = years;
            }
            else
            {
                MessageBox.Show("Invalid sentence format.");
                return;
            }

            if (years <= 0 || years > 300)
            {
                MessageBox.Show("Sentence must be more than zero and less than or equal to 300 years.");
                return;
            }
            else 


            selectedPrisoner.Crime_Desc = Crime.SelectedItem?.ToString();

            var selectedStatus = Status.SelectedItem as PrisonerStatus;
          
          
            try
            {
                _PrisonDB.SubmitChanges();
                MessageBox.Show("Prisoner details updated successfully.");
                LoadPrisoners(); 
            }
            catch (ChangeConflictException)
            {
                _PrisonDB.Refresh(RefreshMode.KeepCurrentValues, selectedPrisoner);
                MessageBox.Show("A conflict occurred. Changes could not be saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}