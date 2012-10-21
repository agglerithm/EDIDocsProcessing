using System;
using System.Xml.Linq;
using EDIDocsProcessing.Common.Extensions;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class TestExtensions
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        [Test]
        public void can_cast_empty_string_to_int()
        {
            string num = "";
            Assert.That(num.CastToInt() == 0);
        }

        [Test]
        public void can_cast_null_string_to_int()
        {
            string num = null;
            Assert.That(num.CastToInt() == 0);
        }

        [Test]
        public void can_cast_to_bool()
        {
            Assert.IsTrue("true".CastToBool());
            Assert.IsTrue("True".CastToBool());

            Assert.IsFalse("false".CastToBool());
        }

        [Test]
        public void can_cast_to_date_time()
        {
            string dte = DateTime.Today.ToString("MM/dd/yyyy");
            Assert.That(dte.CastToDateTime() == DateTime.Today);
        }

        [Test]
        public void can_cast_to_double()
        {
            string dbl = "0.01";
            Assert.That(dbl.CastToDouble() == 0.01);
        }

        [Test]
        public void can_cast_to_int()
        {
            string num = "1,135";
            Assert.That(num.CastToInt() == 1135);
        }

        [Test]
        public void can_create_byte_array_from_string()
        {
            string str = "hello";
            byte[] buff = str.ToByteArray();
            Assert.That(buff[2] == 108);
        }

        [Test]
        public void can_do_safe_attribute_remove()
        {
            var el = new XElement("foo");
            el.Attribute("bar").SafeRemove();
        }

        [Test]
        public void can_do_safe_element_remove()
        {
            var el = new XElement("foo");
            el.Element("bar").SafeRemove();
        }

        [Test]
        public void can_enclose_in_tag()
        {
            string str = "Here is the string";
            string tag_str = str.EncloseInTag("<EL>");
            Assert.That(tag_str == "<EL>" + str + "</EL>");
        }

        [Test]
        public void can_extract_closing_tag()
        {
            string str = "<EL>";
            string tag_str = str.ExtractClosingTag();
            Assert.That(tag_str == "</EL>");
        }

        [Test]
        public void can_get_safe_attribute_value()
        {
            var el = new XElement("foo");
            string str = el.Attribute("bar").GetSafeValue();
            Assert.IsNull(str);
        }

        [Test]
        public void can_get_safe_element_value()
        {
            var el = new XElement("foo");
            string str = el.Element("bar").GetSafeValue();
            Assert.IsNull(str);
        }

        [Test]
        public void can_perform_safe_replace()
        {
            string str = "Here is the string with 'crap' in it.";
            string nstr = str.SafeReplace("", "daisies");
            Assert.That(nstr == str);
            nstr = str.SafeReplace("crap", "daisies");
            Assert.That(nstr == "Here is the string with 'daisies' in it.");
        }

        [Test]
        public void can_put_string_in_title_case()
        {
            string str = "hello";
            string title_str = str.TitleCase();
            Assert.That(title_str == "Hello");
        }

        [Test]
        public void can_translate_to_and_from_base64()
        {
            string str = "Hello";
            string base64str = str.ToBase64();
            Assert.That(base64str != str);
            string translated_str = base64str.FromBase64();
            Assert.That(str == translated_str);
        }

        [Test, ExpectedException(typeof (Exception))]
        public void will_fail_enclosing_into_invalid_tag()
        {
            string str = "Here is the string";
            string tag_str = str.EncloseInTag("<EL<");
        }
    }
}