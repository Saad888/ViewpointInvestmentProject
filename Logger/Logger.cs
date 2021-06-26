using System;

namespace Project.Logging
{
    public class Logger
    {
        public string Service { get; set; }
        public ConsoleColor Color { get; set; }

        public Logger(string serviceName, ConsoleColor color = ConsoleColor.White)
        {
            Service = serviceName;
            Color = color;
        }

        /// <summary>
        /// Log message with provided color
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            Log(message, Color);
        }

        /// <summary>
        /// Log message with provided color
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public void Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now}] - [{Service}]: {message}");
        }
    }
}
