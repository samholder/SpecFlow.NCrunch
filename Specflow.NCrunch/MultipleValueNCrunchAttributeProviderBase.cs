namespace Specflow.NCrunch
{
    using System.CodeDom;
    using System.Linq;
    using TechTalk.SpecFlow.Generator.CodeDom;

    /// <summary>
    /// Base class for attributes which have multiple parameters passed to the attribute
    /// </summary>
    internal abstract class MultipleValueNCrunchAttributeProviderBase : NCrunchAttributeProviderBase
    {
        protected override CodeAttributeDeclaration InternalProvideAttribute(CodeDomHelper codeDomHelper,
            CodeMemberMethod method,
            string nCrunchAttributeParameters)
        {
            object[] ncrunchAttributeValues = nCrunchAttributeParameters.Split('_').AsEnumerable<object>().ToArray();
            return codeDomHelper.AddAttribute(method, AttributeName(), ncrunchAttributeValues);
        }
    }
}