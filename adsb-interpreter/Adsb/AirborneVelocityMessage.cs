namespace ADSB.Interpreter.Adsb
{
    public class AirborneVelocityMessage : Message
    {
        public override string MessageType { get => 4.ToString(); }
        public override decimal GroundSpeed { get => base.GroundSpeed; set => base.GroundSpeed = value; }
        public override string Track { get => base.Track; set => base.Track = value; }
        public override decimal VerticalRate { get => base.VerticalRate; set => base.VerticalRate = value; }

        public AirborneVelocityMessage(string data) : base(data)
        {

        }
    }
}