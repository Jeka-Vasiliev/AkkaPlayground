using Xunit;
using FluentAssertions;
using AkkaPlayground.MainDevice;

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
            var respond = probe.ExpectMsg<RespondTemperature>();
            respond.RequestId.Should().Be(2);
            respond.Value.Should().Be(24.0);
        }
    }
}
