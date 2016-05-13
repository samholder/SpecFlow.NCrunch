 // ReSharper disable once CheckNamespace
namespace NCrunch.Framework
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly)]
    public sealed class SerialAttribute : Attribute
    {
    }
}