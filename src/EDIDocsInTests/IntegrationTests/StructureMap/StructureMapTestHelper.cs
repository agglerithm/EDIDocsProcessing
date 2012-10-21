using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests.StructureMap
{
    public static class StructureMapTestHelper
    {
        public static void AssertDefaultConcreteTypeResolvesCorrectly<TInterface, TConcrete>(IContainer container)
        {
            Assert.That(container.GetInstance<TInterface>().GetType(), Is.EqualTo(typeof(TConcrete)));
        }

        public static void AssertConcreteTypeResolvesCorretly<TConcrete>(IContainer container)
        {
            Assert.That(container.GetInstance<TConcrete>().GetType(), Is.EqualTo(typeof(TConcrete)));
        }

        public static void AssertResolvesCorrectlyWithKey<TInterface, TConcrete>(IContainer container, string key)
        {
            Assert.That(container.GetInstance<TInterface>(key).GetType(), Is.EqualTo(typeof(TConcrete)));
        }
    }
}