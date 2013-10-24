namespace Sitecore.Serialization.Core
{
    public interface IOutputWriter
    {
        void WriteLine();
        void WriteLine(string message);
        void WriteLine(MessageType messageType, string message);
        void WriteFormatLine(MessageType messageType, string format, params object[] arg);

        void Info(string message);
        void Warn(string message);
        void Fail(string message);
        void Success(string message);
    }
}