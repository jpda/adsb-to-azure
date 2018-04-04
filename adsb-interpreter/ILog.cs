using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ADSB.Interpreter
{
    public interface ILog
    {
        void Critical(string message, [CallerMemberName] string caller = "");
        void Debug(string message, [CallerMemberName] string caller = "");
        void Debug(string message, IDictionary<string, string> props, [CallerMemberName] string caller = "");
        void Error(Exception ex, [CallerMemberName] string caller = "");
        void Error(string message, [CallerMemberName] string caller = "");
        void Event(string name, IDictionary<string, string> props = null, IDictionary<string, double> metric = null);
        void FailedRequest(string name, DateTimeOffset startTime, TimeSpan duration);
        void Flush();
        void Info(string message, [CallerMemberName] string caller = "");
        void Request(string name, DateTimeOffset startTime, TimeSpan duration);
        void Warn(string message, [CallerMemberName] string caller = "");
    }
}