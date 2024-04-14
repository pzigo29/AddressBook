
namespace AddressBook.CommonLibrary
{
	public record class Employee : INotifyPropertyChanged
	{
		public Employee(string name, string position, string email, string? phone = null, string? room = null, string? mainWorkplace = null, string? workplace = null)
		{
			Name = name;
			Position = position;
			Phone = phone;
			Email = email;
			Room = room;
			MainWorkplace = mainWorkplace;
			Workplace = workplace;
		}
		public string Name { get; set; }
		public string Position { get; set; }
		public string? Phone { get; set; }
		public string Email { get; set; }
		public string? Room { get; set; }
		public string? MainWorkplace { get; set; } //fakulta, ustav
		public string? Workplace { get; set; } //skratka hlavného pracoviska + názov katedry alebo oddelenia

		event System.ComponentModel.PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				throw new NotImplementedException();
			}

			remove
			{
				throw new NotImplementedException();
			}
		}
	}
}
