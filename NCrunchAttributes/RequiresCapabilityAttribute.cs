 // ReSharper disable once CheckNamespace
namespace NCrunch.Framework
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RequiresCapabilityAttribute : Attribute
    {
        public RequiresCapabilityAttribute(string capabilityName)
        {
            CapabilityName = capabilityName;
        }

        public string CapabilityName { get; private set; }
    }
}