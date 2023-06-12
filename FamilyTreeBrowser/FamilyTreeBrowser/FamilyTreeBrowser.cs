using FamilyTreeLogic.Exceptions;
using FamilyTreeLogic.Interfaces;
using FamilyTreeLogic.Models;
using FamilyTreeLogic.Services;
using FamilyTreeLogic.Wrappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FamilyTreeBrowser;

public class FamilyTreeBrowser
{
    private static ILogger<FamilyTreeBrowser>? _logger;
    private static IUserInterfaceService? _userInterfaceService;
    private static IFamilyTreeService? _familyTreeService;

    private static IHost? _host;

    private static readonly string _applicationName = nameof(FamilyTreeBrowser);

    /// <summary>
    /// Entry point of the Application
    /// </summary>
    /// <param name="args">Arguments from the Console</param>
    public static void Main(string[] args)
    {
        _host = CreateHostBuilder(args).Build();

        initializeServices();

        runClient(args);
    }

    private static void initializeServices()
    {
        _logger = _host.Services.GetRequiredService<ILogger<FamilyTreeBrowser>>();
        _userInterfaceService = _host.Services.GetRequiredService<IUserInterfaceService>();
        _familyTreeService = _host.Services.GetRequiredService<IFamilyTreeService>();
    }

    private static void runClient(string[] args)
    {
        try
        {
            UserRequest request = _userInterfaceService?.BuildReguest(args);

            _familyTreeService?.Initialize(request);

            var processedData = _familyTreeService.RunProcess();

            _userInterfaceService.DisplayData(processedData);

        }
        catch(UserException ex)
        {
            logMessageAndExit(ex.Message);
        }
        catch(ValidationException ex)
        {
            logMessageAndExit(ex.Message);
        }
        catch (Exception ex)
        {
            logMessageAndExit(ex.Message);
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("appSettings.json", true, true)
               .Build();

                services.AddSingleton(config.GetSection(nameof(AppSettings))
                    .Get<AppSettings>());
               
                services.AddTransient<IUserInterfaceService, UserInterfaceService>();
                services.AddTransient<IFamilyTreeService, FamilyTreeService>();
                services.AddScoped<IFileWrapper, FileWrapper>();
            });

    private static void logMessageAndExit(string message)
    {
        _logger.LogError(message);
        Environment.Exit(-1);
    }
}