using System.Collections.ObjectModel;
using System.Text.Json;

namespace AddressBook.CommonLibrary
{
    public class EmployeeList : ObservableCollection<Employee>
    {
        
        public static EmployeeList? LoadFromJson(FileInfo jsonFile)
        {
            try
            {
                using StreamReader reader = jsonFile.OpenText();
                string json = reader.ReadToEnd();
                EmployeeList? employees = JsonSerializer.Deserialize<EmployeeList>(json);
                return employees;
            }
            catch(FileNotFoundException)
            {
                Console.Error.WriteLine(new FileNotFoundException("File not found"));
                return null;
            }
            catch(Exception)
            {
                Console.Error.WriteLine(new Exception("Error while saving to JSON"));
                return null;
            }
        }
        public void SaveToJson(FileInfo jsonFile)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(this);
                using StreamWriter writer = jsonFile.CreateText();
                writer.Write(jsonString);
            }
            catch(Exception)
            {
                Console.Error.WriteLine(new Exception("Error while saving to JSON"));
            }
        }
        public IEnumerable<string> GetPositions()
        {
            var positions = this.Select(e => e.Position);
            var orderedUniquePositions = positions.Distinct().OrderBy(p => p);
            return orderedUniquePositions;
        }
        public IEnumerable<string> GetMainWorkplaces()
        {
            var mainWorkplaces = this.Select(e => e.MainWorkplace);
            var orderedUniqueMainWorkplaces = mainWorkplaces.Distinct().OrderBy(mw => mw);
            return orderedUniqueMainWorkplaces.Select(mw => mw!);
        }
        public SearchResult Search(string? mainWorkplace = null, string? position = null, string? name = null)
        {
            IEnumerable<Employee> employees = this;
            if (mainWorkplace != null)
            {
                employees = this.Where(e => e.MainWorkplace == mainWorkplace);
            }
            if (position != null)
            {
                employees = employees.Where(e => e.Position == position);
            }
            if (name != null)
            {
                employees = employees.Where(e => e.Name != null && e.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            return new SearchResult(employees.ToArray());
        }
    }
}
