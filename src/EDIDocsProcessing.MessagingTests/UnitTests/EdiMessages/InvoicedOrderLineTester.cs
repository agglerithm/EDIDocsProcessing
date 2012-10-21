using System;
using System.Collections.Generic;
using System.Reflection;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.EdiMessages
{
    [TestFixture]
    public class InvoicedOrderLineTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = new InvoicedOrderLine();
        }

        #endregion

        private InvoicedOrderLine _sut;

        [Test]
        public void can_calculate_total_sales()
        {
            _sut.Quantity = 500;
            _sut.Price = (decimal) 4.25;
            _sut.TotalSales().ShouldEqual(500 * (decimal)4.25);
        }

        [Test]
        public void can_get_string_representation_of_Invoiced_order_line()
        {
            _sut.Quantity = 500;
            _sut.Price = (decimal)4.25;

            _sut.ToString().ShouldContainText(4.25D.ToString());
            _sut.ToString().ShouldContainText(500.ToString());


            PropertyTester.TestToStringBehavior(_sut);
        }

        [Test]
        public void test_properties()
        {
            PropertyTester.TestPropertyBehavior(_sut, null);
        }
    }

    public static class PropertyTester
    {
        public static void TestToStringBehavior<T>(T sut)
        {
            foreach (PropertyInfo propertyInfo in sut.GetType().GetProperties())
            {
                sut.ToString().ShouldContainText(propertyInfo.Name + ":");
            }
        }

        public static void TestPropertyBehavior<T>(T sut, ICollection<Type> typesToIgnore)
        {
            int intValue = 42;
            decimal decimalValue = (decimal) 4.2 ;
            string stringValue = "foo";

            foreach (PropertyInfo propertyInfo in sut.GetType().GetProperties())
            {
                Type type = propertyInfo.PropertyType;

                if (typesToIgnore != null && typesToIgnore.Contains(type)) continue;

                if (type == stringValue.GetType())
                    testProperty(sut, propertyInfo, stringValue);
                else if (type == decimalValue.GetType())
                    testProperty(sut, propertyInfo, decimalValue);
                else if (type == intValue.GetType())
                    testProperty(sut, propertyInfo, intValue);
                else Assert.Fail(string.Format("Property type {0} not handled", type));
            }
        }

        private static void testProperty<T, K>(T sut, PropertyInfo propertyInfo, K testValue)
        {
            propertyInfo.SetValue(sut, testValue, null);
            propertyInfo.GetValue(sut, null).ShouldEqual(testValue);
        }
    }
}