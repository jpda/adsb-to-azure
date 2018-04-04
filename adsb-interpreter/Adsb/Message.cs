using Newtonsoft.Json;
using System;

namespace ADSB.Interpreter.Adsb
{
    public class Message
    {
        [JsonIgnore]
        public string RawMessage { get; set; }
        [JsonIgnore]
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
}