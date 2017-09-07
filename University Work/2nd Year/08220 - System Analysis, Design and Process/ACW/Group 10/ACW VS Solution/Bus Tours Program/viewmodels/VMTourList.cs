using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DatabaseLibrary;

namespace Bus_Tours_Program.viewmodels
{
    internal class VMTourList : INotifyPropertyChanged
    {
        public VMTourList()
        {
            UpdateTourList();
        }

        #region  TotalSalePrice
        private double _totalSalePrice;
        public double TotalSalePrice
        {
            get { return _totalSalePrice; }
            set
            {
                _totalSalePrice = value;
                OnPropertyChanged("TotalSalePrice");
            }
        }
        #endregion

        #region SelectedTour

        private Tour _selectedTour;

        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != null) SaveSelectedTour();

                _selectedTour = value;
                if (_selectedTour == null || SelectedTour.ID == -1) return;
                WynneDatabase db = Database.Instance;
                SelectedTourTickets = new ObservableCollection<Ticket>();
                foreach (Ticket ticket in db.GetTicketsForTour(_selectedTour.ID))
                {
                    SelectedTourTickets.Add(ticket);
                }
                TotalSalePrice = SelectedTourTickets.Sum(ticket => ticket.SalePrice);
                OnPropertyChanged("SelectedTour");
            }
        }

        #endregion

        #region SelectedTourTickets

        private ObservableCollection<Ticket> _selectedTourTickets;

        public ObservableCollection<Ticket> SelectedTourTickets
        {
            get { return _selectedTourTickets; }
            set
            {
                _selectedTourTickets = value;
                OnPropertyChanged("SelectedTourTickets");
            }
        }

        #endregion

        #region SearchStart

        private DateTime _searchStart = DateTime.Now;

        public DateTime SearchStart
        {
            get { return _searchStart; }
            set
            {
                _searchStart = value;
                OnPropertyChanged("SearchStart");
                UpdateTourList();
            }
        }

        #endregion

        #region SearchEnd

        private DateTime _searchEnd = DateTime.Now.AddMonths(6);

        public DateTime SearchEnd
        {
            get { return _searchEnd; }
            set
            {
                _searchEnd = value;
                OnPropertyChanged("SearchEnd");
                UpdateTourList();
            }
        }

        #endregion

        #region AvailableOnly

        private bool _availableOnly;

        public bool AvailableOnly
        {
            get { return _availableOnly; }
            set
            {
                _availableOnly = value;
                OnPropertyChanged("AvailableOnly");
                UpdateTourList();
            }
        }

        #endregion

        #region TourList

        private ObservableCollection<Tour> _tourList;

        public ObservableCollection<Tour> TourList
        {
            get { return _tourList; }
            set
            {
                _tourList = value;
                OnPropertyChanged("TourList");
            }
        }

        #endregion

        #region CoachList
        private ObservableCollection<Coach> _coachList;

        public ObservableCollection<Coach> CoachList
        {
            get { return _coachList; }
            set
            {
                _coachList = value;
                OnPropertyChanged("CoachList");
            }
        }
        #endregion

        private void UpdateTourList()
        {
            WynneDatabase db = Database.Instance;
            TourList = new ObservableCollection<Tour>();
            List<Tour> tempTourList = db.GetTours(_searchStart, _searchEnd, _availableOnly);
            if (tempTourList == null) return; // Library returns null instead of empty list. GG.
            foreach (Tour tour in tempTourList)
            {
                TourList.Add(tour);
            }

            CoachList = new ObservableCollection<Coach>();
            List<Coach> tempCoachList = db.GetCoaches();
            foreach (Coach coach in tempCoachList)
            {
                CoachList.Add(coach);
            }
        }

        public void AddNewTour()
        {
            if (SelectedTour != null && SelectedTour.ID == -1) return;
            var newTour = new Tour {DepartureDateTime = DateTime.Now.AddDays(1)};
            TourList.Add(newTour);
            SelectedTour = TourList[TourList.Count - 1];
            SelectedTour.ID = -1;
        }

        public void SaveSelectedTour()
        {
            if (SelectedTour == null) return;
            WynneDatabase db = Database.Instance;
            if (SelectedTour.ID == -1)
            {
                int tourID = db.CreateTour(SelectedTour.Title, SelectedTour.Description, SelectedTour.DepartureDateTime,
                    SelectedTour.CoachID, SelectedTour.BaseTicketPrice);
                SelectedTour.ID = tourID;
                int seatCount = CoachList.First(coach => coach.ID == SelectedTour.CoachID).NumberOfSeats;
                // HACK: Create tour doesn't actually make tickets
                if (db.GetAvailableTicketCount(SelectedTour.ID) != seatCount)
                    for (int i = 0; i < seatCount; i++) db.CreateTicket(SelectedTour.ID);
            }
            else
                db.ModifyTour(SelectedTour.ID, SelectedTour.Title, SelectedTour.Description,
                    SelectedTour.DepartureDateTime, SelectedTour.CoachID, SelectedTour.BaseTicketPrice);

            db.UpdateTicketPriceForTour(SelectedTour.ID, SelectedTour.BaseTicketPrice);
        }

        public void DeleteSelectedTour()
        {
            if (SelectedTour == null) return;
            if (SelectedTour.ID == -1)
            {
                TourList.RemoveAt(TourList.Count - 1);
            }
            else
            {
                WynneDatabase db = Database.Instance;
                bool result = db.DeleteTour(SelectedTour.ID);
                if (result == false) throw new Exception("Error: Could not delete tour!");
            }

            SelectedTour = null;
            UpdateTourList();
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
