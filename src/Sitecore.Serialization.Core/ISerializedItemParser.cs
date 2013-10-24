namespace Sitecore.Serialization.Core
{
    public interface ISerializedItemParser
    {
        string[] CorrectContents(string[] inputData);
    }
}