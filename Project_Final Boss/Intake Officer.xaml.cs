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
    /// Interaction logic for Intake_Officer.xaml
    /// </summary>
    public partial class Intake_Officer : Window
    {
        DataClasses1DataContext _PrisonDB = null;
        public Intake_Officer()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);
        }

        private void AddPrisoner_Click(object sender, RoutedEventArgs e)
        {
            AddPrisoner addPrisonerWindow = new AddPrisoner();
            addPrisonerWindow.Show();
            this.Close();
        }

        private void ManagePrisoner_Click(object sender, RoutedEventArgs e)
        {
            ManagePrisonerData managePrisonerDataWindow = new ManagePrisonerData();
            managePrisonerDataWindow.Show();
            this.Close();
        }

        private void ViewPrisoner_Click(object sender, RoutedEventArgs e)
        {
            ViewPrisoner viewPrisonerWindow = new ViewPrisoner();
            viewPrisonerWindow.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}