using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DatabaseLibrary;

namespace Bus_Tours_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                _pageTitle = value;
                OnPropertyChanged("PageTitle");
            }
        }

        private string _pageFile;
        public string PageFile
        {
            get { return _pageFile; }
            set
            {
                _pageFile = value;
                OnPropertyChanged("PageFile");
            }
        }
        

        public MainWindow()
        {
            WynneDatabase db = Database.Instance;
            this.PageTitle = "Customers";
            this.PageFile = "pages/CustomerList.xaml";
            this.DataContext = this;
            InitializeComponent();
        }

        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            this.PageTitle = "Customers";
            this.PageFile = "pages/CustomerList.xaml";
        }

        private void TourButton_Click(object sender, RoutedEventArgs e)
        {
            this.PageTitle = "Tours";
            this.PageFile = "pages/TourList.xaml";
        }

        private void CoachButton_Click(object sender, RoutedEventArgs e)
        {
            this.PageTitle = "Coaches";
            this.PageFile = "pages/CoachList.xaml";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}