namespace NCrunch.Generator.SpecflowPlugin
{
    using System.Collections.Generic;

    /// <summary>
    /// Class holding the names of the attributes that NCrunch uses.
    /// </summary>
    public static class NCrunchAttributeNames
    {
        /// <summary>
        /// The prefix which all NCrunch attributes start with.  This is used to identify attributes in SpecFlow scenarios which should be converted to NCrunch attributes on the generated tests
        /// </summary>
        public static readonly string NCrunchAttributePrefix = "NCrunch.Framework.";

        public static readonly string AttributeSuffix = "Attribute";

        /// <summary>
        /// Identifier for the NCrunch ExclusivelyUses attribute
        /// </summary>
        public static readonly string NCrunchExclusivelyUses = NCrunchAttributePrefix + "ExclusivelyUses" + AttributeSuffix;

        /// <summary>
        /// Identifier for the NCrunch InclusivelyUses attribute
        /// </summary>
        public static readonly string NCrunchInclusivelyUses =  NCrunchAttributePrefix + "InclusivelyUses" + AttributeSuffix;

        /// <summary>
        /// Identifier for the NCrunch Isolated attribute
        /// </summary>
        public static readonly string NCrunchIsolated = NCrunchAttributePrefix + "Isolated" + AttributeSuffix;
        /// <summary>
        /// Identifier for the NCrunch Serial attribute
        /// </summary>
        public static readonly string NCrunchSerial = NCrunchAttributePrefix + "Serial" + AttributeSuffix;
        /// <summary>
        /// Identifier for the NCrunch RequiresCapability attribute
        /// </summary>
        public static readonly string NCrunchRequiresCapability = NCrunchAttributePrefix + "RequiresCapability" + AttributeSuffix;
        /// <summary>
        /// Identifier for the NCrunch Category attribute
        /// </summary>
        public static readonly string NCrunchCategory = NCrunchAttributePrefix + "Category" + AttributeSuffix;
        /// <summary>
        /// Identifier for the NCrunch Timeout attribute
        /// </summary>
        public static readonly string NCrunchTimeout = NCrunchAttributePrefix + "Timeout" + AttributeSuffix;

        public static IEnumerable<string> All()
        {
            yield return NCrunchIsolated;
            yield return NCrunchExclusivelyUses;
            yield return NCrunchInclusivelyUses;
            yield return NCrunchSerial;
            yield return NCrunchRequiresCapability;
            yield return NCrunchCategory;
            yield return NCrunchTimeout;
        }

        public static string RemovePrefixAndSuffix(string attributeName)
        {
            return attributeName.Replace(NCrunchAttributePrefix, "").Replace(AttributeSuffix,"");
        }
    }
}