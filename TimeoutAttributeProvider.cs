using System;
using System.CodeDom;
using TechTalk.SpecFlow.Utils;

namespace NCrunch.Generator.SpecflowPlugin
{
    using System.Globalization;

    /// <summary>
    /// Implementation class which is responsible for generating the NCrunch attributes NCrunch.Framework.Timeout(int milliseconds)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
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