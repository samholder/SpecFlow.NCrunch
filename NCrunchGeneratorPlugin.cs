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
    ///     The NCrunch attributes generator plugin.
    /// </summary>
    public class NCrunchGeneratorPlugin : IGeneratorPlugin
    {
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters)
        {
            generatorPluginEvents.CustomizeDependencies+= (sender, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<NCrunchAttributeGeneratorProvider, IUnitTestGeneratorProvider>();
            };
        }
    }
}