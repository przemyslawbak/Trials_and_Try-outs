﻿using Company.Consumers;
using Contracts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GettingStarted.Tests
{
    [TestFixture]
    public class ConsumerTests
    {
        [Test]
        public void IsTrue()
        {
            // Classic syntax
            Assert.IsTrue(2 + 2 == 4);

            // Constraint Syntax
            Assert.That(2 + 2 == 4, Is.True);
            Assert.That(2 + 2 == 4);
        }

        [Test]
        public async Task IsTrueAsync()
        {
            await Task.Delay(100);

            // Classic syntax
            Assert.IsTrue(2 + 2 == 4);

            // Constraint Syntax
            Assert.That(2 + 2 == 4, Is.True);
            Assert.That(2 + 2 == 4);
        }

        [Test]
        public async Task SampleConsumerTest_Is_Sent_And_Consumed_And_()
        {
            await using ServiceProvider? provider = new ServiceCollection().AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<GettingStartedConsumer>();
            }).BuildServiceProvider(true);

            ITestHarness harness = provider.GetRequiredService<ITestHarness>();

            await harness.Start();

            var client = harness.GetRequestClient<GettingStartedConract>();

            await client.GetResponse<GettingStartedConract>(new //timeout :(
            {
                Value = "test_value_1"
            });

            Assert.IsTrue(await harness.Sent.Any<GettingStartedConract>());

            Assert.IsTrue(await harness.Consumed.Any<GettingStartedConract>());

            IConsumerTestHarness<GettingStartedConsumer> consumerHarness = harness.GetConsumerHarness<GettingStartedConsumer>();

            Assert.That(await consumerHarness.Consumed.Any<GettingStartedConract>());
        }
    }
}
