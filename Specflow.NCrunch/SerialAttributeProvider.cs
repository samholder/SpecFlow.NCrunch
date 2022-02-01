namespace Specflow.NCrunch
{
    using System.CodeDom;
    using TechTalk.SpecFlow.Generator.CodeDom;

    /// <summary>
    /// Implementation class which is responsible for generating the NCrunch attributes NCrunch.Framework.Serial()
    /// </summary>
    internal class SerialAttributeProvider : NCrunchAttributeProviderBase
    {
        protected override CodeAttributeDeclaration InternalProvideAttribute(CodeDomHelper codeDomHelper,
            CodeMemberMethod method,
            string nCrunchAttributeParameters)
        {
            return codeDomHelper.AddAttribute(method, AttributeName());
        }

        protected override string AttributeName()
        {
            return NCrunchAttributeNames.NCrunchSerial;
        }
    }
}