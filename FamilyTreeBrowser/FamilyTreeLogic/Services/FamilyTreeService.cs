using System.Text;
using System.Text.Json;
using FamilyTreeLogic.Enums;
using FamilyTreeLogic.Exceptions;
using FamilyTreeLogic.Interfaces;
using FamilyTreeLogic.Models;

namespace FamilyTreeLogic.Services;

public class FamilyTreeService : IFamilyTreeService
{
    private readonly FamilyTreeData _familyTreeData = new FamilyTreeData();

    private readonly IFileWrapper _fileWrapper;

    int _numberOfFirstNames = 0; // used just in the findDuplicateFamilyTrees feature

    public FamilyTreeService(IFileWrapper fileWrapper)
    {
        _fileWrapper = fileWrapper;
    }

    /// <inheritdoc />
    public FamilyTreeData Initialize(UserRequest request)
    {
        _ = request ?? throw new ArgumentNullException($"{nameof(request)}");

        _familyTreeData.UserRequest = request;

        try
        {
            loadDataFromFile();

            if (string.IsNullOrEmpty(request.FirstName) == true)
            {
                setPeople();
            }
        }
        catch
        {
            throw;
        }

        return _familyTreeData;
    }
    
    /// <inheritdoc />
    public IEnumerable<string> RunProcess()
    {
        try
        {
            findDuplicateFamilyTrees();
            searchOnData();
            sortData();

            return buildDataToDisplay();
        }
        catch
        {
            throw;
        }
    }

    private IEnumerable<string> buildDataToDisplay()
    {
        List<string> result = new List<string>();

        foreach (var p in _familyTreeData.People)
        {
            result.Add(buildPersonDisplay(p));
        }

        return result;
    }

    private string buildPersonDisplay(Person person)
    {
        string whiteSpace = " ";

        StringBuilder strBuilder = new StringBuilder();

        if (person.Gender.Equals(Gender.Female))
        {
            strBuilder.Append("Ms.");
        }

        if (person.Gender.Equals(Gender.Male))
        {
            strBuilder.Append("Mr.");
        }

        strBuilder.Append(whiteSpace);
        strBuilder.Append(person.FirstName);
        strBuilder.Append(whiteSpace);
        strBuilder.Append(person.LastName);
        strBuilder.Append(whiteSpace);
        strBuilder.Append(person.Age);

        return strBuilder.ToString();
    }

    private void loadDataFromFile()
    {
        var request = _familyTreeData.UserRequest;

        try
        {
            string json = _fileWrapper.ReadAll(request?.InputFile ?? string.Empty);
            _familyTreeData.PersonNodes = JsonSerializer.Deserialize<IEnumerable<PersonNode>>(json);
        }
        catch(Exception ex)
        {
            throw new ValidationException($"The content of the '{request.InputFile}' file is not valid");
        }
    }

    private void setPeople()
    {
        foreach (var p in _familyTreeData.PersonNodes)
        {
            addDataToPeople(new List<PersonNode>() { p });
        }
    }

    private void addDataToPeople(IEnumerable<PersonNode> personNodes)
    {
        if (personNodes is null)
        {
            return;
        }

        foreach (var personNode in personNodes)
        {
            _familyTreeData.People.Add(buildPerson(personNode));
            addDataToPeople(personNode?.Children);
        }
    }

    private void sortData()
    {
        var request = _familyTreeData.UserRequest;

        if(request.SortParam.Equals(SortParam.Unknown))
        {
            return;
        }

        IEnumerable<Person> sortedPeople = new List<Person>();

        if (request.SortParam.Equals(SortParam.Age))
        {
            sortedPeople = _familyTreeData.People
                .OrderBy(p => p.Age);
        }

        if (request.SortParam.Equals(SortParam.LastName))
        {
            sortedPeople = _familyTreeData.People
                .OrderBy(p => p.LastName);
        }

        _familyTreeData.People = sortedPeople.ToList();
    }

    private void searchOnData()
    {
        var request = _familyTreeData.UserRequest;

        if(string.IsNullOrWhiteSpace(request.SearchParam))
        {
            return;
        }

        _familyTreeData.People = _familyTreeData.People
            .Where(x =>
                x.FirstName.ToLowerInvariant().Contains(request.SearchParam)
                || x.LastName.ToLowerInvariant().Contains(request.SearchParam))
            .ToList();
    }

    private void findDuplicateFamilyTrees()
    {
        var request = _familyTreeData.UserRequest;

        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return;
        }

        foreach (var personNode in _familyTreeData.PersonNodes)
        {
            _numberOfFirstNames = 0;
            _numberOfFirstNames += compareFirstName(personNode, request.FirstName);
            searchProvidedFirstName(personNode.Children);

            if (_numberOfFirstNames > 1)
            {
                _familyTreeData.People.Add(buildPerson(personNode));
            }
        }
    }

    private void searchProvidedFirstName(IEnumerable<PersonNode> personNodes)
    {
        if (personNodes is null)
        {
            return;
        }

        if(_numberOfFirstNames > 1)
        {
            return;
        }

        foreach (var personNode in personNodes)
        {
            _numberOfFirstNames += compareFirstName(personNode, _familyTreeData.UserRequest.FirstName);
            searchProvidedFirstName(personNode?.Children);
        }
    }

    private int compareFirstName(Person person, string firstName)
       => person.FirstName.ToLowerInvariant().Equals(firstName)
           ? 1
           : 0;

    private Person buildPerson(PersonNode personNode)
        => new Person()
        {
            FirstName = personNode.FirstName,
            LastName = personNode.LastName,
            Gender = personNode.Gender,
            Age = personNode.Age
        };
}
