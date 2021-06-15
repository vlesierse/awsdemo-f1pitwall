using System;

namespace F1Pitwall.Telemetry
{
    public struct WheelSurfaceType
    {
        public WheelSurfaceType(SurfaceType rl, SurfaceType rr, SurfaceType fl, SurfaceType fr)
        {
            ReerLeft = rl;
            ReerRight = rr;
            FrontLeft = fl;
            FrontRight = fr;
        }

        public WheelSurfaceType(ReadOnlySpan<byte> buffer)
        {
            ReerLeft = (SurfaceType)buffer[0];
            ReerRight = (SurfaceType)buffer[1];
            FrontLeft = (SurfaceType)buffer[2];
            FrontRight = (SurfaceType)buffer[3];
        }

        SurfaceType ReerLeft { get; }
        SurfaceType ReerRight { get; }
        SurfaceType FrontLeft { get; }
        SurfaceType FrontRight { get; }
    }
}
