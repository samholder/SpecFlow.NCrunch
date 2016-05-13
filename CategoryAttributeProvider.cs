namespace NCrunch.Generator.SpecflowPlugin
{
    /// <summary>
    ///     Implementation class which is responsible for generating the NCrunch attributes NCrunch.Framework.Category(string
    ///     categoryName)
    /// </summary>
    class CategoryAttributeProvider : SingleValueNCrunchAttributeProviderBase
    {
        protected override string AttributeName()
        {
            return NCrunchAttributeNames.NCrunchCategory;
        }
    }
}