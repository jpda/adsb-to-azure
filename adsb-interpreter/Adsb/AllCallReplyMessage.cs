namespace ADSB.Interpreter.Adsb
{
    public class AllCallReplyMessage : Message
    {
        public override string MessageType { get => 8.ToString(); }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public AllCallReplyMessage(string data) : base(data) { }
    }
}