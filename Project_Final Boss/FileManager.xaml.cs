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
    /// Interaction logic for FileManager.xaml
    /// </summary>
    public partial class FileManager : Window
    {
        DataClasses1DataContext _PrisonDB = null;
        public FileManager()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


        private void ProcessRecord_Click(object sender, RoutedEventArgs e)
        {

            ProcessRecord process = new ProcessRecord();
            process.Show();
            this.Close();
        }

        private void UploadAFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewPrisoner_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
