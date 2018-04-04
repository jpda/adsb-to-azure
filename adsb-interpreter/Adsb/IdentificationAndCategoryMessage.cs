namespace ADSB.Interpreter.Adsb
{
    public class IdentificationAndCategoryMessage : Message
    {
        public override string MessageType { get => 1.ToString(); }
        public override string Callsign { get; set; }

        public IdentificationAndCategoryMessage(string data) : base(data)
        {
            Callsign = GetValue<string>(10);
        }
    }
}