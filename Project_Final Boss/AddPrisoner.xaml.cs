using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;



namespace Project_Final_Boss
{
    /// <summary>
    /// Interaction logic for AddPrisoner.xaml
    /// </summary>
    /// 


    public partial class AddPrisoner : Window

    {

        DataClasses1DataContext _PrisonDB = null;
        VideoCaptureDevice videoSource = null;
        Bitmap capturedImage = null;

        public AddPrisoner()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);

            // Populate crime descriptions
            char[] sex = { 'M', 'F' };
            Sex.ItemsSource = sex;
            string[] crimeDescriptions = { "Assault", "Theft", "Fraud", "Drug Possession", "Vandalism", "Other" };
            Crime.ItemsSource = crimeDescriptions;

            InitializeCamera(); // Initialize camera on window load
        }

        private void InitializeCamera()
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString); // Use the first available camera
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("No video input devices found.");
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                capturedImage = (Bitmap)eventArgs.Frame.Clone();

                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        capturedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = ms;
                        bi.CacheOption = BitmapCacheOption.OnLoad; // Improve performance
                        bi.EndInit();
                        ImageCapture.Source = bi; // Display captured frame in Image control
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Camera_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (capturedImage != null)
                {
                    // Save captured image to a file
                    string imagePath = SaveImageToFile(capturedImage);

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        MessageBox.Show($"Image saved successfully: {imagePath}");
                        // Save image path to your database
                        // For now, just display the path
                        MessageBox.Show($"Image Path: {imagePath}");
                    }
                    else
                    {
                        MessageBox.Show("Failed to save image.");
                    }
                }
                else
                {
                    MessageBox.Show("No image captured.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing image: {ex.Message}");
            }
        }

        private string SaveImageToFile(Bitmap image)
        {
            string imagePath = ""; // Path to save the image file
            try
            {
                // Define your image file path and name
                string fileName = $"mugshot_{DateTime.Now:yyyyMMddHHmmss}.bmp";
                string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), fileName);

                // Save the bitmap to the specified file
                image.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);

                imagePath = path; // Store the path for database storage or further use
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving image: {ex.Message}");
            }
            return imagePath;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = FirstName.Text.Trim();
                string middleName = MiddleName.Text.Trim();
                string lastName = LastName.Text.Trim();
                char? sex = (char?)Sex.SelectedItem;
                int sentence = int.Parse(SentenceInPrision.Text);
                string crime = Crime.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || sex == null || string.IsNullOrWhiteSpace(crime))
                {
                    MessageBox.Show("Please fill in all required fields (First Name, Last Name, Sex, Crime).");
                    return;
                }


                if (!int.TryParse(SentenceInPrision.Text, out sentence) || sentence <= 0 || sentence > 300)

                {
                    MessageBox.Show("Sentence years must be a positive integer not exceeding 300.");
                    return;
                }

                DateTime? selectedDate = DateOfBirthPicker.SelectedDate;
                if (selectedDate == null)
                {
                    MessageBox.Show("Please select a valid date of birth.");
                    return;
                }

                // Save the prisoner record to database
                Prisoner newPrisoner = new Prisoner
                {
                    Prisoner_ID = GeneratePrisonerID(),
                    Prisoner_Surname = lastName,
                    Prisoner_GivenName = firstName,
                    Prisoner_MiddleName = middleName,
                    Prisoner_Sex = sex.Value,
                    Date_Of_Birth = selectedDate.Value,
                    Crime_Desc = crime,
                    Sentence_Years = sentence,
                    Admission_Date = DateTime.Now,
                    Prisoner_Status_ID = 1,
                    Mugshot_Path = SaveImageToFile(capturedImage) // Save image path to database
                };

                _PrisonDB.Prisoners.InsertOnSubmit(newPrisoner);
                _PrisonDB.SubmitChanges();

                MessageBox.Show("Prisoner added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding prisoner: {ex.Message}");
            }
        }

        private string GeneratePrisonerID()
        {
            int prisonerCount = _PrisonDB.Prisoners.Count();
            return "P" + (prisonerCount + 1).ToString().PadLeft(3, '0');
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
    }
}

     