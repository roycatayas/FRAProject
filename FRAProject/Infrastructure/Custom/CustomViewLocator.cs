using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace FRA.Web.Infrastructure.Custom
{
    public class CustomViewLocator : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // Get Attribute Value
            var descriptor = (context.ActionContext.ActionDescriptor as ControllerActionDescriptor);
            var viewPath = descriptor.ControllerTypeInfo.CustomAttributes?.FirstOrDefault(rc => rc.AttributeType == typeof(CustomAttribute))?.ConstructorArguments[0].Value.ToString();

            var additionalLocations = new LinkedList<string>();

            // If the attribute exists it will add this
            if (viewPath != null)
            {
                additionalLocations.AddLast($"/{viewPath}/Views/{{1}}/{{0}}.cshtml");
                additionalLocations.AddLast($"/{viewPath}/Views/{{0}}.cshtml");
                additionalLocations.AddLast($"/{viewPath}/Views/_Shared/{{0}}.cshtml");
                additionalLocations.AddLast($"/{viewPath}/Views/_Partials/{{0}}.cshtml");
            }

            // if not it will use this
            additionalLocations.AddLast("/Views/{1}/{0}.cshtml");
            additionalLocations.AddLast("/Views/{0}.cshtml");
            additionalLocations.AddLast("/Views/_Partials/{0}.cshtml");
            return viewLocations.Concat(additionalLocations).Select(x => x.Replace("/Shared", "/_Shared"));
            //return new[]
            //{
            //    "/Area/Administration/Views/{1}/{0}.cshtml",
            //    "/Area/Administration/Views/Shared/{0}.cshtml"
            //}; 
            //return viewLocations.Select(d => d.Replace("/Views/", "/Administration/"));
        }
    }
}
