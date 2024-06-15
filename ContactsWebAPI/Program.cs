using ContactsWebAPI.Models;
using ContactsWebAPI.Models.Abstract;
using ContactsWebAPI.Models.Concrete;
using ContactsWebAPI.Models.Entities;
using ContactsWebAPI.Models.SeedDatas;
using ContactsWebAPI.Models.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
  builder.AddConsole();
});

ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

builder.Services.ConfigureOptions<DbOptionSetup>();
builder.Services.ConfigureOptions<EFOptionSetup>();
builder.Services.AddDbContext<ContactDbContext>((serviceProvider, cfg) => {
  var dbUtility = serviceProvider.GetService<IOptions<DbUtility>>()!.Value;
  var efUtility = serviceProvider.GetService<IOptions<EFUtility>>()!.Value;
  //logger.LogInformation($"dbUtility.DbConnectionString: {dbUtility.DbConnectionString}");
  //logger.LogInformation($"efUtility.MaxReTryCount: {efUtility.MaxReTryCount}, efUtility.MaxReTryDelay: {efUtility.MaxReTryDelay}, efUtility.CommandTimeout: {efUtility.CommandTimeout}");
  cfg.UseSqlServer(dbUtility.DbConnectionString, sqlOptions => {
    sqlOptions.EnableRetryOnFailure(maxRetryCount: efUtility.MaxReTryCount, maxRetryDelay: TimeSpan.FromSeconds(efUtility.MaxReTryDelay), errorNumbersToAdd: null);
    sqlOptions.CommandTimeout(efUtility.CommandTimeout);
  });
  cfg.EnableDetailedErrors(efUtility.EnableDetailedErrors);
  cfg.EnableDetailedErrors(efUtility.EnableSensitiveDataLogging);
  if (efUtility.EnableLogToConsole)
  {
    //cfg.LogTo(Console.WriteLine);
    cfg.LogTo(Console.WriteLine, LogLevel.Information);
  }
});

builder.Services.AddScoped<IContactRepository, SQLContactRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  //Will return a null if T is not registered
  //var dbContext = scope.ServiceProvider.GetService<ContactDbContext>();
  //Will not return a null if T is not registered, will throw a InvalidOperationException if T is not registered
  var dbContext = scope.ServiceProvider.GetRequiredService<ContactDbContext>();

  if (dbContext.Database.EnsureCreated())
  {
    logger.LogInformation("Database created...");
  }
  else
  {
    logger.LogInformation("Database already exists...");
    //if (dbContext.Database.EnsureDeleted())
    //{ 
    //  logger.LogInformation("Database deleted...");
    //}
  }

  if (!dbContext.Contacts.Any())
  {
    dbContext.Contacts.AddRange(ContactSeedData.CreateContacts);
    if (dbContext.SaveChanges() > 0)
    {
      logger.LogInformation($"Seeded '{dbContext.Contacts.Count()}' contacts...");
    }
  }
}

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

app.UseHttpsRedirection();



#region With Results
//app.MapGet("/contacts", async (IContactRepository contactRepository) =>
//{
//  return Results.Ok(await contactRepository.GetContactsAsync());
//})
//.WithName("GetContacts")
//.WithOpenApi();

//app.MapGet("/contacts/{id:int}", async (int id, IContactRepository contactRepository) =>
//{
//  var result = await contactRepository.GetContactAsync(id);
//  if (result != null)
//    return Results.Ok<Contact>(result); // <Contact>
//  return Results.NotFound($"Contact with id '{id}' not found");
//})
//.WithName("GetContact")
//.WithOpenApi();


//app.MapPost("/contacts", async (Contact contact, IContactRepository contactRepository) =>
//{
//  var result = await contactRepository.CreateContactAsync(contact);
//  if (result)
//    return Results.Created($"/contacts/{contact.Id}", contact);
//  return Results.BadRequest($"Could not create contact with Name: '{contact.ContactName}'");
//})
//  .WithName("CreateContact")
//  .WithOpenApi();

//app.MapPut("/contacts", async (Contact contact, IContactRepository contactRepository) =>
//{
//  var result = await contactRepository.UpdateContactAsync(contact);
//  if (result)
//    return Results.Ok(contact);
//  return Results.BadRequest($"Could not update contact with ContactId: '{contact.Id}' and Name: '{contact.ContactName}'");
//})
//  .WithName("UpdateContact")
//  .WithOpenApi();


//app.MapDelete("/contacts/{id:int}", async (int id, IContactRepository contactRepository) =>
//{
//  var result = await contactRepository.DeleteContactAsync(id);
//  if (result)
//    return Results.Ok($"Contact with id '{id}' deleted successfully");
//  return Results.NotFound($"Contact with id '{id}' not found");
//})
//  .WithName("DeleteContact")
//  .WithOpenApi(); 
#endregion


#region With TypedResults
app.MapGet("/contacts", async (IContactRepository contactRepository) =>
{
  return TypedResults.Ok(await contactRepository.GetContactsAsync());
})
.WithName("GetContacts")
.WithOpenApi();

app.MapGet("/contacts/{id:int}", async Task<Results<Ok<Contact>, NotFound<string>>> (int id, IContactRepository contactRepository) =>
{
  var result = await contactRepository.GetContactAsync(id);
  if (result != null)
    return TypedResults.Ok<Contact>(result); // <Contact>
  return TypedResults.NotFound($"Contact with id '{id}' not found");
})
.WithName("GetContact")
.WithOpenApi();


app.MapPost("/contacts", async Task<Results<Created<Contact>, BadRequest<string>>> (Contact contact, IContactRepository contactRepository) =>
{
  var result = await contactRepository.CreateContactAsync(contact);
  if (result)
    return TypedResults.Created($"/contacts/{contact.Id}", contact);
  return TypedResults.BadRequest($"Could not create contact with Name: '{contact.ContactName}'");
})
  .WithName("CreateContact")
  .WithOpenApi();

app.MapPut("/contacts", async Task<Results<Ok<Contact>, BadRequest<string>>> (Contact contact, IContactRepository contactRepository) =>
{
  var result = await contactRepository.UpdateContactAsync(contact);
  if (result)
    return TypedResults.Ok(contact);
  return TypedResults.BadRequest($"Could not update contact with ContactId: '{contact.Id}' and Name: '{contact.ContactName}'");
})
  .WithName("UpdateContact")
  .WithOpenApi();


app.MapDelete("/contacts/{id:int}", async Task<Results<Ok<string>, NotFound<string>>> (int id, IContactRepository contactRepository) =>
{
  var result = await contactRepository.DeleteContactAsync(id);
  if (result)
    return TypedResults.Ok($"Contact with id '{id}' deleted successfully");
  return TypedResults.NotFound($"Contact with id '{id}' not found");
})
  .WithName("DeleteContact")
  .WithOpenApi();

#endregion

app.Run();


