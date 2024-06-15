using ContactsWebAPI.Models.Abstract;
using ContactsWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebAPI.Models.Concrete;

public class SQLContactRepository(ContactDbContext dbContext) : IContactRepository
{
  private readonly ContactDbContext _dbContext = dbContext;

  public async Task<bool> CreateContactAsync(Contact contact)
  {
    _dbContext.Contacts.Add(contact);
    return await _dbContext.SaveChangesAsync() > 0;
  }

  public async Task<bool> DeleteContactAsync(int id)
  {
    Contact contact = _dbContext.Contacts.Find(id);
    if (contact != null)
    {
      _dbContext.Contacts.Remove(contact);
      return await _dbContext.SaveChangesAsync() > 0;
    }
    return false;
  }

  public async Task<Contact> GetContactAsync(int id) => await _dbContext.Contacts.FindAsync(id);

  public async Task<IEnumerable<Contact>> GetContactsAsync() => await _dbContext.Contacts.ToListAsync();

  public async Task<IEnumerable<Contact>> GetContactsByNameAsync(string contactName) => await _dbContext.Contacts.Where(c=>c.ContactName == contactName).ToListAsync();

  public async Task<IEnumerable<Contact>> GetContactsByPhoneAsync(string phoneNumber) => await _dbContext.Contacts.Where(c => c.Phone == phoneNumber).ToListAsync();

  public async Task<IEnumerable<Contact>> GetContactsByTypeAsync(string contactType) => await _dbContext.Contacts.Where(c => c.ContactType == contactType).ToListAsync();

  public async Task<bool> UpdateContactAsync(Contact contact)
  {
    _dbContext.Entry(contact).State = EntityState.Modified;
    return await _dbContext.SaveChangesAsync() > 0;
  }
}
