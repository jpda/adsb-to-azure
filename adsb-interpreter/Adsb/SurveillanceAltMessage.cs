namespace ADSB.Interpreter.Adsb
{
    public class SurveillanceAltMessage : Message
    {
        public override string MessageType { get => 5.ToString(); }
        public override decimal Altitude { get => base.Altitude; set => base.Altitude = value; }
        public override bool SquawkChange { get => base.SquawkChange; set => base.SquawkChange = value; }
        public override bool SPI { get => base.SPI; set => base.SPI = value; }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public SurveillanceAltMessage(string data) : base(data)
        {

        }
    }
}