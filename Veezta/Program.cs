
using Application.Contracts;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastrucutre;
using Infrastrucutre.Data;
using Infrastrucutre.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VeeztaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString"));
});



builder.Services.AddIdentity<CustomUser, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.Password.RequireDigit = true; // Requires a number
    options.Password.RequireLowercase = true; // Requires lowercase letter
    options.Password.RequireUppercase = true; // Requires uppercase letter
    options.Password.RequiredLength = 8; // Set the minimum length here
    options.Password.RequireNonAlphanumeric = true; // Set to true if you require a non-alphanumeric character
})
            .AddEntityFrameworkStores<VeeztaDbContext>()
            .AddDefaultTokenProviders();


builder.Services.AddScoped(typeof(IAdminRepository), typeof(AdminRepository));
builder.Services.AddScoped<ISpecializationRepository,  SpecailizationRepoistory>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepoistory>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped< IDoctorService , DoctorService>();
builder.Services.AddScoped<IPatientService ,PatientService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAdminService, AdminServices>();



var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.AddSingleton(smtpSettings);
builder.Services.AddScoped<IEmailSender, EmailSender>();












var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomUser>>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    await DataSeeder.SeedData(userManager, roleManager);
//   await DataSeeder.EnsureRolesAsync(roleManager);
  
//}

var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
await DataSeeder.EnsureRolesAsync(roleManager);




app.Run();
