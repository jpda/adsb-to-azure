using System;

namespace ADSB.Interpreter.Adsb
{
    public class Message
    {
        public string RawMessage { get; set; }
        public string[] RawMessageSplit { get; set; }
        public virtual string MessageType { get; set; }
        public string TransmissionType { get; set; }
        public string SessionId { get; set; }
        public string AircraftId { get; set; }
        public string Hexident { get; set; }
        public string FlightId { get; set; }
        public DateTime MessageDate { get; set; }
        public DateTime MessageTime { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime LogTime { get; set; }
        public virtual string Callsign { get; set; }
        public virtual decimal Altitude { get; set; }
        public virtual decimal GroundSpeed { get; set; }
        public virtual string Track { get; set; }
        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }
        public virtual decimal VerticalRate { get; set; }
        public virtual string Squawk { get; set; }
        public virtual bool SquawkChange { get; set; }
        public virtual bool Emergency { get; set; }
        public virtual bool SPI { get; set; }
        public virtual bool IsOnGround { get; set; }

        protected Message(string data)
        {
            RawMessage = data;
            RawMessageSplit = data.Split(',');
            TransmissionType = GetValue<string>(1);
            SessionId = GetValue<string>(2);
            AircraftId = GetValue<string>(3);
            Hexident = GetValue<string>(4);
            FlightId = GetValue<string>(5);
            MessageDate = GetValue(6, x => DateTime.Parse(x));
            MessageTime = GetValue(7, x => DateTime.Parse(x));
            LogDate = GetValue(8, x => DateTime.Parse(x));
            LogTime = GetValue(9, x => DateTime.Parse(x));
            Callsign = GetValue<string>(10);
            Altitude = GetValue(11, x => decimal.Parse(x));
            GroundSpeed = GetValue(12, x => decimal.Parse(x));
            Track = GetValue<string>(13);
            Latitude = GetValue(14, x => decimal.Parse(x));
            Longitude = GetValue(15, x => decimal.Parse(x));
            VerticalRate = GetValue(16, x => decimal.Parse(x));
            Squawk = GetValue<string>(17);
            SquawkChange = GetValue(18, x => x == "1");
            Emergency = GetValue(19, x => x == "1");
            SPI = GetValue(20, x => x == "1");
            IsOnGround = GetValue(21, x => x == "1");
        }

        public static Message Parse(string data)
        {
            var pieces = data.Split(',');
            var type = int.Parse(pieces[1]);

            switch (type)
            {
                case 1: return new IdentificationAndCategoryMessage(data);
                case 2: return new SurfacePositionMessage(data);
                case 3: return new AirbornePositionMessage(data);
                case 4: return new AirborneVelocityMessage(data);
                case 5: return new SurveillanceAltMessage(data);
                case 6: return new SurveillanceIdMessage(data);
                case 7: return new AirToAirMessage(data);
                case 8: return new AllCallReplyMessage(data);
                default: return new Message(data);
            }
        }

        public T GetValue<T>(int index, Func<string, T> convert = null)
        {
            var val = RawMessageSplit[index];
            if (string.IsNullOrEmpty(val)) return default(T);
            if (convert == null) return (T)(object)val;
            try
            {
                return convert(val);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return default(T);
        }
    }

    public class IdentificationAndCategoryMessage : Message
    {
        public override string MessageType { get => 1.ToString(); }
        public override string Callsign { get; set; }

        public IdentificationAndCategoryMessage(string data) : base(data)
        {
            Callsign = GetValue<string>(10);
        }
    }

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

    public class AirToAirMessage : Message
    {
        public override string MessageType { get => 7.ToString(); }
        public override decimal Altitude { get => base.Altitude; set => base.Altitude = value; }
        public override decimal GroundSpeed { get => base.GroundSpeed; set => base.GroundSpeed = value; }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public AirToAirMessage(string data) : base(data) { }
    }

    public class AllCallReplyMessage : Message
    {
        public override string MessageType { get => 8.ToString(); }
        public override bool IsOnGround { get => base.IsOnGround; set => base.IsOnGround = value; }

        public AllCallReplyMessage(string data) : base(data) { }
    }
}