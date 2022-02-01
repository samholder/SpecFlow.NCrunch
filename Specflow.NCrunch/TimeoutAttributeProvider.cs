namespace Specflow.NCrunch
{
    using System;
    using System.CodeDom;
    using System.Globalization;
    using TechTalk.SpecFlow.Generator.CodeDom;

    /// <summary>
    /// Implementation class which is responsible for generating the NCrunch attributes NCrunch.Framework.Timeout(int milliseconds)
    /// </summary>
    internal class TimeoutAttributeProvider : NCrunchAttributeProviderBase
    {
        protected override CodeAttributeDeclaration InternalProvideAttribute(CodeDomHelper codeDomHelper,
            CodeMemberMethod method,
            string nCrunchAttributeParameters)
        {
            return codeDomHelper.AddAttribute(method, AttributeName(), Convert.ToInt32(nCrunchAttributeParameters,CultureInfo.InvariantCulture));
        }

        protected override string AttributeName()
        {
            return NCrunchAttributeNames.NCrunchTimeout;
        }
    }
}