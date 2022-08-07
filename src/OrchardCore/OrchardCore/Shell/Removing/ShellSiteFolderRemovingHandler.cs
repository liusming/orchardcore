using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OrchardCore.Environment.Shell.Removing;

/// <summary>
/// Allows to remove site folder of a given tenant.
/// </summary>
public class ShellSiteFolderRemovingHandler : IShellRemovingHandler
{
    private readonly ShellOptions _shellOptions;
    private readonly IStringLocalizer S;
    private readonly ILogger _logger;

    public ShellSiteFolderRemovingHandler(
        IOptions<ShellOptions> shellOptions,
        IStringLocalizer<ShellSiteFolderRemovingHandler> localizer,
        ILogger<ShellSiteFolderRemovingHandler> logger)
    {
        _shellOptions = shellOptions.Value;
        S = localizer;
        _logger = logger;
    }

    /// <summary>
    /// Removes the site folder of the provided tenant.
    /// </summary>
    public Task RemovingAsync(ShellRemovingContext context)
    {
        var shellAppDataFolder = Path.Combine(
            _shellOptions.ShellsApplicationDataPath,
            _shellOptions.ShellsContainerName,
            context.ShellSettings.Name);

        try
        {
            Directory.Delete(shellAppDataFolder, true);
        }
        catch (Exception ex) when (ex is DirectoryNotFoundException)
        {
        }
        catch (Exception ex) when ((ex.HResult & 0x0000FFFF) == 80)
        {
            // Sharing violation, may happen if multiple nodes share the same file system
            // without using a distributed lock, in that case let another node do the job.
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to remove the site folder '{TenantFolder}' of tenant '{TenantName}'.",
                shellAppDataFolder,
                context.ShellSettings.Name);

            context.LocalizedErrorMessage = S["Failed to remove the site folder '{0}'.", shellAppDataFolder];
            context.Error = ex;
        }

        return Task.CompletedTask;
    }
}
