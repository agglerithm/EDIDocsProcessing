using EDIDocsProcessing.Core.DataAccess;
using FluentNHibernate;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core.DataAccess
{
    [TestFixture]
    public class UOWTest
    {
        private INHibernateUnitOfWork _sut;
        private Mock<ISessionSource> _sessSrc;
        private Mock<ISession> _sess;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _sessSrc = new Mock<ISessionSource>();
            _sess = new Mock<ISession>();
            _sessSrc.Setup(s => s.CreateSession()).Returns(_sess.Object);
            _sut = new NHibernateUnitOfWork(_sessSrc.Object);
        }

        [Test]
        public void can_remember_session()
        {
            _sut.Start();
            Assert.That(_sut.CurrentSession == _sess.Object);
        }
    }
}