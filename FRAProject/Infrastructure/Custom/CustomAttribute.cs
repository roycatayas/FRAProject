using System;

namespace FRA.Web.Infrastructure.Custom
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomAttribute : Attribute
    {
        private string _viewPath;

        public CustomAttribute(string viewPath)
        {
            this._viewPath = viewPath;
        }

        public string ViewPath { get; set; }
    }
}
