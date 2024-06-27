using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Win32;

namespace Project_Final_Boss
{
    /// <summary>
    /// Interaction logic for AddPrisoner.xaml
    /// </summary>
    public partial class AddPrisoner : Window
    {
        DataClasses1DataContext _PrisonDB = null;
        VideoCaptureDevice vcd = null;
        public AddPrisoner()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);

            // Populate the Sex ComboBox
            char[] sex = { 'M', 'F' };
            Sex.ItemsSource = sex;

            // Populate the Crime ComboBox
            string[] crimeDescriptions = { "Assault", "Theft", "Fraud", "Drug Possession", "Vandalism", "Other" };
            Crime.ItemsSource = crimeDescriptions;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e) { }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e) { }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e) { }

        private void TextBox_TextChanged4(object sender, TextChangedEventArgs e) { }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void Camera_Click(object sender, RoutedEventArgs e)
        {
            // Initialize camera selection and start capturing
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }

            // Select the first video device
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(HandleCameraFrame);
            videoSource.Start();
        }

        private void HandleCameraFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Update the image control (pic) with the latest frame from the camera
            Dispatcher.Invoke(() =>
            {
                pic.Source = BitmapToImageSource((Bitmap)eventArgs.Frame.Clone());
            });
        }

        private BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        private void CaptureImage()
        {
            BitmapSource bitmapSource = (BitmapSource)pic.Source;
            Bitmap bitmap = BitmapSourceToBitmap(bitmapSource);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(fileStream);
                }
            }
        }
        private Bitmap BitmapSourceToBitmap(BitmapSource bitmapSource)
        {
            Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
                bitmap = new Bitmap(stream);
            }
            return bitmap;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vcd != null && vcd.IsRunning)
            {
                vcd.SignalToStop();
                vcd.WaitForStop();
                vcd = null;
            }
        }
        private byte[] BitmapSourceToByteArray(BitmapSource bitmapSource)
        {
            byte[] data;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values
            string firstName = FirstName.Text;
            string middleName = MiddleName.Text;
            string lastName = LastName.Text;
            char sex = (char)Sex.SelectedItem;
            string crime = Crime.SelectedItem.ToString();
            int sentence = int.Parse(SentenceInPrision.Text);
            DateTime selectedDate = DateOfBirthPicker.SelectedDate.Value;

            // Placeholder for mugshot data
            byte[] mugshotData = null;

            // If there's a captured image, convert and save it
            if (pic.Source != null)
            {
                BitmapSource bitmapSource = (BitmapSource)pic.Source;
                mugshotData = BitmapSourceToByteArray(bitmapSource);
            }

            // Create new prisoner object
            Prisoner newPrisoner = new Prisoner
            {
                Prisoner_ID = GeneratePrisonerID(),
                Prisoner_Surname = lastName,
                Prisoner_GivenName = firstName,
                Prisoner_MiddleName = middleName,
                Prisoner_Sex = sex,
                Date_Of_Birth = selectedDate,
                Crime_Desc = crime,
                Sentence_Years = sentence,
                Admission_Date = DateTime.Now,
                Prisoner_Status_ID = 1, // Assuming 1 is the status for 'Admitted'
                Mugshot = mugshotData
            };

            // Insert new prisoner into the database
            _PrisonDB.Prisoners.InsertOnSubmit(newPrisoner);
            _PrisonDB.SubmitChanges();

            MessageBox.Show("Prisoner added successfully!");
        }

        private string GeneratePrisonerID()
        {
            // Generate a new Prisoner_ID
            int prisonerCount = _PrisonDB.Prisoners.Count();
            return "P" + (prisonerCount + 1).ToString().PadLeft(3, '0');
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CaptureImage_Click(object sender, RoutedEventArgs e)
        {
            CaptureImage();

        }

        private void Closecam_Click(object sender, RoutedEventArgs e)
        {
       
        }
    }
}
           