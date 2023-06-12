using FamilyTreeLogic.Enums;
using System.Text.Json.Serialization;

namespace FamilyTreeLogic.Models;

/// <summary>
/// Data model for storing people data
/// </summary>
public class Person
{
    /// <summary>
    /// First Name of the person
    /// </summary>
    public string? FirstName {get; init;}

    /// <summary>
    /// Last Name of the person
    /// </summary>
    public string? LastName { get; init;}

    /// <summary>
    /// Gender of the person
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; init;}

    /// <summary>
    /// Age of the person
    /// </summary>
    public int Age {get; init;}
}
