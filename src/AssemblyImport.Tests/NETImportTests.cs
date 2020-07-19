using NClass.AssemblyImport;
using NClass.CSharp;
using NClass.DiagramEditor.ClassDiagram;
using NReflect.Filter;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Threading;

namespace AssemblyImport.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class NETImportTests
    {
        [TestCase("ImportDefault.cs")]
        [TestCase("ImportMethods.cs")]
        [TestCase("ImportProperties.cs")]
        [TestCase("ImportEvents.cs")]
        [TestCase("ImportFields.cs")]
        [TestCase("ImportOperators.cs")]
        public void ImportFromSource(string fileName)
        {
            var assembly = AssemblyGenerator.Generate(fileName);
            var settings = GetDefaultSettings();
            var diagram = new ClassDiagram(CSharpLanguage.Instance);
            var importer = new NETImport(diagram, settings);

            importer.ImportAssembly(assembly.Location, useNewAppDomain: false).ShouldBeTrue();
            diagram.ToXml().ShouldMatchApproved(config => config.WithDescriminator(fileName));
        }

        private static ImportSettings GetDefaultSettings()
        {
            return new ImportSettings
            {
                Name = "<last used>",
                CreateAssociations = true,
                CreateGeneralizations = true,
                CreateNestings = true,
                CreateRealizations = true,
                CreateRelationships = true,
                LabelAggregations = true,
                FilterRules = new List<FilterRule> { new FilterRule(FilterModifiers.AllModifiers, FilterElements.AllElements) } 
            };
        }
    }
}
