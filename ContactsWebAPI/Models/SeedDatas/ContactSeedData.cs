using ContactsWebAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ContactsWebAPI.Models.SeedDatas;

public static class ContactSeedData
{
  public static List<Contact> CreateContacts = [
    new() { ContactName = "Mowgli", ContactType = "Family", Phone = "1234567890", Email = "mowgli@m.com" },
    new() { ContactName = "Baloo", ContactType = "Friend", Phone = "2345678901", Email = "baloo@b.com" },
    new() { ContactName = "Sherekhan", ContactType = "Professional", Phone = "3456789012", Email = "sherekhan@s.com" },
    new() { ContactName = "Bhageera", ContactType = "Family", Phone = "4567890123", Email = "bahgeera@b.com" },
    new() { ContactName = "Kaa", ContactType = "Friend", Phone = "5678901234", Email = "kaa@k.com" },
    ];
}
