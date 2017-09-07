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
using Bus_Tours_Program.viewmodels;

namespace Bus_Tours_Program
{
    /// <summary>
    /// Interaction logic for CoachList.xaml
    /// </summary>
    public partial class CoachList : Page
    {
        private VMCoachList vm;
        public CoachList()
        {
            InitializeComponent();
            vm = new VMCoachList();
            this.DataContext = vm;
        }

        private void AddCoachButton_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfSeatsTextbox.Text.Length == 0 || RegistrationTextbox.Text.Length == 0) return;
            int numSeats;
            if (int.TryParse(NumberOfSeatsTextbox.Text, out numSeats) == false) return;
            vm.AddNewCoach(numSeats, RegistrationTextbox.Text);
            CoachListView.SelectedIndex = CoachListView.Items.Count - 1;
            NumberOfSeatsTextbox.Text = "";
            RegistrationTextbox.Text = "";
        }

        private void DeleteCoachButton_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteSelectedCoach();
        }

        private void CoachListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Prevents multi-selection
            if (CoachListView.SelectedItems.Count > 1)
                CoachListView.SelectedItems.RemoveAt(0);
        }

        
    }
}
