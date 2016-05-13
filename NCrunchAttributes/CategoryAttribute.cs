 // ReSharper disable once CheckNamespace
namespace NCrunch.Framework
{
    using System;

    [AttributeUsage(AttributeTargets.Method
                    | AttributeTargets.Class
                    | AttributeTargets.Field
                    | AttributeTargets.Assembly,
        AllowMultiple = true)]
    public sealed class CategoryAttribute: Attribute
    {
        public CategoryAttribute(string category)
        {
            Category = category;
        }

        public string Category { get; private set; }
    }
}