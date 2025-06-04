using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WaveApp.Models
{
    public class Buoy : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private double _latitude;
        private double _longitude;
        private string _status;
        private double _maxCapacity;
        private double _currentEnergy;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public double MaxCapacity
        {
            get => _maxCapacity;
            set => SetProperty(ref _maxCapacity, value);
        }

        public double CurrentEnergy
        {
            get => _currentEnergy;
            set => SetProperty(ref _currentEnergy, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
