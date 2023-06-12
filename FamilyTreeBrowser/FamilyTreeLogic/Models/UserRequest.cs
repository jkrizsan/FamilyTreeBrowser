using FamilyTreeLogic.Enums;

namespace FamilyTreeLogic.Models;

/// <summary>
/// Contains parameters of the user request
/// </summary>
public class UserRequest
{
    /// <summary>
    /// The Input JSON file name
    /// </summary>
    public string? InputFile { get; init; }

    /// <summary>
    /// Search parameter will help to filtering the data
    /// </summary>
    public string? SearchParam { get; init; }

    /// <summary>
    /// Parameter that will influence the sortation
    /// </summary>
    public SortParam SortParam { get; init; }

    /// <summary>
    /// Parameter help to find duplicate family trees based on the first name
    /// </summary>
    public string? FirstName { get; init; }
}
