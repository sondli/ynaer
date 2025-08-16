using Microsoft.AspNetCore.Identity;
using YNAER.Domain.Entities;
using YNAER.Infrastructure.Identity;
using YNAER.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddTransient<IUserStore<ApplicationUser>, DapperUserStore>();
builder.Services.AddTransient<IRoleStore<IdentityRole<Guid>>, DapperRoleStore>();
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(builder.Configuration.GetConnectionString("Database")!));

builder.Services
    .AddIdentity<ApplicationUser,
        IdentityRole<Guid>>(options => // We use IdentityRole as a placeholder since we aren't implementing a role store yet
    {
        // Configure password settings, lockout, etc. here if needed
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddUserStore<DapperUserStore>()
    .AddRoleStore<DapperRoleStore>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();