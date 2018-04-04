namespace ADSB.Interpreter.Adsb
{
    public class AirbornePositionMessage : Message
    {
        public override string MessageType { get => 3.ToString(); }
        public override decimal Altitude { get => base.Altitude; set => base.Altitude = value; }
        public override decimal Latitude { get => base.Latitude; set => base.Latitude = value; }
        public override decimal Longitude { get => base.Longitude; set => base.Longitude = value; }
        public override bool SquawkChange { get => base.SquawkChange; set => base.SquawkChange = value; }
        public override bool Emergency { get => base.Emergency; set => base.Emergency = value; }
        public override bool SPI { get => base.SPI; set => base.SPI = value; }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public AirbornePositionMessage(string data) : base(data)
        {

        }
    }
}