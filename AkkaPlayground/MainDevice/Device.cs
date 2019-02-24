using Akka.Actor;
using Akka.Event;

namespace AkkaPlayground.MainDevice
{
    public class Device : UntypedActor
    {
        private double? _lastTemperatureReading = null;

        public Device(string groupId, string deviceId)
        {
            GroupId = groupId;
            DeviceId = deviceId;
        }

        protected override void PreStart() => Log.Info($"Device actor {DeviceId} - {GroupId} started");
        protected override void PostStop() => Log.Info($"Device actor {DeviceId} - {GroupId} stopped");

        public ILoggingAdapter Log { get; } = Context.GetLogger();
        public string GroupId { get; }
        public string DeviceId { get; }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RequestTrackDevice req when req.GroupId.Equals(GroupId) && req.DeviceId.Equals(DeviceId):
                    Sender.Tell(DeviceRegistered.Instance);
                    break;
                case RequestTrackDevice req:
                    Log.Warning($"Ignoring TrackDevice request for {req.GroupId}-{req.DeviceId}.This actor is responsible for {GroupId}-{DeviceId}.");
                    break;
                case RecordTemperature record:
                    Log.Info($"Record temperature reading {record.Value} with {record.RequestId}");
                    _lastTemperatureReading = record.Value;
                    Sender.Tell(new TemperatureRecorded(record.RequestId));
                    break;
                case ReadTemperature read:
                    Sender.Tell(new RespondTemperature(read.ReguestId, _lastTemperatureReading));
                    break;
            }
        }

        public static Props Props(string groupId, string deviceId) =>
            Akka.Actor.Props.Create(() => new Device(groupId, deviceId));
    }
}
