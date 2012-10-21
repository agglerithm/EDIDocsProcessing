using System.Collections.Generic;
using EDIDocsProcessing.Core;
using EdiMessages;
using MassTransit;
using MassTransitContrib;
using Moq;
using Moq.Language.Flow;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core
{
    [TestFixture]
    public class PublishEdiAsnSentSpecificationTester
    {
        private IPostActionSpecification _sut;
        private IServiceBus _serviceBus;
        private const string BOL = "Bol";
        private const string ControlNumber = "ControlNumber";
        private IList<ShippedLine> Lines= new List<ShippedLine>();
        private OrderHasBeenShippedMessage DefaultOrderHasBeenShippedMessage;

        [SetUp]
        public void Setup()
        {
            Lines = new List<ShippedLine>();
            Lines.Add(new ShippedLine(){LineNumber = "line1"});
            Lines.Add(new ShippedLine(){LineNumber = "line2"});
            Lines.Add(new ShippedLine(){LineNumber = "line3"});

            DefaultOrderHasBeenShippedMessage = new OrderHasBeenShippedMessage()
                                                    {
                                                        BOL = BOL,
                                                        ControlNumber = ControlNumber,
                                                        Lines = Lines
                                                    };

            var mock = new Mock<IMessagePublisher>();
            mock.Setup(bus => bus.Publish(It.Is<EdiAsnSentMessage>(msg => AssertEdiAsnSentMessageValues(msg))));

            _sut = new PublishEdiAsnSentSpecification(mock.Object);
        }

        [Test]
        public void should_be_satisfied_by_OrderHasBeenSentMessage()
        {
            var result = _sut.IsSatisfiedBy(DefaultOrderHasBeenShippedMessage);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void should_not_be_satisfied_by_ChangeOrderMessage()
        {
            var changeOrderMessage = new ChangeOrderMessage();

            var result = _sut.IsSatisfiedBy(changeOrderMessage);
            Assert.That(result, Is.False);
        }

        [Test]
        public void when_executing_should_publish_all_linenumbers()
        {
            _sut.Execute(DefaultOrderHasBeenShippedMessage);
        }

        private bool AssertEdiAsnSentMessageValues(EdiAsnSentMessage message)
        {
            Assert.That(message.BOL, Is.EqualTo(BOL));
            Assert.That(message.ControlNumber, Is.EqualTo(ControlNumber));
            Assert.That(message.LineNumbers.Count, Is.EqualTo(Lines.Count));
            return true;
        }
    }
}