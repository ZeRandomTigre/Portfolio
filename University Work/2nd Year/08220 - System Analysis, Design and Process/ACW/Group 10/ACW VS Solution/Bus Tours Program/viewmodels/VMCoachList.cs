using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary;

namespace Bus_Tours_Program.viewmodels
{
    class VMCoachList : INotifyPropertyChanged
    {
        #region CoachList
        private ObservableCollection<Coach> _coachList = null;
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
        #region SelectedCoach
        private Coach _selectedCoach;
        public Coach SelectedCoach
        {
            get
            {
                return _selectedCoach;
            }
            set
            {
                _selectedCoach = value;
                OnPropertyChanged("SelectedCoach");
            }
        }
        #endregion
        public VMCoachList()
        {
            UpdateCoachList();
        }

        private void UpdateCoachList()
        {
            var db = Database.Instance;
            CoachList = new ObservableCollection<Coach>();
            var tempCoachList = db.GetCoaches();
            if (tempCoachList == null) return; // Library returns null instead of empty list. GG.
            foreach (var coach in tempCoachList)
            {
                CoachList.Add(coach);
            }
        }

        public void AddNewCoach(int numberOfSeats, string registrationNumber)
        {
            var db = Database.Instance;
            db.AddCoach(numberOfSeats, registrationNumber);
            UpdateCoachList();
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

        public void DeleteSelectedCoach()
        {
            if (SelectedCoach == null) return;
            var db = Database.Instance;
            var result = db.DeleteCoach(SelectedCoach.ID);
            UpdateCoachList();
            SelectedCoach = null;
            if (result == false) throw new Exception("Error: Could not delete coach!");
        }
    }
}
