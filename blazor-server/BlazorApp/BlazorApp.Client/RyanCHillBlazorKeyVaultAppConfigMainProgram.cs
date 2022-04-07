using Azure.Identity;
using BlazorApp.Client;
using BlazorApp.Client.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Identity.Web;
using static System.Console;

var builder = WebApplication.CreateBuilder(args);
/*
builder.Configuration.AddAzureAppConfiguration(config => {
    var credentials = new DefaultAzureCredential();
    var s = builder.Configuration.ToString();
    s = builder.Configuration.GetConnectionString("AppConfig").ToString();
    var endpoint = builder.Configuration["ConnectionStrings:AppConfig"].ToString();
    var options = config.Connect(endpoint);
    options.ConfigureKeyVault(kv => kv.SetCredential(credentials));
});
*/

//if (builder.Environment.IsDevelopment()) builder.Configuration.AddUserSecrets<Program>();// https://stackoverflow.com/questions/69645288/configuration-doesnt-pick-connection-string-from-secrets-json-file-in-asp-net-c
//builder.Configuration.SetBasePath(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Roaming\Microsoft\UserSecrets\40e0aa80-df3e-45e6-87d2-200bc3bce925")).AddJsonFile("secrets.json"); // https://stackoverflow.com/questions/71492149/how-to-get-connectionstring-from-secrets-json-in-asp-net-core-6

/*
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
*/

/*
builder.WebHost.ConfigureAppConfiguration(config =>
{
    var settings = config.Build();
    var connectionString = settings.GetConnectionString("AppConfig");
    if (connectionString is null)
        connectionString = FetchConnectionStringFromSecrets.json(@".");
    config.AddAzureAppConfiguration(options => {
        var credentials = new DefaultAzureCredential();
        var endpoint = builder.Configuration.ToString();
        options.Connect(connectionString);
        options.ConfigureKeyVault(options =>
        {
            options.SetCredential(credentials);
        });
        //var endpoint = builder.Configuration.GetConnectionString("AppConfig");
        //options.Connect(new Uri(endpoint), credentials).ConfigureKeyVault(kv =>  {  kv.SetCredential(credentials);  });

    });    
});*/



/* System.NotSupportedException
HResult = 0x80131515
  Message = The web root changed from "c:\Users\shein\source\repos\AppServiceDotNet\blazor-server\BlazorApp\BlazorApp.Client\wwwroot" to "c:\Users\shein\source\repos\AppServiceDotNet\blazor-server\BlazorApp\BlazorApp.Client\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.
  Source=Microsoft.AspNetCore
  StackTrace:
   at Microsoft.AspNetCore.Builder.ConfigureWebHostBuilder.ConfigureAppConfiguration(Action`2 configureDelegate)
   at Microsoft.AspNetCore.Hosting.WebHostBuilderExtensions.ConfigureAppConfiguration(IWebHostBuilder hostBuilder, Action`1 configureDelegate)
   at Program.<Main>$(String[] args) in c:\Users\shein\source\repos\AppServiceDotNet\blazor-server\BlazorApp\BlazorApp.Client\RyanCHillBlazorKeyVaultAppConfigMainProgram.cs:line 24
*/

/*

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
*/
/*
var webAppOptions = new WebApplicationOptions();

WebApplication.CreateBuilder(webAppOptions);

// Add Microsoft Authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
*/

// https://docs.microsoft.com/en-us/azure/azure-app-configuration/quickstart-aspnet-core-app?tabs=core6x
//Retrieve the Connection String from the secrets manager 
var connectionString = builder.Configuration.GetConnectionString("AppConfig");
if (connectionString is null)
    connectionString = FetchConnectionStringFromSecrets.json(@".");

builder.Host.ConfigureAppConfiguration(builder =>
{
    //Connect to your App Config Store using the connection string
    builder.AddAzureAppConfiguration(options =>
    {
        options.Connect(connectionString);
        options.ConfigureKeyVault(options => {
            options.SetCredential(new DefaultAzureCredential());
        });
    });
})
            .ConfigureServices(services =>
            {
                services.AddControllersWithViews();
            });

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
WriteLine($"secret={0}", app.Configuration["clientSecret"]);

app.Run();
