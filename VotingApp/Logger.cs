using System;

namespace VotingApp
{
    static class Logger
    {
        public static string Log { get; private set; }

        public static void WriteLog(Exception ex, string message)
        {
            Log += ex.Message + message + "\n";
        }

        public static void WriteLog(string message)
        {
            Log += message + "\n";
        }
    }
}
