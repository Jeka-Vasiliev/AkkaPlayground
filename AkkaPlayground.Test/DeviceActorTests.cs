using System;
using Xunit;
using AkkaPlayground;
using FluentAssertions;

namespace AkkaPlayground.Test
{
    public class DeviceActorSpec: Akka.TestKit.Xunit2.TestKit
    {
        [Fact]
        public void Device_actor_must_reply_with_empty_reading_if_no_temperature_is_known()
        {
            var probe = CreateTestProbe();
            var deviceActor = Sys.ActorOf(Device.Props("group", "device"));

            deviceActor.Tell(new ReadTemperature(42), probe.Ref);
            var response = probe.ExpectMsg<RespondTemperature>();
            response.RequestId.Should().Be(42);
            response.Value.Should().BeNull();
        }
    }
}
