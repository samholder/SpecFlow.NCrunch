 // ReSharper disable once CheckNamespace
namespace NCrunch.Framework
{
    using System;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class ExclusivelyUsesAttribute : ResourceUsageAttribute
    {
        public ExclusivelyUsesAttribute(params string[] resourceName)
            : base(resourceName) { }
    }
}