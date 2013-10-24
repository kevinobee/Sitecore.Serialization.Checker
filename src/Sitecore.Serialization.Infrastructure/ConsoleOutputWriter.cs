using System;
using System.Collections.Generic;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Infrastructure
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string message)
        {
            WriteLine(MessageType.Info, message);
        }

        public void WriteLine(MessageType messageType, string message)
        {
            if (messageType != MessageType.Info)
            {
                SetMessageColor(messageType);
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteFormatLine(MessageType messageType, string format, params object[] arg)
        {
            var message = string.Format(format, arg);

            WriteLine(messageType, message);
        }

        public void Info(string message)
        {
            WriteLine(MessageType.Info, message);
        }

        public void Warn(string message)
        {
            WriteLine(MessageType.Warn, message);
        }

        public void Fail(string message)
        {
            WriteLine(MessageType.Fail, message);
        }

        public void Success(string message)
        {
            WriteLine(MessageType.Success, message);
        }

        private static void SetMessageColor(MessageType messageType)
        {
            var messageTypeColors = new Dictionary<MessageType, ConsoleColor>
                {
                    { MessageType.Fail, ConsoleColor.Red },
                    { MessageType.Warn, ConsoleColor.DarkYellow },
                    { MessageType.Success, ConsoleColor.Green }
                };

            ConsoleColor color;

            if (messageTypeColors.TryGetValue(messageType, out color))
            {
                Console.ForegroundColor = color;
            }
        }
    }
}