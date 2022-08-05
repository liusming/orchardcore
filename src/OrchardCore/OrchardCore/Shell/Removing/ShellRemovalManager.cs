using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell.Builders;
using OrchardCore.Environment.Shell.Models;
using OrchardCore.Modules;

namespace OrchardCore.Environment.Shell.Removing;

public class ShellRemovalManager : IShellRemovalManager
{
    private readonly IShellHost _shellHost;
    private readonly IShellContextFactory _shellContextFactory;
    private readonly IEnumerable<IShellRemovingHandler> _shellRemovingHandlers;
    private readonly ILogger _logger;

    public ShellRemovalManager(
        IShellHost shellHost,
        IShellContextFactory shellContextFactory,
        IEnumerable<IShellRemovingHandler> shellRemovingHandlers,
        ILogger<ShellRemovalManager> logger)
    {
        _shellHost = shellHost;
        _shellContextFactory = shellContextFactory;
        _shellRemovingHandlers = shellRemovingHandlers;
        _logger = logger;
    }

    public async Task<ShellRemovingContext> RemoveAsync(ShellSettings shellSettings, bool localResourcesOnly = false)
    {
        var context = new ShellRemovingContext
        {
            ShellSettings = shellSettings,
            LocalResourcesOnly = localResourcesOnly,
        };

        if (shellSettings.Name == ShellHelper.DefaultShellName)
        {
            context.ErrorMessage = $"The tenant should not be the '{ShellHelper.DefaultShellName}' tenant.";
            return context;
        }

        if (shellSettings.State != TenantState.Disabled && shellSettings.State != TenantState.Uninitialized)
        {
            context.ErrorMessage = $"The tenant '{shellSettings.Name}' should be 'Disabled' or 'Uninitialized'.";
            return context;
        }

        // Check if the tenant is not 'Uninitialized' and that all resources should be removed.
        if (shellSettings.State == TenantState.Disabled && !context.LocalResourcesOnly)
        {
            // Create an isolated shell context composed of all features that have been installed.
            using var shellContext = await _shellContextFactory.CreateMaximumContextAsync(shellSettings);
            (var locker, var locked) = await shellContext.TryAcquireShellRemovingLockAsync();
            if (!locked)
            {
                _logger.LogError(
                    "Failed to acquire a lock before executing the tenant handlers while removing the tenant '{TenantName}'.",
                    shellSettings.Name);

                context.ErrorMessage = $"Failed to acquire a lock before executing the tenant handlers.";
                return context;
            }

            await using var acquiredLock = locker;

            await shellContext.CreateScope().UsingServiceScopeAsync(async scope =>
            {
                // Execute tenant level removing handlers (singletons or scoped) in a reverse order.
                // If feature A depends on feature B, the activating handler of feature B should run
                // before the handler of the dependent feature A, but on removing the resources of
                // feature B should be removed after the resources of the dependent feature A.
                var tenantHandlers = scope.ServiceProvider.GetServices<IModularTenantEvents>().Reverse();
                foreach (var handler in tenantHandlers)
                {
                    try
                    {
                        await handler.RemovingAsync(context);
                        if (!context.Success)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        var type = handler.GetType().FullName;

                        _logger.LogError(
                            ex,
                            "Failed to execute the tenant handler '{TenantHandler}' while removing the tenant '{TenantName}'.",
                            type,
                            shellSettings.Name);

                        context.ErrorMessage = $"Failed to execute the tenant handler '{type}'.";
                        context.Error = ex;

                        break;
                    }
                }
            });
        }

        if (_shellHost.TryGetSettings(ShellHelper.DefaultShellName, out var defaultSettings))
        {
            // Use the default shell context to execute the host level removing handlers.
            var shellContext = await _shellHost.GetOrCreateShellContextAsync(defaultSettings);
            (var locker, var locked) = await shellContext.TryAcquireShellRemovingLockAsync();
            if (!locked)
            {
                _logger.LogError(
                    "Failed to acquire a lock before executing the host handlers while removing the tenant '{TenantName}'.",
                    shellSettings.Name);

                context.ErrorMessage = $"Failed to acquire a lock before executing the host handlers.";

                // If only local resources should be removed while syncing tenants.
                if (context.LocalResourcesOnly)
                {
                    // Indicates that we can retry in a next loop.
                    context.FailedOnLockTimeout = true;
                }

                return context;
            }

            await using var acquiredLock = locker;

            // Execute host level removing handlers in a reverse order.
            foreach (var handler in _shellRemovingHandlers.Reverse())
            {
                try
                {
                    await handler.RemovingAsync(context);
                    if (!context.Success)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    var type = handler.GetType().FullName;

                    _logger.LogError(
                        ex,
                        "Failed to execute the host handler '{HostHandler}' while removing the tenant '{TenantName}'.",
                        type,
                        shellSettings.Name);

                    context.ErrorMessage = $"Failed to execute the host handler '{type}'.";
                    context.Error = ex;

                    break;
                }
            }
        }

        return context;
    }
}
