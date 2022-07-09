using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Shortcodes
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageShortcodeTemplates = new Permission("ManageShortcodeTemplates", "Manage shortcode templates", isSecurityCritical: true);

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageShortcodeTemplates }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = BuiltInRole.Administrator,
                    Permissions = new[] { ManageShortcodeTemplates }
                },
                new PermissionStereotype
                {
                    Name = BuiltInRole.Editor,
                    Permissions = new[] { ManageShortcodeTemplates }
                }
            };
        }
    }
}
