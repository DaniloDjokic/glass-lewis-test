using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;

namespace AuthServer;

public class CustomResourceOwnerPasswordValidator(UserDbContext userDbContext, IPasswordHasher<User> passwordHasher) : IResourceOwnerPasswordValidator
{
    public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = userDbContext.Users.FirstOrDefault(u => u.Username == context.UserName);

        if (user is not null && passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, context.Password) == PasswordVerificationResult.Success)
        {
            context.Result = new GrantValidationResult(
                    subject: user.Id.ToString(),
                    authenticationMethod: "password",
                    claims: new List<Claim>
                    {
                        new Claim("sub", user.Id.ToString()),
                        new Claim("name", user.Username)
                    });
        }
        else
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
        }

        return Task.CompletedTask;
    }
}
