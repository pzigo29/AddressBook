namespace AddressBook.CommonLibrary
{
    public class SearchResult(Employee[] employees)
    {
        public Employee[] Employees { get; } = employees;

        public void SaveToCsv(FileInfo csvFile, string delimiter = "\t")
        {
            using StreamWriter writer = csvFile.CreateText();
            string header = "Name" + delimiter + "Main Workplace" + delimiter + "Workplace" + delimiter + "Room" + delimiter + "Phone" + delimiter + "Email" + delimiter + "Position";
            writer.WriteLine(header);
            foreach (var e in Employees)
            {
                string line = e.Name + delimiter + e.MainWorkplace + delimiter + e.Workplace + 
                    delimiter + e.Room + delimiter + e.Phone + delimiter + e.Email + delimiter + e.Position;
                writer.WriteLine(line);
            }
        }
    }
}
