namespace NCrunch.Generator.SpecflowPlugin
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using TechTalk.SpecFlow.Generator;
    using TechTalk.SpecFlow.Generator.Configuration;
    using TechTalk.SpecFlow.Generator.UnitTestProvider;
    using TechTalk.SpecFlow.Utils;

    public class NCrunchAttributeGeneratorProvider : IUnitTestGeneratorProvider
    {
        private readonly IUnitTestGeneratorProvider baseGeneratorProvider;
        private readonly CodeDomHelper codeDomHelper;

        public NCrunchAttributeGeneratorProvider(CodeDomHelper codeDomHelper, SpecFlowProjectConfiguration configuration)
        {
            string runtimeUnitTestProvider = configuration.RuntimeConfiguration.RuntimeUnitTestProvider;
            switch (runtimeUnitTestProvider.ToUpper(CultureInfo.InvariantCulture))
            {
                case "NUNIT":
                    baseGeneratorProvider = new NUnitTestGeneratorProvider(codeDomHelper);
                    break;
                case "MSTEST":
                    baseGeneratorProvider = new MsTest2010GeneratorProvider(codeDomHelper);
                    break;
                case "MSTEST.2010":
                    baseGeneratorProvider = new MsTest2010GeneratorProvider(codeDomHelper);
                    break;
                case "MSTEST.2008":
                    baseGeneratorProvider = new MsTestGeneratorProvider(codeDomHelper);
                    break;
                case "XUNIT":
                    baseGeneratorProvider = new XUnitTestGeneratorProvider(codeDomHelper);
                    break;
                case "MBUNIT":
                    baseGeneratorProvider = new MbUnitTestGeneratorProvider(codeDomHelper);
                    break;
                case "MBUNIT.3":
                    baseGeneratorProvider = new MbUnit3TestGeneratorProvider(codeDomHelper);
                    break;
                default:
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The specified unit test provider '{0}' is not usable with NCrunch wrapper",
                        runtimeUnitTestProvider));
            }

            this.codeDomHelper = codeDomHelper;
        }

       

        public UnitTestGeneratorTraits GetTraits()
        {
            return baseGeneratorProvider.GetTraits();
        }

        public void SetTestClass(TestClassGenerationContext generationContext, string featureTitle,
            string featureDescription)
        {
            baseGeneratorProvider.SetTestClass(generationContext, featureTitle, featureDescription);
        }

        public void SetTestClassCategories(TestClassGenerationContext generationContext,
            IEnumerable<string> featureCategories)
        {
            baseGeneratorProvider.SetTestClassCategories(generationContext, featureCategories);
        }

        public void SetTestClassIgnore(TestClassGenerationContext generationContext)
        {
            baseGeneratorProvider.SetTestClassIgnore(generationContext);
        }

        public void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
            baseGeneratorProvider.FinalizeTestClass(generationContext);
        }

        public void SetTestClassInitializeMethod(TestClassGenerationContext generationContext)
        {
            baseGeneratorProvider.SetTestClassInitializeMethod(generationContext);
        }

        public void SetTestClassCleanupMethod(TestClassGenerationContext generationContext)
        {
            baseGeneratorProvider.SetTestClassCleanupMethod(generationContext);
        }

        public void SetTestInitializeMethod(TestClassGenerationContext generationContext)
        {
            baseGeneratorProvider.SetTestInitializeMethod(generationContext);
        }

        public void SetTestCleanupMethod(TestClassGenerationContext generationContext)
        {
            baseGeneratorProvider.SetTestCleanupMethod(generationContext);
        }

        public void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            string scenarioTitle)
        {
            baseGeneratorProvider.SetTestMethod(generationContext, testMethod, scenarioTitle);
        }

        public void SetTestMethodCategories(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            IEnumerable<string> scenarioCategories)
        {
            var nonNCrunchCategories = scenarioCategories.Where(category => !IsNCrunchAttributeIdentifier(category)).ToList();
            baseGeneratorProvider.SetTestMethodCategories(generationContext, testMethod, nonNCrunchCategories);
            foreach (string nCrunchCategories in scenarioCategories.Where(IsNCrunchAttributeIdentifier))
            {
                string[] specFlowAttributeSplit = nCrunchCategories.Split(':');
                string nCrunchAttributeIdentifier = specFlowAttributeSplit.First();
                var nCrunchAttributeParameters = specFlowAttributeSplit.Last();

                AddNcrunchAttributes(testMethod, nCrunchAttributeIdentifier, nCrunchAttributeParameters.Split(',').ToArray());
            }
        }

        private static bool IsNCrunchAttributeIdentifier(string category)
        {
            var splitCategory = category.Split(':');
            var attributeName = splitCategory.First();
            var startsWithNCrunchAttributePrefix = attributeName.StartsWith(NCrunchAttributeNames.NCrunchAttributePrefix, StringComparison.OrdinalIgnoreCase);
            var allNcrunchAttributeNamesInSimpleForm = NCrunchAttributeNames.All().Select(NCrunchAttributeNames.RemovePrefixAndSuffix).ToList();
            var attributeMatchesAnExistingNCrunchAttributeInSimpleForm = allNcrunchAttributeNamesInSimpleForm.Contains(attributeName);
            return startsWithNCrunchAttributePrefix | attributeMatchesAnExistingNCrunchAttributeInSimpleForm;
        }



        public void SetTestMethodIgnore(TestClassGenerationContext generationContext, CodeMemberMethod testMethod)
        {
            baseGeneratorProvider.SetTestMethodIgnore(generationContext, testMethod);
        }

        public void SetRowTest(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            string scenarioTitle)
        {
            baseGeneratorProvider.SetRowTest(generationContext, testMethod, scenarioTitle);
        }

        public void SetRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            IEnumerable<string> arguments,
            IEnumerable<string> tags, bool isIgnored)
        {
            baseGeneratorProvider.SetRow(generationContext, testMethod, arguments, tags, isIgnored);
        }

        public void SetTestMethodAsRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            string scenarioTitle,
            string exampleSetName, string variantName, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            baseGeneratorProvider.SetTestMethodAsRow(generationContext, testMethod, scenarioTitle, exampleSetName,
                variantName, arguments);
        }

        private void AddNcrunchAttributes(CodeMemberMethod testMethod, string nCrunchAttributeIdentifier, string[] nCrunchAttributeValues)
        {
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchExclusivelyUses))
            {
                codeDomHelper.AddAttribute(testMethod, NCrunchAttributeNames.NCrunchExclusivelyUses, nCrunchAttributeValues);
            }
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchIsolated))
            {
                codeDomHelper.AddAttribute(testMethod, NCrunchAttributeNames.NCrunchIsolated);
            }
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchSerial))
            {
                codeDomHelper.AddAttribute(testMethod, NCrunchAttributeNames.NCrunchSerial);
            }
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchInclusivelyUses))
            {
                codeDomHelper.AddAttribute(testMethod, NCrunchAttributeNames.NCrunchInclusivelyUses, nCrunchAttributeValues);
            }
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchRequiresCapability))
            {
                codeDomHelper.AddAttribute(testMethod, NCrunchAttributeNames.NCrunchRequiresCapability, nCrunchAttributeValues);
            }
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchCategory))
            {
                codeDomHelper.AddAttribute(testMethod, NCrunchAttributeNames.NCrunchCategory, nCrunchAttributeValues);
            }
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchTimeout))
            {
                codeDomHelper.AddAttribute(testMethod, NCrunchAttributeNames.NCrunchTimeout, int.Parse(nCrunchAttributeValues.First(),CultureInfo.InvariantCulture));
            }
        }

        private static bool MatchesIdentifier(string nCrunchAttributeIdentifier, string nCrunchAttributeName)
        {
            return nCrunchAttributeIdentifier == nCrunchAttributeName || nCrunchAttributeIdentifier == NCrunchAttributeNames.RemovePrefixAndSuffix(nCrunchAttributeName);
        }
    }
}