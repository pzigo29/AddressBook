using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AddressBook.CommonLibrary;

namespace AddressBook.EditorWpfApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private EmployeeList? Employees { get; set; } = [];
		public MainWindow()
		{
			InitializeComponent();
			Closing += MainWindow_Closing;
			foreach (var button in new List<Button> {EditEmployee, EraseEmployee})
			{
				button.IsEnabled = false;
			}
			EditMenu.IsEnabled = false;
			EraseMenu.IsEnabled = false;
			SearchButton.IsEnabled = false;
			ShowEmployeeCount(Employees?.Count.ToString());
		}

		private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
		{
			IfPropertyChanged();
		}

		private void Open_OnClick(object sender, RoutedEventArgs e)
		{
			IfPropertyChanged();
			OpenFileDialog openFileDialog = new()
			{
				Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
				Title = "Open a file"
			};
			if (openFileDialog.ShowDialog() == true)
			{
				string fileName = openFileDialog.FileName;
				Employees = EmployeeList.LoadFromJson(new FileInfo(fileName));

				ShowEmployeeCount(Employees?.Count.ToString());
				EmployeesView.ItemsSource = Employees;
				SearchButton.IsEnabled = EmployeesView.ItemsSource is ObservableCollection<Employee> { Count: > 0 };
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
					MessageBox.Show("Najprv otvorte adresár.", "Adresár nie je otvorený",
						MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}
		}

		private void Exit_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
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
			ShowEmployeeCount(employees.Count.ToString());
			SearchButton.IsEnabled = EmployeesView.ItemsSource is ObservableCollection<Employee> { Count: > 0 };

		}

		private void EditEmployee_OnClick(object sender, RoutedEventArgs e)
		{
			EditEmployeeMethod();
		}

		private void EmployeesView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EditEmployee.IsEnabled = EmployeesView.SelectedItem != null;
			EraseEmployee.IsEnabled = EmployeesView.SelectedItem != null;
			EditMenu.IsEnabled = EmployeesView.SelectedItem != null;
			EraseMenu.IsEnabled = EmployeesView.SelectedItem != null;
		}

		private void EraseEmployee_OnClick(object sender, RoutedEventArgs e)
		{
			var yesNo = MessageBox.Show("Naozaj chcete vymazať zamestnanca?", "Vymazať zamestnanca", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

			if (yesNo == MessageBoxResult.Yes)
			{
				var employees = (ObservableCollection<Employee>)EmployeesView.ItemsSource;
				employees.Remove((Employee)EmployeesView.SelectedItem);
				ShowEmployeeCount(employees.Count.ToString());
			}
		}

		private void New_OnClick(object sender, RoutedEventArgs e)
		{
			var employees = (ObservableCollection<Employee>)EmployeesView.ItemsSource;
			if (employees is { Count: > 0 })
			{
				IfPropertyChanged();
				employees.Clear();
				ShowEmployeeCount(employees.Count.ToString());
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
            Employees = [];
			foreach (Employee employee in EmployeesView.ItemsSource)
			{
				Employees.Add(employee);
			}
			if (saveFileDialog.ShowDialog() == true)
			{
				Employees?.SaveToJson(new FileInfo(saveFileDialog.FileName));
				Employee.IsPropertyChanged = false;
			}
		}

		private void About_OnClick(object sender, RoutedEventArgs e)
		{
			var about = new AboutWindow();
			about.ShowDialog();
		}

		private void IfPropertyChanged()
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
		}

		private void ShowEmployeeCount(string? employeesCount)
		{
			CountEmployees.Inlines.Clear();
			CountEmployees.Inlines.Add("Počet: ");
			CountEmployees.Inlines.Add(new Run(employeesCount)
			{
				FontWeight = FontWeights.Bold
			});
		}

		private void ListViewItem_OnDoubleClick(object sender, MouseButtonEventArgs e)
		{
			EditEmployeeMethod();
		}

		private void EditEmployeeMethod()
		{
			var addEmployeeWindow = new EmployeeWindow((Employee)EmployeesView.SelectedItem);
			addEmployeeWindow.EmployeeSaved += OnEmployeeSaved;
			addEmployeeWindow.ShowDialog();
		}
	}
}