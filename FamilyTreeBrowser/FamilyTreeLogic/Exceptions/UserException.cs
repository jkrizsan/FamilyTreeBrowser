namespace FamilyTreeLogic.Exceptions;

/// <summary>
/// Indicates exception for user errors
/// </summary>
public class UserException : Exception
{
    /// <summary>
    /// Constructor
    /// </summary>
    public UserException(string message)
    {
        ErrorMessage = message;
    }

    public string ErrorMessage { get; }
}
