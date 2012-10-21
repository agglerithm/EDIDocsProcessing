using System;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests
{
    public static class Extensions
    {
        public static void ShouldEqual<T>(this T actual, T expected)
        {
            Assert.That(actual, Is.EqualTo(expected));
        }

        public static void ShouldNotEqual<T>(this T actual, T expected)
        {
            Assert.That(actual, Is.Not.EqualTo(expected));
        }

        public static void ShouldBeTrue(this bool actual)
        {
            Assert.That(actual, Is.True);
        }

        public static void ShouldBeFalse(this bool actual)
        {
            Assert.That(actual, Is.False);
        }

        public static void ShouldContainText(this string actual, string substring)
        {
            Assert.That(actual, Text.Contains(substring));
        }

        public static void ShouldBeGreaterThanOrEqualTo<T>(this T actual, T expected) where T : IComparable
        {
            Assert.That(actual, Is.GreaterThanOrEqualTo(expected));
        }

        public static void ShouldBeSameAs<T>(this T actual, T expected)
        {
            Assert.That(actual, Is.SameAs(expected));
        }
    }
}