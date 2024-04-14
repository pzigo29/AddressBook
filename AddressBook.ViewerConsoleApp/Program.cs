using AddressBook.CommonLibrary;
using System.CommandLine;


var input = new Option<string>("--input", "Input file") { IsRequired = true };
var name = new Option<string>("--name", "Name of the employee");
var position = new Option<string>("--position", "Position of the employee");
var mainWorkplace = new Option<string>("--main-workplace", "Main workplace of the employee");
var output = new Option<string>("--output", "Output file");

var rootCommand = new RootCommand() { input, name, position, mainWorkplace, output };
rootCommand.SetHandler(DisplayToOutput, input, name, position, mainWorkplace, output);


return rootCommand.Invoke(args);

static void DisplayToOutput(string input, string? name, string? position, string? mainWorkplace, string? output)
{
	var employees = EmployeeList.LoadFromJson(new FileInfo(input));

	if (employees == null)
	{
		return;
	}
	var searchResult = employees.Search(mainWorkplace, position, name);
	for (int i = 0; i < searchResult.Employees.Length; i++)
	{
		Console.WriteLine($"[{i+1}] {searchResult.Employees[i].Name}");
        Console.WriteLine($"Pracovisko: {searchResult.Employees[i].Workplace}");
        Console.WriteLine($"Miestnosť: {searchResult.Employees[i].Room}");
        Console.WriteLine($"Telefón: {searchResult.Employees[i].Phone}");
        Console.WriteLine($"E-mail: {searchResult.Employees[i].Email}");
        Console.WriteLine($"Funkcia: {searchResult.Employees[i].Position}");
		Console.WriteLine();
    }
	if (output != null)
	{
		searchResult.SaveToCsv(new FileInfo(output), ";");
	}
}
