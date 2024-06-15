using ContactsWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebAPI.Models
{
  public class ContactDbContext(DbContextOptions<ContactDbContext> dbContextOptions) : DbContext(dbContextOptions)
  {
    public DbSet<Contact> Contacts { get; set; }
  }
}
