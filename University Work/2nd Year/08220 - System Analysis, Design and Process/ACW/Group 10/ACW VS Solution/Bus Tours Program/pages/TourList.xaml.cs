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
using DatabaseLibrary;

namespace Bus_Tours_Program
{
    /// <summary>
    /// Interaction logic for TourList.xaml
    /// </summary>
    public partial class TourList : Page
    {
        private VMTourList vm;
        public TourList()
        {
            InitializeComponent();
            vm = new VMTourList();
            this.DataContext = vm;
        }

        private void ToursListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Prevents multi-selections
            if (TourListView.SelectedItems.Count > 1)
                TourListView.SelectedItems.RemoveAt(0);
        }

        private void AddTourButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddNewTour();
            TourListView.SelectedIndex = vm.TourList.Count - 1;
        }

        private void SaveSelectedTourBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveSelectedTour();
            var idx = TourListView.SelectedIndex;
            TourListView.SelectedIndex = -1;
            TourListView.SelectedIndex = idx;
        }

        private void DeleteSelectedTourBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteSelectedTour();
        }
    }
}
