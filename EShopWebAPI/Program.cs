using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Domain.Entities;
using Application.Common.Mappings;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Configurations;
using Domain.Common.Roles;
using Application.Services;
using Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistence classes
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configuration manager
builder.Services.AddScoped<Domain.Interfaces.IConfigurationManager, Infrastructure.Configurations.ConfigurationManager>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// Service classes
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<ProductServices>();
builder.Services.AddScoped<CartServices>();
// builder.Services.AddScoped<FigureServices>();
builder.Services.AddScoped<OrderService>();

// AutoMapper service
// Quet project, tim tat ca file MappingProfile roi gop lai thanh 1
// Mapping profile co san trong /Application/Common/Mappings/
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Helper services
builder.Services.AddScoped<JwtHelper>();


// EF Identity configurations
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole<Guid>>>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT configurations
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue(builder.Configuration["AuthTokenOptions:Name"], out var token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CustomerPolicy", policy => policy.RequireRole(UserRoles.Customer, UserRoles.Admin))
    .AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRoles.Admin));

builder.Services.Configure<AuthTokenOptions>(
    builder.Configuration.GetSection("AuthTokenOptions")
);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost") // Allow any localhost port
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Update database to use the lastest migrations, ignored if database is up to date
    var db = services.GetRequiredService<ApplicationDbContext>();

    var pendingMigrations = db.Database.GetPendingMigrations().ToList();
    if (pendingMigrations.Count != 0)
    {
        db.Database.Migrate();
    }

    // Add the roles
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Admin));
    }

    if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
    {
        await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Customer));
    }

    var userManager = services.GetRequiredService<UserManager<User>>();

    var passwordValidators = userManager.PasswordValidators;
    var userValidators = userManager.UserValidators;
    userManager.PasswordValidators.Clear();
    userManager.UserValidators.Clear();

    var admin = await userManager.FindByNameAsync("admin");
    if (admin == null)
    {
        admin = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            Email = "admin@animeforum.com"
        };

        await userManager.CreateAsync(admin, "admin");
        await userManager.AddToRoleAsync(admin, UserRoles.Admin);
    }

    var customer = await userManager.FindByNameAsync("customer");
    if (customer == null)
    {
        customer = new User
        {
            Id = Guid.NewGuid(),
            UserName = "customer",
            Email = "member@figureshop.com"
        };

        await userManager.CreateAsync(customer, "customer");
        await userManager.AddToRoleAsync(customer, UserRoles.Customer);
    }

    foreach (var validator in passwordValidators)
    {
        userManager.PasswordValidators.Add(validator);
    }

    foreach (var validator in userValidators)
    {
        userManager.UserValidators.Add(validator);
    }


    if (db.ChangeTracker.HasChanges())
    {
        await db.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        Console.WriteLine($"Serving static file: {ctx.File.PhysicalPath}");
    }
});

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
