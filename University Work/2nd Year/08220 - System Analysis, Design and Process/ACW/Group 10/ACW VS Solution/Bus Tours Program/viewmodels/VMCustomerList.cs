using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Linq;
using DatabaseLibrary;

namespace Bus_Tours_Program.viewmodels
{
    public class TourTicket{
        public int TicketID { get; set; }
        public int TourID { get; set; }
        public string TourName {get; set;}
        public double SalePrice {get; set;}
        public DateTime DepartureDateTime {get; set;}
    }
    class VMCustomerList : INotifyPropertyChanged
    {
        #region SearchString
        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                UpdateCustomerList();
            }
        }
        #endregion
        #region SelectedCustomer
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                if (_selectedCustomer != null) SaveSelectedUser();

                _selectedCustomer = value;
                if (_selectedCustomer == null) return;
                var db = Database.Instance;
                UpdateSelectedCustomerTourTickets();
                OnPropertyChanged("SelectedCustomer");
            }
        }
        #endregion
        #region SelectedCustomerTickets
        private ObservableCollection<TourTicket> _selectedCustomerTickets = null; 
        public ObservableCollection<TourTicket> SelectedCustomerTickets {
            get { return _selectedCustomerTickets; }
            set
            {
                _selectedCustomerTickets = value;

                OnPropertyChanged("SelectedCustomerTickets");
            } 
        }
        #endregion
        #region BookableTours
        private ObservableCollection<Tour> _bookableTours = null;
        public ObservableCollection<Tour> BookableTours
        {
            get { return _bookableTours; }
            set
            {
                _bookableTours = value;
                OnPropertyChanged("BookableTours");
            }
        }
        #endregion
        #region SelectedTourToBook
        private Tour _selectedTourToBook = null;
        public Tour SelectedTourToBook
        {
            get { return _selectedTourToBook; }
            set
            {
                _selectedTourToBook = value;
                OnPropertyChanged("SelectedTourToBook");
                OnPropertyChanged("TicketBookingCost");
            }
        }
        #endregion
        #region QuantityToBook
        private int _quantityToBook = 1;
        public int QuantityToBook
        {
            get { return _quantityToBook; }
            set
            {
                _quantityToBook = value;
                OnPropertyChanged("QuantityToBook");
                OnPropertyChanged("TicketBookingCost");
            }
        }
        #endregion
        #region CustomerList
        private ObservableCollection<Customer> _customerList = null;
        public ObservableCollection<Customer> CustomerList
        {
            get { return _customerList; }
            set
            {
                _customerList = value;
                OnPropertyChanged("CustomerList");
            }
        }
        #endregion
        #region TicketBookingCost
        private double _ticketBookingCost;
        public double TicketBookingCost
        {
            get
            {
                if (SelectedTourToBook == null) return 0;
                double sum = 0.0;
                for (var i = 0; i < QuantityToBook; i++)
                {
                    if (i < 4 && SelectedCustomer.IsGoldClubMember)
                        sum += SelectedTourToBook.BaseTicketPrice * 0.8;
                    else
                        sum += SelectedTourToBook.BaseTicketPrice;
                }
                return sum;
            }
        }
        #endregion
        #region SelectedTicket
        private TourTicket _selectedTicket = null;
        public TourTicket SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                _selectedTicket = value;
                OnPropertyChanged("SelectedTicket");
            }
        }
        #endregion

        public VMCustomerList()
        {
            UpdateCustomerList();
        }

        private void UpdateSelectedCustomerTourTickets()
        {
               var db = Database.Instance;
                var tickets = db.GetTicketsByCustomerId(_selectedCustomer.ID);
                var joinedTickets = tickets.Join(db.GetTours(DateTime.MinValue, DateTime.MaxValue),
                       ticket => ticket.TourID, tour => tour.ID,
                       (ticket, tour) => new { Ticket = ticket, Tour = tour });
               SelectedCustomerTickets = new ObservableCollection<TourTicket>();
                foreach(var ticket in joinedTickets)
               {
                   SelectedCustomerTickets.Add(
                       new TourTicket {
                           TicketID = ticket.Ticket.ID,
                           TourID = ticket.Tour.ID,
                           TourName = ticket.Tour.Title, 
                           DepartureDateTime=ticket.Tour.DepartureDateTime, 
                           SalePrice=ticket.Ticket.SalePrice
                   });
               }
        }

        private void UpdateCustomerList()
        {
            var db = Database.Instance;
            CustomerList = new ObservableCollection<Customer>();
            var tempCustList = db.GetCustomers(_searchString);
            if (tempCustList == null) return; // Library returns null instead of empty list. GG.
            foreach (var customer in tempCustList)
            {
                CustomerList.Add(customer);
            }

            BookableTours = new ObservableCollection<Tour>();
            var tempTourList = db.GetTours(DateTime.Now, DateTime.MaxValue, true);
            foreach (var tour in tempTourList)
            {
                BookableTours.Add(tour);
            }
        }

        public void SaveSelectedUser()
        {
            if (SelectedCustomer == null) return;
            var db = Database.Instance;
            db.ModifyCustomer(SelectedCustomer.ID, SelectedCustomer.Title, SelectedCustomer.FirstName,
                SelectedCustomer.Surname, SelectedCustomer.Address, SelectedCustomer.Email,
                SelectedCustomer.TelephoneNumber);
            if (SelectedCustomer.IsGoldClubMember)
            {
                DateTime expiry = SelectedCustomer.GoldClubMemberExpiryDate;
                db.CancelGoldClub(SelectedCustomer.ID); // This has side effects for some bizarre reason?!
                db.JoinGoldClub(SelectedCustomer.ID, expiry);
            }
            
        }

        public void DeleteSelectedUser()
        {
            if (SelectedCustomer == null) return;
            var db = Database.Instance;
            var result = db.DeleteCustomer(SelectedCustomer.ID);
            UpdateCustomerList();
            SelectedCustomer = null;
            if (result == false) throw new Exception("Error: Could not delete user!");
        }

        public void ToggleGoldSelectedUser()
        {
            if (SelectedCustomer == null) return;
            var db = Database.Instance;

            if (SelectedCustomer.IsGoldClubMember)
            {
                db.CancelGoldClub(SelectedCustomer.ID);
                SelectedCustomer.IsGoldClubMember = false;
            }
            else
            {
                var expiry = DateTime.Now.AddYears(1);
                db.JoinGoldClub(SelectedCustomer.ID, expiry);
                SelectedCustomer.IsGoldClubMember = true;
                SelectedCustomer.GoldClubMemberExpiryDate = expiry;
            }
            OnPropertyChanged("CustomerList");
            OnPropertyChanged("SelectedCustomer");           
        }
        public void BookTickets()
        {
            if (SelectedTourToBook == null) return;
            var db = Database.Instance;

            for (var i = 0; i < QuantityToBook; i++)
            {
                double ticketCost = 0;
                if (i < 4 && SelectedCustomer.IsGoldClubMember)
                    ticketCost += SelectedTourToBook.BaseTicketPrice * 0.8;
                else
                    ticketCost += SelectedTourToBook.BaseTicketPrice;

                db.BuyTicket(SelectedTourToBook.ID, SelectedCustomer.ID, ticketCost);
            }

            UpdateSelectedCustomerTourTickets();
        }

        public void CancelSelectedTicket()
        {
            if (SelectedTicket == null) return;
            var db = Database.Instance;
            bool result = db.DeleteTicket(SelectedTicket.TicketID);
            db.CreateTicket(SelectedTicket.TourID);
            SelectedTicket = null;
            UpdateSelectedCustomerTourTickets();
            System.Diagnostics.Debug.WriteLine(result);
        }

        public void AddNewCustomer()
        {
            var db = Database.Instance;
            db.AddCustomer("Mr", "", "", "", "", "");
            UpdateCustomerList();
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
