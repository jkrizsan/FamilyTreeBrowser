using FamilyTreeLogic.Interfaces;

namespace FamilyTreeLogic.Wrappers;

public class FileWrapper : IFileWrapper
{
    /// <inheritdoc />
    public bool Exists(string fileName)
        => File.Exists(fileName);

    /// <inheritdoc />
    public string ReadAll(string FileName)
        => File.ReadAllText(FileName);
}
