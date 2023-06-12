using FamilyTreeLogic.Models;

namespace FamilyTreeLogic.Interfaces;

/// <summary>
/// Interface for the FamilyTree business logic
/// </summary>
public interface IFamilyTreeService
{
    /// <summary>
    /// The process receive the user request and load data from the file
    /// Build up the inner data set.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>FamilyTreeData</returns>
    FamilyTreeData Initialize(UserRequest request);

    /// <summary>
    /// Apply user requests on the inner data set
    /// Built up the data that will be displayed to the user
    /// </summary>
    /// <returns>List of strings</returns>
    IEnumerable<string> RunProcess();
}
