namespace ADSB.Interpreter.Adsb
{
    public class AirToAirMessage : Message
    {
        public override string MessageType { get => 7.ToString(); }
        public override decimal Altitude { get => base.Altitude; set => base.Altitude = value; }
        public override decimal GroundSpeed { get => base.GroundSpeed; set => base.GroundSpeed = value; }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public AirToAirMessage(string data) : base(data) { }
    }
}