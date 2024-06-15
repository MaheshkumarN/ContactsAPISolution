using System.Text.Json.Serialization;

namespace ContactsWebAPI.Models.Entities;

public class Contact
{
  [JsonPropertyName("id")]
  public int Id { get; set; }
  [JsonPropertyName("contactName")]
  public string? ContactName { get; set; }
  [JsonPropertyName("contactType")] 
  public string? ContactType { get; set; }
  [JsonPropertyName("phone")]
  public string? Phone { get; set; }
  [JsonPropertyName("email")]
  public string? Email { get; set; }
}
