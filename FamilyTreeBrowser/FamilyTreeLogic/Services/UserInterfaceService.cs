using System.Text;
using FamilyTreeLogic.Enums;
using FamilyTreeLogic.Exceptions;
using FamilyTreeLogic.Interfaces;
using FamilyTreeLogic.Models;

namespace FamilyTreeLogic.Services;

public class UserInterfaceService : IUserInterfaceService
{
    private readonly AppSettings _appSettings;

    private string _help;

    private const string _sortArg = "-sort";
    private const string _inputArg = "-input";
    private const string _helpArg = "-h";
    private const string _searchArg = "-search";
    private const string _duplicateFamilyTreeArg = "-findduplicateinfamilytree";
    private const string _filePostfix = ".json";
    private int _argSize;

    public UserInterfaceService(AppSettings appSettings)
    {
        _appSettings = appSettings;

        buildHelp();
    }

    /// <inheritdoc />
    public UserRequest BuildReguest(string[] args)
    {
        _argSize = args.Length;

        validateHelper(args);
        validateArgumentsLength(args);

        string inputFile = getInputFile(args);
        string searchParam = getSearchParam(args);
        string firstName = getFirstName(args);
        SortParam sortParam = getSortParam(args);
        
        return new UserRequest
        {
            InputFile = inputFile,
            SortParam = sortParam,
            SearchParam = searchParam,
            FirstName = firstName
        };
    }

    /// <inheritdoc />
    public void DisplayData(IEnumerable<string> dataToDisplay)
    {
        var data = dataToDisplay
            .Distinct()
            .ToList();

        int start = 0;
        int end = _appSettings.PageSize < data.Count
            ? _appSettings.PageSize
            : data.Count;

        while (end <= data.Count)
        {
            displayPage(data, start, end);

            if(end == data.Count)
            {  break; }

            if (getTheNextPage())
            {
                start += _appSettings.PageSize;
                int tmp = end + _appSettings.PageSize;
                end = tmp < data.Count ? tmp : data.Count;
            }
        }
    }

    private bool getTheNextPage()
    {
        while (true)
        {
            if (Console.ReadKey().Key.Equals(ConsoleKey.Enter))
            {
                return true;
            }
            else
            {
                writeToConsole("\nPress Enter to get the next page!");
            }
        }
    }

    private void displayPage(List<string> data, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            writeToConsole(data[i]);
        }
    }

    private SortParam getSortParam(string[] args)
    {
        if (args.Contains(_sortArg) == false)
        {
            return SortParam.Unknown;
        }

        int index = Array.IndexOf(args, _sortArg);

        if (index == _argSize - 1)
        {
            throw new UserException($"After the '{_sortArg}' argument, the sort param must be set!");
        }

        string param = args[index + 1].ToLowerInvariant();

        if (param.StartsWith('-'))
        {
            throw new UserException($"The sort parameter cannot start with '-' char!");
        }

        if (param != "age" && param != "lastname")
        {
            throw new UserException($"The sort parameter must be 'LastName' or 'Age'!");
        }

        return param == "age" ? SortParam.Age : SortParam.LastName;
    }

    private string getSearchParam(string[] args)
    {
        if (args.Contains(_searchArg) == false)
        {
            return string.Empty;
        }

        int index = Array.IndexOf(args, _searchArg);

        if (index == _argSize - 1)
        {
            throw new UserException($"After the '{_searchArg}' argument, the search param must be set!");
        }

        string param = args[index + 1];

        if (param.StartsWith('-'))
        {
            throw new UserException($"The search parameter cannot start with '-' char!");
        }

        if (param.Length < 2)
        {
            throw new UserException($"The search parameter must be at least 2 chars length!");
        }

        return param.ToLowerInvariant();
    }

    private string getFirstName(string[] args)
    {
        if (args.Contains(_duplicateFamilyTreeArg) == false)
        {
            return string.Empty;
        }

        int index = Array.IndexOf(args, _duplicateFamilyTreeArg);

        if (index == _argSize - 1)
        {
            throw new UserException($"After the '{_duplicateFamilyTreeArg}' argument, the FirstName param must be set!");
        }

        string param = args[index + 1];

        if (param.StartsWith('-'))
        {
            throw new UserException($"The FirstName parameter cannot start with '-' char!");
        }

        if (param.Length < 2)
        {
            throw new UserException($"The FirstName parameter must be at least 2 chars length!");
        }

        return param.ToLowerInvariant();
    }

    private string getInputFile(string[] args)
    {
        if (args[0].Equals(_inputArg) == false
            || args[1].EndsWith(_filePostfix) == false)
        {
            throw new UserException($"First argument need to be: '{_inputArg}', that have to follow a JSON file name!");
        }

        return args[1];
    }

    private void validateHelper(string[] args)
    {
        if (args[0].Equals(_helpArg))
        {
            writeToConsole(_help);

            throw new UserException(string.Empty);
        }
    }

    private void validateArgumentsLength(string[] args)
    {
        if (args.Length.Equals(0) || args.Length < 2)
        {
            throw new UserException($"Needed to provide some arguments for the proper working. Please use the '{_helpArg}' flag if more information needed!");
        }
    }

    private void writeToConsole(string data)
        => Console.WriteLine(data);

    private void buildHelp()
    {
        StringBuilder help = new StringBuilder("Help:");

        help.AppendLine();
        help.AppendLine();
        help.AppendLine("-input inputfilename                   Define the input file");
        help.AppendLine("-sort sortvalue                        Define the value to sort by");
        help.AppendLine("-search searchword                     Define the value to search by");
        help.AppendLine("-findduplicateinfamilytree findvalue   Define the value to find duplicates in family tree");
        help.AppendLine();

        _help = help.ToString();
    }
}
