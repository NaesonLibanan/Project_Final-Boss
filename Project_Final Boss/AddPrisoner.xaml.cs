using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddPrisoner.xaml
    /// </summary>
    public partial class AddPrisoner : Window
    {
        DataClasses1DataContext _PrisonDB = null;
        public AddPrisoner()
        {
         InitializeComponent();
      _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);

            // Populate crime descriptions (replace with your actual crimes)
            char[] sex = { 'M', 'F' };
            Sex.ItemsSource= sex;
      string[] crimeDescriptions = { "Assault", "Theft", "Fraud", "Drug Possession", "Vandalism", "Other" };
      Crime.ItemsSource = crimeDescriptions;
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

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void Camera_Click(object sender, RoutedEventArgs e)
    {
      // Implement logic to use camera (not covered in this example)
      MessageBox.Show("Camera functionality not implemented yet.");
    }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
          // Get data from TextBoxes and ComboBoxes
          string firstName = FirstName.Text;
          string middleName = MiddleName.Text;
          string lastName = LastName.Text;
            char sex = (char)Sex.SelectedItem;

            string crime = Crime.SelectedItem.ToString();
          int sentence = int.Parse(SentenceInPrision.Text); // Assuming SentenceInPrision holds a number
          DateTime selectedDate = DateOfBirthPicker.SelectedDate.Value;

          // Implement logic to handle Mugshot (replace with your implementation)
          // You might need to convert the image to a byte array for storage
          byte[] mugshotData = null; // Assuming you have code to get mugshot data
          // ... (your mugshot handling code)

          // Create a new Prisoner object
          Prisoner newPrisoner = new Prisoner
          {
            Prisoner_ID = GeneratePrisonerID(), // Implement a function to generate unique ID
            Prisoner_Surname = lastName,
            Prisoner_GivenName = firstName,
            Prisoner_MiddleName = middleName,
            Prisoner_Sex = sex,

            Date_Of_Birth = selectedDate,
            Crime_Desc = crime,
            Sentence_Years = sentence,
            Admission_Date = DateTime.Now, // Set admission date to current date
            Prisoner_Status_ID = 1, // Assuming default status ID is 1
            Mugshot = mugshotData // Add mugshot data property
          };

      // Add the new prisoner to the data context and submit changes
      _PrisonDB.Prisoners.InsertOnSubmit(newPrisoner);
      _PrisonDB.SubmitChanges();

      MessageBox.Show("Prisoner added successfully!");
    }

    // Implement a function to generate a unique Prisoner ID (modify based on your logic)
    private string GeneratePrisonerID()
    {
            // You can use LINQ to get the max ID and increment it
            int employeeCount = _PrisonDB.Prisoners.Count();
            return "P" + (employeeCount + 1).ToString().PadLeft(3, '0');
        }
  }
}