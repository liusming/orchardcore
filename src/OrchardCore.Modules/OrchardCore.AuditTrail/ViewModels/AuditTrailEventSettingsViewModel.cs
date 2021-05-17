using Microsoft.Extensions.Localization;

namespace OrchardCore.AuditTrail.ViewModels
{
    public class AuditTrailEventSettingsViewModel
    {
        public string FullName { get; set; }
        public LocalizedString LocalizedName { get; set; }
        public LocalizedString Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsMandatory { get; set; }
    }
}
