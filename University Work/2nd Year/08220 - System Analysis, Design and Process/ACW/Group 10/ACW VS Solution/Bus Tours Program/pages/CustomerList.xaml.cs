using System.Windows;
using System.Windows.Controls;
using Bus_Tours_Program.viewmodels;

namespace Bus_Tours_Program
{
    /// <summary>
    /// Interaction logic for CustomerList.xaml
    /// </summary>
    public partial class CustomerList : Page
    {
        private VMCustomerList vm;
        public CustomerList()
        {
            InitializeComponent();
            vm = new VMCustomerList();
            this.DataContext = vm;
        }

        private void AddCustButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddNewCustomer();
            CustListView.SelectedIndex = vm.CustomerList.Count - 1;
        }

        private void SaveSelectedUsrBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveSelectedUser();
        }

        private void DeleteSelectedUsrBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteSelectedUser();
        }

        private void CustListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Prevents multi-selection
            if (CustListView.SelectedItems.Count > 1)
                CustListView.SelectedItems.RemoveAt(0);
        }

        private void BookTicketsBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.BookTickets();
        }

        private void GoldSelectedUsrBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.ToggleGoldSelectedUser();
            //Hacky way of refreshing the datatemplate
            var idx = CustListView.SelectedIndex;
            CustListView.SelectedIndex = -1;
            CustListView.SelectedIndex = idx;
        }

        private void CancelTicketBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.CancelSelectedTicket();
        }
    }
}
