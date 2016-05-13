 // ReSharper disable once CheckNamespace
namespace NCrunch.Framework
{
    using System;
    using System.Collections;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class TimeoutAttribute : System.Attribute
    {
        private readonly IDictionary properties;

        public TimeoutAttribute(int timeout)
        {
            properties = new Hashtable();
            properties["Timeout"] = timeout;
        }

        public IDictionary Properties
        {
            get { return properties; }
        }
    }
}