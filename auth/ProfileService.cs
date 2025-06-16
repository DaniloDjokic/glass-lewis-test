using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace AuthServer;

public class ProfileService(UserDbContext userDbContext) : IProfileService
{
    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (sub is not null)
        {
            var user = userDbContext.Users.FirstOrDefault(u => u.Id.ToString() == sub);

            if (user is not null)
            {
                context.IssuedClaims.Add(new Claim("sub", user.Username));
                context.IssuedClaims.Add(new Claim("name", user.Username));
            }
        }

        return Task.CompletedTask;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (sub is not null)
        {
            context.IsActive = userDbContext.Users.Any(u => u.Id.ToString() == sub);
        }

        return Task.CompletedTask;
    }
}
