namespace NCrunch.Generator.SpecflowPlugin
{
    /// <summary>
    /// Class which generates the ncrunch ExclusivelyUses attribute 
    /// on tests generated from SpecFlow scenarios which are
    /// decorated with an attribute which begins with @NCrunch.Framework.ExclusivelyUses. 
    /// Each word seperated by an underscore after this will be passed to the attribute
    /// </summary>
    internal class ExclusivelyUsesAttributeProvider : MultipleValueNCrunchAttributeProviderBase
    {
        protected override string AttributeName()
        {
            return NCrunchAttributeNames.NCrunchExclusivelyUses;
        }
    }
}