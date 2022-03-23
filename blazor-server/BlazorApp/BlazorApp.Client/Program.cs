using Azure.Identity;
using BlazorApp.Client.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Configure App Configuration
builder.Host.ConfigureWebHostDefaults(webHostBuilder =>
{
    webHostBuilder.ConfigureAppConfiguration((context, config) =>
    {
        var credentials = new DefaultAzureCredential();
        config.AddAzureAppConfiguration(options =>
        {
            var endpoint = builder.Configuration["AzureAppConfiguration:Endpoint"].ToString();
            options.Connect(new Uri(endpoint), credentials).ConfigureKeyVault(kv =>
            {
                kv.SetCredential(credentials);
            });
        });
    });
});

// Add Microsoft Authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
