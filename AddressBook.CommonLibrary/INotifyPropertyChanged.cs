using System.ComponentModel;

namespace AddressBook.CommonLibrary
{
    public interface INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
