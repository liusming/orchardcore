using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.DisplayManagement.Zones;

namespace OrchardCore.Localization
{
    public class AdminCulturePickerShapes : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Layout")
                .OnDisplaying(async context =>
                {
                    if (context.Shape is IZoneHolding layout)
                    {
                        var httpContextAccessor = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                        if (!AdminAttribute.IsApplied(httpContextAccessor.HttpContext))
                        {
                            return;
                        }

                        var shapeFactory = context.ServiceProvider.GetRequiredService<IShapeFactory>();
                        var culturePickerShape = await shapeFactory.CreateAsync("CulturePicker");

                        await layout.Zones["NavbarTop"].AddAsync(culturePickerShape);
                    }
                });
        }
    }
}