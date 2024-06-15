using Microsoft.Extensions.Options;

namespace ContactsWebAPI.Models.Utilities;

public class EFOptionSetup(IConfiguration configuration) : IConfigureOptions<EFUtility>
{

  private readonly IConfiguration _configuration = configuration;
  private readonly string EFOptionSection = "EFOptions";
  public void Configure(EFUtility utility)
  {
    _configuration.GetSection(EFOptionSection).Bind(utility);
  }
}
