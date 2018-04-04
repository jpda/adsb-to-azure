using System;

namespace ADSB.Interpreter.Adsb
{
    public class SurfacePositionMessage : Message
    {
        public override string MessageType { get => 2.ToString(); }
        public override decimal Altitude { get => base.Altitude; set => base.Altitude = value; }
        public override decimal GroundSpeed { get => base.GroundSpeed; set => base.GroundSpeed = value; }
        public override string Track { get => base.Track; set => base.Track = value; }
        public override decimal Latitude { get => base.Latitude; set => base.Latitude = value; }
        public override decimal Longitude { get => base.Longitude; set => base.Longitude = value; }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public SurfacePositionMessage(string data) : base(data)
        {
            var decParse = new Func<string, decimal>(x => decimal.Parse(x));
            Altitude = GetValue(11, decParse);
            GroundSpeed = GetValue(12, decParse);
            Track = GetValue<string>(13);
            Latitude = GetValue(14, decParse);
            Longitude = GetValue(15, decParse);
            IsOnGround = GetValue(21, x => bool.Parse(x));
        }
    }
}