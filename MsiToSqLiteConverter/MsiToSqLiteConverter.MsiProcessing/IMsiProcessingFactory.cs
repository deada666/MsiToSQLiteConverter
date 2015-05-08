
namespace MsiToSqLiteConverter.MsiProcessing
{
    public interface IMsiProcessingFactory
    {
        IMsiParser CreateMsiParser(string msiPath);

        IMsiWriter CreateMsiWriter(string msiPath);
    }
}
