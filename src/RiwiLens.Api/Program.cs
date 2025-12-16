using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using src.RiwiLens.Infrastructure;
using src.RiwiLens.Infrastructure.Persistence;
using src.RiwiLens.Infrastructure.Data.Seed;
using src.RiwiLens.Infrastructure.Services.Identity;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Domain.Entities;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();


var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ========================
// IDENTITY
// ========================
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



// ========================
// JWT
// ========================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JWT_ISSUER"]
            ?? throw new InvalidOperationException("JWT_ISSUER not configured"),

        ValidAudience = builder.Configuration["JWT_AUDIENCE"]
            ?? throw new InvalidOperationException("JWT_AUDIENCE not configured"),

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["JWT_KEY"]
                ?? throw new InvalidOperationException("JWT_KEY not configured")
            )
        ),

        ClockSkew = TimeSpan.Zero
    };
});


// ========================
// SERVICES
// ========================
builder.Services.AddInfrastructure(builder.Configuration);


// ==========================================
// SWAGGER
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
    
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RiwiLens API",
        Version = "v1",
        Description = "API for RiwiLen"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

// Endpoint de prueba raíz
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// ==========================================
// DATABASE CONNECTION TEST
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        db.Database.Migrate();
        Console.WriteLine("✅ API connected to Database successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database connection error: {ex.Message}");
    }
}
// ==========================================
// SEEDS
// ==========================================
/*
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
*/
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var env = services.GetRequiredService<IWebHostEnvironment>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

    if (env.IsDevelopment())
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await AdminSeed.SeedAdminsAsync(userManager, roleManager);
    }
}



app.Run();