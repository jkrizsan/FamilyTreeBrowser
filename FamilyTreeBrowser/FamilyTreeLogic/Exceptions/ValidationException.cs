namespace FamilyTreeLogic.Exceptions;

/// <summary>
/// Indicates exception for the validation errors
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ValidationException(string message)
    {
        ErrorMessage = message;
    }

    public string ErrorMessage { get; }
}
