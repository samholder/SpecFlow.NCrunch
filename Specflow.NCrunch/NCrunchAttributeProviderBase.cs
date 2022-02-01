namespace Specflow.NCrunch
{
    using System.CodeDom;
    using TechTalk.SpecFlow.Generator.CodeDom;

    /// <summary>
    ///     Base class for all attribute providers that will be used to provide the NCrunch attributes to decorate the specflow
    ///     generated tests
    /// </summary>
    internal abstract class NCrunchAttributeProviderBase : INCrunchAttributeProvider
    {
        public CodeAttributeDeclaration ProvideAttribute(CodeDomHelper codeDomHelper, CodeMemberMethod method,
            string nCrunchAttributeIdentifier,
            string nCrunchAttributeParameters)
        {
            if (nCrunchAttributeIdentifier == AttributeName())
            {
                return InternalProvideAttribute(codeDomHelper, method, nCrunchAttributeParameters);
            }

            return null;
        }

        protected abstract CodeAttributeDeclaration InternalProvideAttribute(CodeDomHelper codeDomHelper,
            CodeMemberMethod method, string nCrunchAttributeParameters);

        protected abstract string AttributeName();
    }
}