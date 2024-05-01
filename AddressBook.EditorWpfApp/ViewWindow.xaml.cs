using System.IO;
using System.Windows;
using System.Windows.Documents;
using AddressBook.CommonLibrary;
using Microsoft.Win32;

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : Window
    {
        public EmployeeList? Employees { get; set; }
        public SearchResult? SearchResult { get; private set; }
        public ViewWindow(EmployeeList employees)
        {
            InitializeComponent();
            Employees = employees;

            Position.ItemsSource = Employees?.GetPositions();
            Workplace.ItemsSource = Employees?.GetMainWorkplaces();

            ShowEmployeeCount();
        }

        private void FindEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (OutputText.Document.Blocks.Count > 0)
            {
                OutputText.Document.Blocks.Clear();
            }
            SearchResult = Employees?.Search(Workplace.SelectedItem?.ToString(), Position.SelectedItem?.ToString(), FullName.Text);
            ShowEmployeeCount();
            if (SearchResult != null)
            {
                foreach (var emp in SearchResult.Employees)
                {
                    Paragraph paragraph = new();
                    paragraph.Inlines.Add(new Run(emp.Name + "\n")
                    {
                        FontSize = 15
                    });
                    paragraph.Inlines.Add(new Run("Pracovisko: ") { FontWeight = FontWeights.Bold });
                    paragraph.Inlines.Add(emp.Workplace + "\n");
                    paragraph.Inlines.Add(new Run("Miestnosť: ") { FontWeight = FontWeights.Bold });
                    paragraph.Inlines.Add(emp.Room + "\n");
                    paragraph.Inlines.Add(new Run("Telefón: ") { FontWeight = FontWeights.Bold });
                    paragraph.Inlines.Add(emp.Phone + "\n");
                    paragraph.Inlines.Add(new Run("E-mail: ") { FontWeight = FontWeights.Bold });
                    paragraph.Inlines.Add(emp.Email + "\n");
                    paragraph.Inlines.Add(new Run("Funkcia: ") { FontWeight = FontWeights.Bold });
                    paragraph.Inlines.Add(emp.Position + "\n");
                    OutputText.Document.Blocks.Add(paragraph);
                }
            }
        }

        private void ExportToCSV_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Save as"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;
                SearchResult?.SaveToCsv(new FileInfo(fileName), ";");
            }
        }

        private void ResetSearch_Click(object sender, RoutedEventArgs e)
        {
            FullName.Text = "";
            Position.SelectedIndex = -1;
            Workplace.SelectedIndex = -1;
            OutputText.Document.Blocks.Clear();
            ShowEmployeeCount();
        }

        private void ShowEmployeeCount()
        {
            EmployeeCount.Inlines.Clear();
            var count = SearchResult?.Employees.Length ?? 0;
            var countText = $"{count}";
            var boldRun = new Run(countText)
            {
                FontWeight = FontWeights.Bold
            };
            EmployeeCount.Inlines.Add("Počet nájdených zamestnancov: ");
            EmployeeCount.Inlines.Add(boldRun);
        }
    }
}
