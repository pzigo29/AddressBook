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
using System.Windows.Shapes;
using AddressBook.CommonLibrary;

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        public Employee? Employee { get; set; }
        public EmployeeWindow()
        {
            InitializeComponent();
            //EmployeeSaved += employee => Employee = employee;
        }

        public EmployeeWindow(Employee employee) : this()
        {
            Employee = employee;
            EmployeeBox.Text = employee.Name;
            PositionBox.Text = employee.Position;
            EmailBox.Text = employee.Email;
            PhoneBox.Text = employee.Phone ?? "";
            RoomBox.Text = employee.Room ?? "";
            MainWorkplaceBox.Text = employee.MainWorkplace ?? "";
            WorkplaceBox.Text = employee.Workplace ?? "";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Employee = new Employee(EmployeeBox.Text, PositionBox.Text, EmailBox.Text, PhoneBox.Text, RoomBox.Text, 
                MainWorkplaceBox.Text, WorkplaceBox.Text);
            EmployeeSaved?.Invoke(Employee);
            Close();
            Employee?.OnPropertyChanged();
        }

        public event Action<Employee>? EmployeeSaved;

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
