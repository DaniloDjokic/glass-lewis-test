using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer;

public static class AuthServerInitializer
{
    public static void InitializeAuthStore(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                            b.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                                sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                            b.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                                sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
            })
            .AddDeveloperSigningCredential() //INFO: Only for development
            .AddProfileService<ProfileService>()
            .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();


        services.AddIdentityCore<User>(options => { })
            .AddEntityFrameworkStores<UserDbContext>();
    }

    public static void InitializeAuthData(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;

            var configDbContext = services.GetRequiredService<ConfigurationDbContext>();
            var persDbContext = services.GetRequiredService<PersistedGrantDbContext>();
            var userDbContext = services.GetRequiredService<UserDbContext>();
            var passwordHasher = services.GetRequiredService<IPasswordHasher<User>>();

            configDbContext.Database.Migrate();
            persDbContext.Database.Migrate();
            userDbContext.Database.Migrate();

            var authScope = configuration["auth:Scope"] ?? throw new ArgumentNullException("Scope is not set");

            if (!configDbContext.Clients.Any())
            {
                var clientSecret = configuration["auth:ClientSecret"] ?? throw new ArgumentNullException("ClientSecret is not set");

                var client = new Client
                {
                    ClientId = configuration["auth:ClientId"] ?? throw new ArgumentNullException("ClientId is not set"),
                    ClientSecrets = { new Secret(clientSecret.Sha256()) },
                    AllowedGrantTypes = { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
                    AllowedScopes = { authScope }
                };

                configDbContext.Clients.Add(client.ToEntity());
                configDbContext.SaveChanges();
            }

            if (!configDbContext.ApiResources.Any())
            {
                var apiResource = new ApiResource(authScope, "glass-lewis-api");
                configDbContext.ApiResources.Add(apiResource.ToEntity());
                configDbContext.SaveChanges();
            }

            if (!configDbContext.ApiScopes.Any())
            {
                var apiScope = new ApiScope(authScope);
                configDbContext.ApiScopes.Add(apiScope.ToEntity());
                configDbContext.SaveChanges();
            }

            if (!userDbContext.Users.Any())
            {
                var username = configuration["Auth:TestUser:Username"] ?? throw new ArgumentNullException("Test username is not set");
                var password = configuration["Auth:TestUser:Password"] ?? throw new ArgumentNullException("Test password is not set");

                var user = new User { Username = username };
                user.PasswordHash = passwordHasher.HashPassword(user, password);
                userDbContext.Users.Add(user);
                userDbContext.SaveChanges();
            }
        }
    }
}

