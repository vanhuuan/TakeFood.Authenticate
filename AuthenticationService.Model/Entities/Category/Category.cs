using MongoDB.Bson.Serialization.Attributes;

namespace AuthenticationService.Model.Entities.Category;

/// <summary>
/// Category shared
/// </summary>
public class Category
{
    /// <summary>
    /// Category's name
    /// </summary>
    [BsonElement("Name")]
    public string Name;
}
