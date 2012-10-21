using System.Xml.Linq;
using EDIDocsProcessing.Common;

using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class XmlUtilityTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
        }

        [Test]
        public void can_set_value_for_existing_attribute()
        {
            XElement el = XElement.Parse("<fooBar foo=\"bar\" />");
            XMLUtilities.set_attribute("foo", "bel", el);
            Assert.That(el.Attribute("foo").Value == "bel");
        }

        [Test]
        public void will_ignore_null_element()
        {
            XMLUtilities.set_attribute("foo", "bar", null);
        }

        [Test]
        public void will_ignore_null_value()
        {
            XElement el = XElement.Parse("<fooBar foo=\"bar\" />");
            XMLUtilities.set_attribute("foo", null, el);
            Assert.That(el.Attribute("foo").Value == "bar");
        }

        [Test]
        public void will_return_value_from_missing_attribute()
        {
            XElement el = XElement.Parse("<fooBar foo=\"bar\" />");
            string str = XMLUtilities.test_attribute("snafu", el);
            Assert.IsNotNull(str);
        }

        [Test]
        public void will_return_value_from_missing_element()
        {
            XElement el = XElement.Parse("<fooBar foo=\"bar\" />");
            string str = XMLUtilities.test_element(el, "snafu");
            Assert.IsNotNull(str);
        }
    }
}