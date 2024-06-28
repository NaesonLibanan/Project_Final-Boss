using Microsoft.Win32;
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
using System.IO;
using System.Data.Linq;

namespace Project_Final_Boss
{

    /// <summary>
    /// Interaction logic for ProcessRecord.xaml
    /// </summary>
    public partial class ProcessRecord : Window

    {
        DataClasses1DataContext _PrisonDB = null;
        public ProcessRecord()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);
            PrisonerComboBox.ItemsSource = _PrisonDB.Prisoners.ToList();
            StaffComboBox.ItemsSource = _PrisonDB.Staffs.ToList();
            ProcessingTypeComboBox.ItemsSource = new string[] { "Medical", "Admission", "Disciplinary", "Document", "Other" };
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Input Validation
            if (PrisonerComboBox.SelectedItem == null ||
                StaffComboBox.SelectedItem == null ||
                ProcessingDatePicker.SelectedDate == null ||
                ProcessingTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Create a new ProcessingRecord object
            ProcessingRecord newRecord = new ProcessingRecord();
            newRecord.ProcessingRecord_ID = GenerateProcessingRecordID(); 
            newRecord.Prisoner_ID = (PrisonerComboBox.SelectedItem as Prisoner).Prisoner_ID;
            newRecord.Staff_ID = (StaffComboBox.SelectedItem as Staff).Staff_ID;
            newRecord.Processing_Date = ProcessingDatePicker.SelectedDate.Value;
            newRecord.Processing_Type = ProcessingTypeComboBox.SelectedItem.ToString();
            newRecord.Document_Description = DocumentDescriptionTextBox.Text;

            // Handle Document Upload (if applicable)
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                newRecord.Document = File.ReadAllBytes(openFileDialog.FileName);
            }

            // Insert the new record into the database
            try
            {
                _PrisonDB.ProcessingRecords.InsertOnSubmit(newRecord);
                _PrisonDB.SubmitChanges();
                MessageBox.Show("Processing record added successfully.");
                LoadProcessingRecords(); // Refresh the DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding record: {ex.Message}");
                // Log the error or take appropriate action
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Get Selected Record:
            ProcessingRecord selectedRecord = RecordsDataGrid.SelectedItem as ProcessingRecord;
            if (selectedRecord == null)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            // 2. Input Validation:
            if (PrisonerComboBox.SelectedItem == null ||
                StaffComboBox.SelectedItem == null ||
                ProcessingDatePicker.SelectedDate == null ||
                ProcessingTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // 3. Update Record Properties:
            selectedRecord.Prisoner_ID = (PrisonerComboBox.SelectedItem as Prisoner).Prisoner_ID;
            selectedRecord.Staff_ID = (StaffComboBox.SelectedItem as Staff).Staff_ID;
            selectedRecord.Processing_Date = ProcessingDatePicker.SelectedDate.Value;
            selectedRecord.Processing_Type = ProcessingTypeComboBox.SelectedItem.ToString();
            selectedRecord.Document_Description = DocumentDescriptionTextBox.Text;

            // 4. Handle Document Update (If Needed):
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                selectedRecord.Document = File.ReadAllBytes(openFileDialog.FileName);
            }


            try
            {
                _PrisonDB.SubmitChanges();
                MessageBox.Show("Processing record updated successfully.");
                LoadProcessingRecords(); 
            }
            catch (ChangeConflictException) 
            {
                _PrisonDB.Refresh(RefreshMode.KeepCurrentValues, selectedRecord);
                MessageBox.Show("Another user has modified this record. Please reload and try again.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating record: {ex.Message}");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FileManager fileManagerWindow = new FileManager();
            fileManagerWindow.Show();
            this.Close();
        }


        private void PrisonerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProcessingRecords();
        }

        private void StaffComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProcessingRecords();
        }

        private void LoadProcessingRecords()
        {
            var prisonerId = (PrisonerComboBox.SelectedItem as Prisoner)?.Prisoner_ID;
            var staffId = (StaffComboBox.SelectedItem as Staff)?.Staff_ID;

            if (prisonerId != null || staffId != null)
            {
                var records = _PrisonDB.ProcessingRecords
                    .Where(r => (prisonerId == null || r.Prisoner_ID == prisonerId) &&
                                (staffId == null || r.Staff_ID == staffId))
                    .ToList();

                RecordsDataGrid.ItemsSource = records;
                UpdateButton.IsEnabled = records.Any();
            }
            else
            {
                RecordsDataGrid.ItemsSource = null;
                UpdateButton.IsEnabled = false;
            }
        }
        private string GenerateProcessingRecordID()
        {
          
            string maxExistingId = _PrisonDB.ProcessingRecords
                                     .Select(r => r.ProcessingRecord_ID)
                                     .OrderByDescending(id => id)
                                     .FirstOrDefault();
            int nextNumber = 1; 

            if (!string.IsNullOrEmpty(maxExistingId) && maxExistingId.StartsWith("R") &&
                int.TryParse(maxExistingId.Substring(1), out int currentNumber))
            {
                nextNumber = currentNumber + 1; 
            }

        
            string newRecordId = "R" + nextNumber.ToString().PadLeft(3, '0');

            return newRecordId;
        }
    }
}


