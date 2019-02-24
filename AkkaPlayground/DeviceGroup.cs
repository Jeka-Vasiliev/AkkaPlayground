using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaPlayground
{
    public sealed class RequestTrackDevice
    {
        public RequestTrackDevice(string groupId, string deviceId)
        {
            GroupId = groupId;
            DeviceId = deviceId;
        }

        public string GroupId { get; }
        public string DeviceId { get; }
    }

    public sealed class DeviceRegistered
    {
        public static DeviceRegistered Instance { get; } = new DeviceRegistered();
        private DeviceRegistered() { }
    }

    class DeviceGroup
    {
    }
}
