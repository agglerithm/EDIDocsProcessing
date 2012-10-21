using System;
using EdiMessages;
using MassTransit;

namespace TestSubscriber
{
    public class Subscriber :Consumes<CreateOrderMessage>.All
    {

        public void Consume(CreateOrderMessage message)
        {
            Console.WriteLine("CreateOrderMessage received :");
            Console.WriteLine(message.ToString());

        }
    }
}