using System;
using System.IO;
using System.Threading;


namespace EdgeDetection.GeneralHelper
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", $"log_{DateTime.Now:yyyyMMdd}.txt");

        static Logger()
        {
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void Info(string message)
        {
            Log("INFO", message);
        }

        public static void Error(string message, Exception ex = null)
        {
            var errorMsg = message;
            if (ex != null)
            {
                errorMsg += Environment.NewLine + ex.ToString();
            }

            Log("ERROR", errorMsg);
        }

        private static void Log(string level, string message)
        {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";
            lock (_lock)
            {
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }

            Console.WriteLine(logMessage);
        }
    }
}
