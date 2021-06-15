using System;
using System.Linq;
using System.Numerics;

namespace F1Pitwall.Telemetry
{
    public record PacketMotionData : Packet
    {
        public PacketMotionData(ReadOnlyMemory<byte> data) : base(data)
        {
            var buffer = data.Span.Slice(24);
            Cars = Enumerable.Range(0, 22).Select(i => new CarMotionData(data.Span.Slice(24 + (i * 60), 60))).ToArray();
            SuspensionPosition = new WheelFloat(buffer.Slice(1320, 16));
            SuspensionVelocity = new WheelFloat(buffer.Slice(1336, 16));
            SuspensionAcceleration = new WheelFloat(buffer.Slice(1352, 16));
            WheelSpeed = new WheelFloat(buffer.Slice(1368, 16));
            WheelSlip = new WheelFloat(buffer.Slice(1384, 16));
            LocalVelocity = new Vector3(BitConverter.ToSingle(buffer.Slice(1400, 4)), BitConverter.ToSingle(buffer.Slice(1404, 4)), BitConverter.ToSingle(buffer.Slice(1408, 4)));
            AngularVelocity = new Vector3(BitConverter.ToSingle(buffer.Slice(1412, 4)), BitConverter.ToSingle(buffer.Slice(1416, 4)), BitConverter.ToSingle(buffer.Slice(1420, 4)));
            AngularAcceleration = new Vector3(BitConverter.ToSingle(buffer.Slice(1424, 4)), BitConverter.ToSingle(buffer.Slice(1428, 4)), BitConverter.ToSingle(buffer.Slice(1432, 4)));
            FrontWheelsAngle = BitConverter.ToSingle(buffer.Slice(1436, 4));
        }

        public CarMotionData[] Cars { get; private set; }

        public WheelFloat SuspensionPosition { get; }
        public WheelFloat SuspensionVelocity { get; }
        public WheelFloat SuspensionAcceleration { get; }
        public WheelFloat WheelSpeed { get; }
        public WheelFloat WheelSlip { get; }
        public Vector3 LocalVelocity { get; }
        public Vector3 AngularVelocity { get; }
        public Vector3 AngularAcceleration { get; }
        public float FrontWheelsAngle { get; }
    }
}
