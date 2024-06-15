using ContactsWebAPI.Models.Entities;


namespace ContactsWebAPI.Models.Abstract;

public interface IContactRepository
{
  Task<IEnumerable<Contact>> GetContactsAsync();
  Task<IEnumerable<Contact>> GetContactsByNameAsync(string contactName);
  Task<IEnumerable<Contact>> GetContactsByTypeAsync(string contactType);
  Task<IEnumerable<Contact>> GetContactsByPhoneAsync(string phoneNumber);
  Task<Contact> GetContactAsync(int id);
  Task<bool> CreateContactAsync(Contact contact);
  Task<bool> UpdateContactAsync(Contact contact);
  Task<bool> DeleteContactAsync(int id);
}
