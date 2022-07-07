using System.Threading.Tasks;

namespace OrchardCore.Environment.Shell.Removing;

/// <summary>
/// Host service that collaborates to the removing process of a given tenant.
/// </summary>
public interface IShellRemovingHandler
{
    /// <summary>
    /// Collaborates to the removing process of the provided tenant.
    /// </summary>
    Task RemovingAsync(ShellRemovingContext context);
}
