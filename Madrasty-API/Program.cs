using Madrasty.Entites;
using Madrasty_API.Entities;
using Madrasty_API.Helpers;
using Madrasty_API.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<Jwt>();
builder.Services.AddSingleton(jwtOptions);

// add swagger gen


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<StudentProfile>();
    x.AddProfile<DepartmentProfile>();
    x.AddProfile<CourseProfile>();
});

builder.Services.AddDbContext<ApplicationDbContext>(cfg =>
{
    cfg.UseLazyLoadingProxies()
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<Madrasty_API.UnitOfWork.UnitOfWork>();

string users = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(users,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).
AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Topics.Any())
    {
        dbContext.Topics.AddRange(
            new Topic { Name = "Programming" },
            new Topic { Name = "Databases" },
            new Topic { Name = "Web Development" }
        );
    }

    if (!dbContext.Departments.Any())
    {
        dbContext.Departments.AddRange(
            new Department { Name = "Computer Science", Location = "Main Campus" },
            new Department { Name = "Information Systems", Location = "Main Campus" }
        );
    }

    dbContext.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(users);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
