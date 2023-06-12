namespace FamilyTreeLogic.Models;

/// <summary>
/// Store data set here that use in the inner processing
/// </summary>
public class FamilyTreeData
{
    /// <summary>
    /// User Request
    /// </summary>
    public UserRequest UserRequest { get; set; }

    /// <summary>
    /// All People
    /// </summary>
    public List<Person> People { get; set; }

    /// <summary>
    /// All Person Nodes
    /// </summary>
    public IEnumerable<PersonNode>  PersonNodes { get; set; }

    public FamilyTreeData()
    {
        PersonNodes = new List<PersonNode>();
        People = new List<Person>();
    }
}
