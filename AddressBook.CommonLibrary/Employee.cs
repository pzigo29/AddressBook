using System.ComponentModel;

namespace AddressBook.CommonLibrary
{
	public record Employee : INotifyPropertyChanged
	{
        public static bool IsPropertyChanged { get; set; }

        public Employee(string name, string position, string email, string? phone = null, string? room = null, string? mainWorkplace = null, string? workplace = null)
		{
			_name = name;
			_position = position;
            _phone = phone;
			_email = email;
			_room = room;
			_mainWorkplace = mainWorkplace;
			_workplace = workplace;
            IsPropertyChanged = false;
		}
		private string _name;
        private string _position;
        private string? _phone;
        private string _email;
        private string? _room;
        private string? _mainWorkplace;
        private string? _workplace;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }
        public string? Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public string? Room
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged();
            }
        }
        public string? MainWorkplace
        {
            get => _mainWorkplace;
            set
            {
                _mainWorkplace = value;
                OnPropertyChanged();
            }
        } //fakulta, ustav
        public string? Workplace
        {
            get => _workplace;
            set
            {
                _workplace = value;
                OnPropertyChanged();
            }
        } //skratka hlavného pracoviska + názov katedry alebo oddelenia

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            IsPropertyChanged = true;
        }

        
    }
}
