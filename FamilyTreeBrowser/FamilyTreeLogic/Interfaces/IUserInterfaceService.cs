using FamilyTreeLogic.Models;

namespace FamilyTreeLogic.Interfaces;

/// <summary>
/// User Interface Logic
/// </summary>
public interface IUserInterfaceService
{
    /// <summary>
    /// Validate the User Request
    /// Build the user request based on the user arguments list
    /// </summary>
    /// <param name="args"></param>
    /// <returns>UserRequest</returns>
    UserRequest BuildReguest(string[] args);

    /// <summary>
    /// Write out processed data to the Console
    /// </summary>
    /// <param name="dataToDisplay"></param>
    void DisplayData(IEnumerable<string> dataToDisplay);
}
