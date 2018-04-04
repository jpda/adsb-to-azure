namespace ADSB.Interpreter.Adsb
{
    public class SurveillanceIdMessage : Message
    {
        public override string MessageType { get => 6.ToString(); }
        public override decimal Altitude { get => base.Altitude; set => base.Altitude = value; }
        public override decimal GroundSpeed { get => base.GroundSpeed; set => base.GroundSpeed = value; }
        public override string Squawk { get => base.Squawk; set => base.Squawk = value; }
        public override bool SquawkChange { get => base.SquawkChange; set => base.SquawkChange = value; }
        public override bool Emergency { get => base.Emergency; set => base.Emergency = value; }
        public override bool SPI { get => base.SPI; set => base.SPI = value; }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public SurveillanceIdMessage(string data) : base(data) { }
    }
}