 // ReSharper disable once CheckNamespace
namespace NCrunch.Framework
{
    using System;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public abstract class ResourceUsageAttribute : Attribute
    {
        private readonly string[] resourceNames;

        protected ResourceUsageAttribute(params string[] resourceName)
        {
            resourceNames = resourceName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] ResourceNames
        {
            get
            {
                return resourceNames;
            }
        }
    }
}