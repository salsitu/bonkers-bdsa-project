using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Web;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Server.Entities;
using ProjectBank.Server;
using Server.Entities;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options => builder.Configuration.Bind("AzureAD", options), options =>
    {
        builder.Configuration.Bind("AzureAD", options);
        options.TokenValidationParameters.RoleClaimType = "https://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ProjectBankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sql_server")));
builder.Services.AddScoped<ProjectBankContext>();
builder.Services.AddScoped<IDBFacade, DBFacade>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

if (!app.Environment.IsEnvironment("Integration"))
{
    await app.SeedAsync();
}


app.Run();

public partial class Program { }
