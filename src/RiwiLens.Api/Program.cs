using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using src.RiwiLens.Infrastructure.Persistence;
using src.RiwiLens.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Identity;
using src.RiwiLens.Infrastructure.Identity;


var builder = WebApplication.CreateBuilder(args);

Env.Load("../../.env");

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

    if (app.Environment.IsDevelopment())
    {
        // Identity
        await IdentitySeeder.SeedAsync(userManager, roleManager);
        await AdminSeed.SeedAdminsAsync(userManager, roleManager);

        // Core catalog
        await CategoryTechnicalSkillSeed.SeedAsync(context);
        await ClassTypeSeed.SeedAsync(context);
        await DaySeed.SeedAsync(context);
        await RoleTeamLeaderSeed.SeedAsync(context);
        await SoftSkillSeed.SeedAsync(context);
        await SpecialtySeed.SeedAsync(context);
        await TechnicalSkillSeed.SeedAsync(context);
        

        Console.WriteLine("✅ All seeds executed successfully");
    }
}


app.Run();