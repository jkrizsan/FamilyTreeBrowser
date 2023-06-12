namespace FamilyTreeLogic.Interfaces;

/// <summary>
/// File Wrapper Interface
/// </summary>
public interface IFileWrapper
{
    /// <summary>
    /// Check that a file exists
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>boolean</returns>
    bool Exists(string fileName);

    /// <summary>
    /// Read all content from a file
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    string ReadAll(string fileName);
}
