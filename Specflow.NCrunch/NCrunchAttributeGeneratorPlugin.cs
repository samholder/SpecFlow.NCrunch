using Specflow.NCrunch;
using TechTalk.SpecFlow.Infrastructure;

[assembly:GeneratorPlugin(typeof(NCrunchAttributeGeneratorPlugin))]

namespace Specflow.NCrunch
{
    using TechTalk.SpecFlow.Generator.Plugins;
    using TechTalk.SpecFlow.Generator.UnitTestConverter;
    using TechTalk.SpecFlow.UnitTestProvider;

    public class NCrunchAttributeGeneratorPlugin : IGeneratorPlugin
    {
        public void Initialize(
            GeneratorPluginEvents generatorPluginEvents, 
            GeneratorPluginParameters generatorPluginParameters,
            UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            generatorPluginEvents.RegisterDependencies += this.GeneratorPluginEventsOnRegisterDependencies;
        }

        private void GeneratorPluginEventsOnRegisterDependencies(
            object sender,
            RegisterDependenciesEventArgs e)
        {
            var generatorProvider = new NCrunchAttributeGeneratorProvider();
            e.ObjectContainer.RegisterInstanceAs<ITestMethodTagDecorator>(generatorProvider, "NCrunch_Method_Tag", false);
            e.ObjectContainer.RegisterInstanceAs<ITestMethodDecorator>(generatorProvider, "NCrunch_Method", false);
            e.ObjectContainer.RegisterInstanceAs<ITestClassTagDecorator>(generatorProvider, "NCrunch_Class_Tag", false);
        }
    }
}