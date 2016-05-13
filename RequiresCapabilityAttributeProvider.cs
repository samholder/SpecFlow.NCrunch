namespace NCrunch.Generator.SpecflowPlugin
{
    /// <summary>
    ///     Implementation class which is responsible for generating the NCrunch attributes
    ///     NCrunch.Framework.RequiresCapability(string capabilityName)
    /// </summary>
    internal class RequiresCapabilityAttributeProvider : SingleValueNCrunchAttributeProviderBase
    {
        protected override string AttributeName()
        {
            return NCrunchAttributeNames.NCrunchRequiresCapability;
        }
    }
}