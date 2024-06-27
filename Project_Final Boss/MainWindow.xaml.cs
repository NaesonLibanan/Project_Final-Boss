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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_Final_Boss
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext _PrisonDB = null;
        public MainWindow()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTbx.Text;
            string password = PasswordTbx.Text;

            var staffLogin = (from s in _PrisonDB.Staffs
                              where s.Staff_Username == username && s.Staff_Password == password
                              select s).FirstOrDefault();

            if (staffLogin != null)
            {
                MessageBox.Show("Login successful!");

        
                if (staffLogin.StaffRole.Role_Desc == "Prisoner Manager") 
                {
                    Intake_Officer intakeWindow = new Intake_Officer();
                    intakeWindow.Show();
                    this.Close(); 
                }
                else if (staffLogin.StaffRole.Role_Desc == "File Handler") // Add more conditions for other roles
                {
                    FileManager fileManagerWindow = new FileManager();
                    fileManagerWindow.Show();
                    this.Close(); 
                }
                else
                {
              
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void Username(object sender, RoutedEventArgs e)
        {
        }

        private void Password(object sender, RoutedEventArgs e)
        {
        }
    }
}