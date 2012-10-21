using System.Xml.Linq;
using EDIDocsProcessing.Common;

using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class TestXmlUtilities
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
        }

        [Test]
        public void can_set_and_get_attribute()
        {
            var el = new XElement("test", "foo");
            XMLUtilities.set_attribute("bar", "&&", el);
            string bar = XMLUtilities.test_attribute("bar", el);
            Assert.That(bar == "&&");
        }

        [Test]
        public void can_set_and_get_element()
        {
            var el = new XElement("test", "foo");
            XMLUtilities.set_element(el, "bar", "&&");
            string bar = XMLUtilities.test_element(el, "bar");
            Assert.That(bar == "&&");
        }

        [Test]
        public void will_not_blow_up_on_missing_attribute()
        {
            var el = new XElement("test", "foo");
            string bar = XMLUtilities.test_attribute("bar", el);
            Assert.That(bar == "");
        }

        [Test]
        public void will_not_blow_up_on_missing_element()
        {
            var el = new XElement("test", "foo");
            string bar = XMLUtilities.test_element(el, "bar");
            Assert.That(bar == "");
        }
    }
}