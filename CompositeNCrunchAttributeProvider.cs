using System.CodeDom;
using System.Collections.Generic;
using TechTalk.SpecFlow.Utils;

namespace NCrunch.Generator.SpecflowPlugin
{
    /// <summary>
    ///     Attribute provider that just delegates to the list of all known providers
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class CompositeNCrunchAttributeProvider : INCrunchAttributeProvider
    {
        private readonly List<INCrunchAttributeProvider> attributeProviders = new List<INCrunchAttributeProvider>
        {
            new ExclusivelyUsesAttributeProvider(),
            new InclusivelyUsesAttributeProvider(),
            new CategoryAttributeProvider(),
            new IsolatedAttributeProvider(),
            new RequiresCapabilityAttributeProvider(),
            new SerialAttributeProvider(),
            new SerialAttributeProvider()
        };

        public CodeAttributeDeclaration ProvideAttribute(CodeDomHelper codeDomHelper, CodeMemberMethod method,
            string nCrunchAttributeIdentifier, string nCrunchAttributeParameters)
        {
            foreach (INCrunchAttributeProvider attributeProvider in attributeProviders)
            {
                CodeAttributeDeclaration attribute = attributeProvider.ProvideAttribute(codeDomHelper, method,
                    nCrunchAttributeIdentifier,
                    nCrunchAttributeParameters);

                if (attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}