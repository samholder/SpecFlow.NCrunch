using System.CodeDom;
using TechTalk.SpecFlow.Utils;

namespace NCrunch.Generator.SpecflowPlugin
{
    /// <summary>
    ///     Base class for attribute providers which create an attributeb assuming that the attribute parameters are just a
    ///     single string that is passed to the attribute as the only parameter
    /// </summary>
    internal abstract class SingleValueNCrunchAttributeProviderBase : NCrunchAttributeProviderBase
    {
        protected override CodeAttributeDeclaration InternalProvideAttribute(CodeDomHelper codeDomHelper,
            CodeMemberMethod method,
            string nCrunchAttributeParameters)
        {
            return codeDomHelper.AddAttribute(method, AttributeName(), nCrunchAttributeParameters);
        }
    }
}