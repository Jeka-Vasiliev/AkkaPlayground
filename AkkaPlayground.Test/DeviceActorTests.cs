using Xunit;
using FluentAssertions;
using AkkaPlayground.MainDevice;
using System;

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

        [Fact]
        public void Device_actor_must_reply_with_latest_temperature_reading()
        {
            var probe = CreateTestProbe();
            var deviceActor = Sys.ActorOf(Device.Props("group", "device"));

            deviceActor.Tell(new RecordTemperature(requestId: 1, value: 24.0), probe);
            probe.ExpectMsg<TemperatureRecorded>(s => s.RequestId == 1);

            deviceActor.Tell(new ReadTemperature(reguestId: 2), probe);
            var response1 = probe.ExpectMsg<RespondTemperature>();
            response1.RequestId.Should().Be(2);
            response1.Value.Should().Be(24.0);

            deviceActor.Tell(new RecordTemperature(requestId: 3, value: 55.0), probe);
            probe.ExpectMsg<TemperatureRecorded>(s => s.RequestId == 3);

            deviceActor.Tell(new ReadTemperature(reguestId: 4), probe);
            var response2 = probe.ExpectMsg<RespondTemperature>();
            response2.RequestId.Should().Be(4);
            response2.Value.Should().Be(55.0);
        }
        
        [Fact]
        public void Device_actor_must_reply_to_registration_requests()
        {
            var probe = CreateTestProbe();
            var deviceActor = Sys.ActorOf(Device.Props("group", "device"));

            deviceActor.Tell(new RequestTrackDevice("group", "device"), probe.Ref);
            probe.ExpectMsg<DeviceRegistered>();
            probe.LastSender.Should().Be(deviceActor);
        }

        [Fact]
        public void Device_actor_must_ignore_wrong_registration_requests()
        {
            var probe = CreateTestProbe();
            var deviceActor = Sys.ActorOf(Device.Props("group", "device"));

            deviceActor.Tell(new RequestTrackDevice("wrongGroup", "device"), probe.Ref);
            probe.ExpectNoMsg(TimeSpan.FromMilliseconds(500));

            deviceActor.Tell(new RequestTrackDevice("group", "wrongDevice"), probe.Ref);
            probe.ExpectNoMsg(TimeSpan.FromMilliseconds(500));
        }
    }
}
