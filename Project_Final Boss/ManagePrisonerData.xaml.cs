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
    /// Interaction logic for ManagePrisonerData.xaml
    /// </summary>
    public partial class ManagePrisonerData : Window
    {
        DataClasses1DataContext _PrisonDB = null;
        public ManagePrisonerData()
        {
            InitializeComponent();
            _PrisonDB = new DataClasses1DataContext(Properties.Settings.Default.ColdheartPrisonConnectionString);
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

        }
    }
}
