namespace Specflow.NCrunch
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using TechTalk.SpecFlow.Generator;
    using TechTalk.SpecFlow.Generator.UnitTestConverter;
    using TechTalk.SpecFlow.Parser;

    public class NCrunchAttributeGeneratorProvider :
        ITestMethodTagDecorator,
        ITestClassTagDecorator,
        ITestMethodDecorator
    {
        public Dictionary<SpecFlowFeature, List<string>> featureTags = new Dictionary<SpecFlowFeature, List<string>>();

        public bool CanDecorateFrom(
            string tagName,
            TestClassGenerationContext generationContext,
            CodeMemberMethod testMethod) =>
            IsNCrunchAttributeIdentifier(tagName);

        public void DecorateFrom(
            string tagName,
            TestClassGenerationContext generationContext,
            CodeMemberMethod testMethod)
        {
            SetTestMethodCategories(testMethod, tagName);
        }

        public bool CanDecorateFrom(string tagName, TestClassGenerationContext generationContext)
        {
            if (!IsNCrunchAttributeIdentifier(tagName))
            {
                return false;
            }
            if (!NCrunchAttributeNames.RemovePrefixAndSuffix(tagName).Equals(NCrunchAttributeNames.RemovePrefixAndSuffix(NCrunchAttributeNames.NCrunchAtomic),StringComparison.OrdinalIgnoreCase))
            {
                // keep a list of features that need the attributes applying to every member
                // This excludes the atomic attribute, which need to go on the class
                var stringList = new List<string>();
                if (featureTags.ContainsKey(generationContext.Feature))
                {
                    stringList = featureTags[generationContext.Feature];
                }

                stringList.Add(tagName);
                featureTags[generationContext.Feature] = stringList;                
            }
            return true;
        }

        private void AddNCrunchAtomicAttributeToFeature(TestClassGenerationContext generationContext, string tagName)
        {
            generationContext.TestClass.CustomAttributes.Add(new CodeAttributeDeclaration(NCrunchAttributeNames.NCrunchAtomic));
        }

        public void DecorateFrom(string tagName, TestClassGenerationContext generationContext)
        {
            //we only want to add the Atomic attribute to the class
            if(NCrunchAttributeNames.RemovePrefixAndSuffix(tagName).Equals(NCrunchAttributeNames.RemovePrefixAndSuffix(NCrunchAttributeNames.NCrunchAtomic), StringComparison.OrdinalIgnoreCase)){
                AddNCrunchAtomicAttributeToFeature(generationContext, tagName);
            }
            
        }

        public bool CanDecorateFrom(
            TestClassGenerationContext generationContext,
            CodeMemberMethod testMethod) =>
            featureTags.ContainsKey(generationContext.Feature);

        public void DecorateFrom(
            TestClassGenerationContext generationContext,
            CodeMemberMethod testMethod)
        {
            foreach (string tagName in featureTags[generationContext.Feature])
            {
                SetTestMethodCategories(testMethod, tagName);
            }
        }

        public int Priority => 1000;
        public bool RemoveProcessedTags { get; } = false;
        public bool ApplyOtherDecoratorsForProcessedTags { get; } = false;

        private void SetTestMethodCategories(CodeMemberMethod testMethod, string tagName)
        {
            string[] strArray = tagName.Split(':');
            string nCrunchAttributeIdentifier = strArray.First();
            string str = strArray.Last();
            AddNCrunchAttributes(testMethod, nCrunchAttributeIdentifier, str.Split(',').ToArray());
        }

        private static bool IsNCrunchAttributeIdentifier(string category)
        {
            string str = category.Split(':').First();
            return str.StartsWith(NCrunchAttributeNames.NCrunchAttributePrefix, StringComparison.OrdinalIgnoreCase) | NCrunchAttributeNames.All().Select(NCrunchAttributeNames.RemovePrefixAndSuffix).ToList().Contains(str);
        }

        private void AddNCrunchAttributes(
            CodeMemberMethod testMethod,
            string nCrunchAttributeIdentifier,
            string[] nCrunchAttributeValues)
        {
            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchExclusivelyUses))
            {
                testMethod.CustomAttributes.Add(new CodeAttributeDeclaration(NCrunchAttributeNames.NCrunchExclusivelyUses, nCrunchAttributeValues.Select(v => new CodeAttributeArgument(new CodePrimitiveExpression(v))).ToArray()));
            }

            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchIsolated))
            {
                testMethod.CustomAttributes.Add(new CodeAttributeDeclaration(NCrunchAttributeNames.NCrunchIsolated));
            }

            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchSerial))
            {
                testMethod.CustomAttributes.Add(new CodeAttributeDeclaration(NCrunchAttributeNames.NCrunchSerial));
            }

            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchInclusivelyUses))
            {
                testMethod.CustomAttributes.Add(new CodeAttributeDeclaration(NCrunchAttributeNames.NCrunchInclusivelyUses, nCrunchAttributeValues.Select(v => new CodeAttributeArgument(new CodePrimitiveExpression(v))).ToArray()));
            }

            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchRequiresCapability))
            {
                testMethod.CustomAttributes.Add(new CodeAttributeDeclaration(NCrunchAttributeNames.NCrunchRequiresCapability, nCrunchAttributeValues.Select(v => new CodeAttributeArgument(new CodePrimitiveExpression(v))).ToArray()));
            }

            if (MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchCategory))
            {
                testMethod.CustomAttributes.Add(new CodeAttributeDeclaration(NCrunchAttributeNames.NCrunchCategory, nCrunchAttributeValues.Select(v => new CodeAttributeArgument(new CodePrimitiveExpression(v))).ToArray()));
            }

            if (!MatchesIdentifier(nCrunchAttributeIdentifier, NCrunchAttributeNames.NCrunchTimeout))
            {
                return;
            }

            testMethod.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    NCrunchAttributeNames.NCrunchTimeout,
                    new CodeAttributeArgument(new CodePrimitiveExpression(int.Parse(nCrunchAttributeValues.First(), CultureInfo.InvariantCulture)))));
        }

        private static bool MatchesIdentifier(
            string nCrunchAttributeIdentifier,
            string nCrunchAttributeName) =>
            nCrunchAttributeIdentifier == nCrunchAttributeName || nCrunchAttributeIdentifier == NCrunchAttributeNames.RemovePrefixAndSuffix(nCrunchAttributeName);
    }
}
