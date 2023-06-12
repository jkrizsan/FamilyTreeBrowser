namespace FamilyTreeLogic.Models;

/// <summary>
/// Data model for storing tree structure
/// </summary>
public class PersonNode : Person
{
    /// <summary>
    /// Children of the person
    /// </summary>
    public PersonNode[]? Children { get; init;}
}
