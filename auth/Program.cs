using AuthServer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

AuthServerInitializer.InitializeAuthStore(builder.Services, builder.Configuration);

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            // policy.WithOrigins(builder.Configuration["ClientUrl"] ?? throw new InvalidOperationException("ClientUrl is not configured."))
            policy.WithOrigins("http://localhost:4200")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
        });
});

var app = builder.Build();

AuthServerInitializer.InitializeAuthData(app.Services, app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseIdentityServer();
app.MapGet("/", () => "IdentityServer is running!");

app.Run();

