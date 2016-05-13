using BoDi;
using NCrunch.Generator.SpecflowPlugin;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;

[assembly: GeneratorPlugin(typeof (NCrunchGeneratorPlugin))]

namespace NCrunch.Generator.SpecflowPlugin
{
    /// <summary>
    ///     The CodedUI generator plugin.
    /// </summary>
    public class NCrunchGeneratorPlugin : IGeneratorPlugin
    {
        /// <summary>
        ///     The register dependencies.
        /// </summary>
        /// <param name="container">
        ///     The container.
        /// </param>
        public void RegisterDependencies(ObjectContainer container)
        {
        }

        /// <summary>
        ///     The register customizations.
        /// </summary>
        /// <param name="container">
        ///     The container.
        /// </param>
        /// <param name="generatorConfiguration">
        ///     The generator configuration.
        /// </param>
        public void RegisterCustomizations(ObjectContainer container,
            SpecFlowProjectConfiguration generatorConfiguration)
        {
            container.RegisterTypeAs<NCrunchAttributeGeneratorProvider, IUnitTestGeneratorProvider>();
        }

        /// <summary>
        ///     The register configuration defaults.
        /// </summary>
        /// <param name="specFlowConfiguration">
        ///     The spec flow configuration.
        /// </param>
        public void RegisterConfigurationDefaults(SpecFlowProjectConfiguration specFlowConfiguration)
        {
        }
    }
}