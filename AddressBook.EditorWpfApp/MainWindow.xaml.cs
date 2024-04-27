using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AddressBook.CommonLibrary;

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EmployeeList? Employees { get; set; } = [];
        public MainWindow()
        {
            InitializeComponent();
            foreach (var button in new List<Button> {EditEmployee, EraseEmployee})
            {
                button.IsEnabled = false;
            }
            EditMenu.IsEnabled = false;
            EraseMenu.IsEnabled = false;
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Open a file"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                Employees = EmployeeList.LoadFromJson(new FileInfo(fileName));

                CountEmployees.Inlines.Clear();
                CountEmployees.Inlines.Add("Počet: ");
                CountEmployees.Inlines.Add(new Run(Employees?.Count.ToString())
                {
                    FontWeight = FontWeights.Bold
                });
                EmployeesView.ItemsSource = Employees;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employees != null)
            {
                var emps = (ObservableCollection<Employee>)EmployeesView.ItemsSource;
                if (emps is { Count: > 0 })
                {
                    EmployeeList list = [.. emps];
                    var viewWindow = new ViewWindow(list);
                    viewWindow.ShowDialog();
                }
                else
                {
                    var messageBoxResult = MessageBox.Show("Najprv otvorte adresár.", "Adresár nie je otvorený",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new EmployeeWindow();
            addEmployeeWindow.EmployeeSaved += OnEmployeeSaved;
            addEmployeeWindow.ShowDialog();
        }

        private void AddEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new EmployeeWindow();
            addEmployeeWindow.EmployeeSaved += OnEmployeeSaved;
            addEmployeeWindow.ShowDialog();
        }

        private void OnEmployeeSaved(Employee employee)
        {
            if (EmployeesView.ItemsSource is not ObservableCollection<Employee> employees)
            {
                employees = [employee];
            }
            else if (EmployeesView.SelectedItem != null)
            {
                var index = EmployeesView.SelectedIndex;
                try
                {
                    employees.RemoveAt(index);
                    employees.Insert(index, employee);
                    //EmployeesView.SelectedItem = null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            else
            {
                employees.Add(employee);
            }
            EmployeesView.ItemsSource = employees;
            CountEmployees.Inlines.Clear();
            CountEmployees.Inlines.Add("Počet: ");
            CountEmployees.Inlines.Add(new Run(employees?.Count.ToString())
            {
                FontWeight = FontWeights.Bold
            });

        }

        private void EditEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new EmployeeWindow((Employee)EmployeesView.SelectedItem);
            addEmployeeWindow.EmployeeSaved += OnEmployeeSaved;
            addEmployeeWindow.ShowDialog();
        }

        private void EmployeesView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditEmployee.IsEnabled = EmployeesView.SelectedItem != null;
            EraseEmployee.IsEnabled = EmployeesView.SelectedItem != null;
            EditMenu.IsEnabled = EmployeesView.SelectedItem != null;
            EraseMenu.IsEnabled = EmployeesView.SelectedItem != null;
        }

        private void EraseEmployee_Click(object sender, RoutedEventArgs e)
        {
            Erase();
        }

        private void EraseMenu_OnClick(object sender, RoutedEventArgs e)
        {
            Erase();
        }

        private void Erase()
        {
            var yesNo = MessageBox.Show("Naozaj chcete vymazať zamestnanca?", "Vymazať zamestnanca", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (yesNo == MessageBoxResult.Yes)
            {
                var employees = (ObservableCollection<Employee>)EmployeesView.ItemsSource;
                employees.Remove((Employee)EmployeesView.SelectedItem);
                CountEmployees.Inlines.Clear();
                CountEmployees.Inlines.Add("Počet: ");
                CountEmployees.Inlines.Add(new Run(employees?.Count.ToString() ?? "0")
                {
                    FontWeight = FontWeights.Bold
                });
            }
        }

        private void New_OnClick(object sender, RoutedEventArgs e)
        {
            var employees = (ObservableCollection<Employee>)EmployeesView.ItemsSource;
            if (employees is { Count: > 0 })
            {
                if (Employee.IsPropertyChanged)
                {
                    var yesNo = MessageBox.Show("Adresár bol zmenený. Chcete ho uložiť?", "Uložiť adresár",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                    if (yesNo == MessageBoxResult.Yes)
                    {
                        SaveToJson();
                    }
                }
                employees.Clear();
                CountEmployees.Inlines.Clear();
                CountEmployees.Inlines.Add("Počet: ");
                CountEmployees.Inlines.Add(new Run(employees?.Count.ToString() ?? "0")
                {
                    FontWeight = FontWeights.Bold
                });
            }
        }

        private void SaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            SaveToJson();
        }

        private void SaveToJson()
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Save as"
            };
            Employees = new();
            foreach (Employee employee in EmployeesView.ItemsSource)
            {
                Employees.Add(employee);
            }
            if (saveFileDialog.ShowDialog() == true)
            {
                Employees?.SaveToJson(new FileInfo(saveFileDialog.FileName));
            }
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }
        //private void New_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void Open_OnClick(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new()
        //    {
        //        Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
        //        Title = "Open a file"
        //    };
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        string fileName = openFileDialog.FileName;
        //        Employees = EmployeeList.LoadFromJson(new FileInfo(fileName));

        //        CountEmployees.Inlines.Clear();
        //        CountEmployees.Inlines.Add("Počet: ");
        //        CountEmployees.Inlines.Add(new Run(Employees?.Count.ToString())
        //        {
        //            FontWeight = FontWeights.Bold
        //        });
        //        EmployeesView.ItemsSource = Employees;
        //    }
        //}

        //private void SaveAs_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void Exit_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void AddMenu_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void EraseMenu_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void About_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void AddEmployee_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void EditEmployee_OnClick(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void EraseEmployee_Click(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void EmployeesView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}